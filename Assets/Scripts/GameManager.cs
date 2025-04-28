using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    #region Variables Generales

    public Transform passiveItemParent;
    public GameObject passiveItemPrefab;

    public int playerGems
    {
        get => InventoryManager.instance.playerGems;
        set => InventoryManager.instance.playerGems = value;
    }

    public int playerBet = 0;

    public int lives = 5;

    public int puntaje = 0;

    public int record = 0;

    public int round;

    public int puntajeObj = 100;

    public int limiteCart = 21;

    public static GameManager instance;

    public DeckManager deckManager;
    // public Card cardManager;
    public Transform player1Transform, player2Transform, discardTansform;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI dealerScoreText;

    public MyUIManager uiManager;
    public EventManager eventManager;

    public DealerCommentManager commentManager;
    public DealerDialogueManager dialogueManager;
    


    #endregion

    #region Inicialización
    private void Start()
    {
        InventoryManager data = InventoryManager.instance;

        // Sincronizar estadisticas desdde el InventoryManager
        SincronizarEstadisticas();

        int playergems = data.playerGems;
        puntajeObj = data.PuntajeObjetivo;
        round = data.round;

        //Reinciiar valores de la ronda
        puntaje = 0;
        playerBet = 0;

        record = SaveManager.LoadHighScore();

        string greeting = commentManager.GetRandomComment("greetings");
        dialogueManager.Say(greeting);

        if (EventManager.Instance != null)
        {
            // Primero desuscribirse para evitar múltiples suscripciones
            EventManager.Instance.OnPlayerTurn -= PlayerTurn;
            EventManager.Instance.OnDealerTurn -= DealerTurn;
            EventManager.Instance.OnEndRound -= EndRound;

            // Luego volver a suscribirse
            EventManager.Instance.OnPlayerTurn += PlayerTurn;
            EventManager.Instance.OnDealerTurn += DealerTurn;
            EventManager.Instance.OnEndRound += EndRound;
        }
        else
        {
            Debug.LogError("GameManager: EventManager.Instance es null en Start()");
        }
    }

    private void SincronizarEstadisticas()
    {
        InventoryManager data = InventoryManager.instance;

        limiteCart = data.limiteCart;
        lives = data.lives;
        playerGems = data.playerGems;
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPlayerTurn -= PlayerTurn;
            EventManager.Instance.OnDealerTurn -= DealerTurn;
            EventManager.Instance.OnEndRound -= EndRound;
        }
    }
    #endregion

    #region Rondas

    public void ResetGame()
    {
        Debug.Log("Reseteando partida...");

        // Limpia referencias de cartas, manos, estados, variables, etc.
        foreach (Transform card in player1Transform)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform card in player2Transform)
        {
            Destroy(card.gameObject);
        }


        puntaje = 0;
        lives = 5;
        // cualquier otra variable de control o UI

        // También podrías resetear flags o pasivos si tienes
    }

    public void NuevaPartida()
    {
        InventoryManager.instance.ResetInventory(); // Borra gemas, ronda, ítems, etc.
        SceneManager.LoadScene("GameScene"); // Cambia por el nombre de tu escena del juego
    }

    // Método para aplicar el efecto pasivo de un objeto
    public void ApplyPassiveEffect(PassiveItem item)
    {
        item.UseItem(this);  // Llama directamente al UseItem para aplicar el efecto
    }

    public void LimpiarManos()
    {
        Debug.Log("Limpiar Manos, se limpio la mano");

        //deckManager.ClearHand(player1Transform);
        //deckManager.ClearHand(player2Transform);

        DiscardCard(player2Transform);
        DiscardCard(player1Transform);

        UpdateScores();
    }

    //Esta funcion genera muchos problemas, creo que hasta convendria quitarla pero pierde sentido la modulacion si asi lo hago
    public IEnumerator StartNewRound()
    {
        SincronizarEstadisticas();

        Debug.Log("startnweround ejecuta setupnewroundcoroutine");
        //StartCoroutine(SetupNewRoundCoroutine());
        PlayerDrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f); // o incluso 0.01f puede servir

        PlayerDrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        PlayerDrawCardFaceDown(player2Transform);
        yield return new WaitForSeconds(0.5f);

        PlayerDrawCard(player2Transform);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator SetupNewRoundCoroutine()
    {
        

        PlayerDrawCard(player1Transform);
        yield return new WaitForSeconds(1f); // o incluso 0.01f puede servir

        PlayerDrawCard(player1Transform);
        //yield return new WaitForSeconds(1f);

        PlayerDrawCardFaceDown(player2Transform);
        //yield return new WaitForSeconds(1f);

        PlayerDrawCard(player2Transform);
        //yield return new WaitForSeconds(1f);

        UpdateScores();

        Debug.Log("Para este punto deben estar las cartas en su lugar");
    }



    public void ResetRound()
    {
        deckManager.ClearHand(player1Transform);
        deckManager.ClearHand(player2Transform);

        playerScoreText.text = "Jugador: 0";
        dealerScoreText.text = "Dealer: ?";

        StartCoroutine(DealInitialCards());
    }

    public IEnumerator DealInitialCards()
    {
        yield return new WaitForSeconds(1f);
        PlayerDrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        PlayerDrawCard(player2Transform);
        yield return new WaitForSeconds(0.5f);

        PlayerDrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        PlayerDrawCardFaceDown(player2Transform);
        yield return new WaitForSeconds(0.5f);

        //uiManager.UpdateHandValues();
        // EventManager.Instance.StartRound();
    }

    #endregion

    #region Apuestas

    public void IncreaseBet(int amount)
    {
        if (playerGems >= amount)
        {
            playerGems -= amount;
            playerBet += amount;
            Debug.Log($"Apuesta aumentada en {amount}. Nueva apuesta: {playerBet}. Gemas restantes: {playerGems}");
        }
        else
        {
            Debug.Log("No tienes suficientes gemas para aumentar la apuesta.");
        }
    }

    #endregion

    #region Cartas

    public void DiscardCard(Transform player) 
    {
        while (player.childCount > 0)
        {
            deckManager.TakeCardFromHand(player, discardTansform);
        }
    }

    public void PlayerDrawCard(Transform player)
    {
        Debug.Log($"Robar una carta a {player}");
        if (player.childCount <= 5)
        {
            Card newCard = deckManager.DrawCard(player);
            if (newCard != null)
            {
                UpdateCard(newCard, true);
            }
        }
        UpdateScores();
    }

    public void PlayerDrawCardFaceDown(Transform player)
    {
        if (player.childCount <= 5)
        {
            Card newCard = deckManager.DrawCard(player);
            if (newCard != null)
            {
                UpdateCard(newCard, false);
            }
            
        }
        // UpdateScores();
    }

    public void UpdateCard(Card card, bool faceUp)
    {
        if (card != null)
        {
            if (faceUp) card.TurnUp();
            else card.TurnDown();
        }
    }

    public void FlipDealerCards()
    {
        foreach (Transform cardTransform in player2Transform)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null && card.IsFaceDown()) card.TurnUp();
        }
    }

    public void EndPlayerTurnIfBusted(Transform player) //checar luego para aniadir pausas
    {
        // Chequea si el jugador se ha pasado de 21 y termina el turno
        if (IsBusted(player))
        {
            Debug.Log("El jugador se pasó de 21. Fin de turno en automático");
            EventManager.Instance.EndPlayerTurn();
        }
    }
    #endregion

    #region Puntajes

    public void UpdateScores()
    {
        if (playerScoreText != null)
        {
            int playerScore = GetPlayerHandValue(player1Transform);
            playerScoreText.text = playerScore.ToString();
        }
        else Debug.LogError("playerScoreText no está asignado en GameManager.");

        if (dealerScoreText != null)
        {
            int dealerScore = GetPlayerHandValue(player2Transform);
            dealerScoreText.text = dealerScore.ToString();
        }
        else Debug.LogError("dealerScoreText no está asignado en GameManager.");
    }

    void DelayedUpdateScores()
    {
        UpdateScores();
    }

    public int GetPlayerHandValue(Transform playerHand)
    {
        int totalValue = 0;
        int aceCount = 0;

        foreach (Transform cardTransform in playerHand)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null && card.faceUp)
            {
                totalValue += card.numero;
                if (card.numero == 11 && card.IsAce()) // As que vale 11
                {
                    aceCount++;
                }
            }
        }

        // Ajustar ases de 11 a 1 si el total se pasa de 21
        while (totalValue > limiteCart && aceCount > 0)
        {
            totalValue -= 10; // convierte un As de 11 a 1
            aceCount--;
        }

        return totalValue;
    }

    public bool IsBusted(Transform playerHand)
    {
        return GetPlayerHandValue(playerHand) > limiteCart; // cambiar todos los 21 por una variables llamada maximo o limite
    }

    public bool CheckAndAdjustIfBusted(Transform hand)
    {
        int total = deckManager.CalculateRawHandValue(hand);

        while (total > limiteCart && deckManager.CountAces(hand) > 0)
        {
            deckManager.AdjustAceValue(hand);
            total = deckManager.CalculateRawHandValue(hand);
        }

        return total > limiteCart;
    }

    #endregion

    #region Turnos

    void HandlePlayerTurn()
    {
        Debug.Log("Es el turno del jugador. Puede robar cartas o plantarse.");
    }

    public void PlayerTurn()
    {
        Debug.Log("Empieza turno del jugador");
    }

    public void DealerTurn()
    {
        Debug.Log("Empieza turno del dealer");
        StartCoroutine(DealerPlays());
    }

    private IEnumerator DealerPlays()
    {

        // aqui llamo al script del dealer
        FlipDealerCards();

        yield return new WaitForSeconds(1f);

        while (GetPlayerHandValue(player2Transform) < 17)
        {
            yield return new WaitForSeconds(1f);

            Card newCard = deckManager.DrawCard(player2Transform);
            if (newCard != null) UpdateCard(newCard, true);

            UpdateScores();

            bool busted = IsBusted(player2Transform);// lo cambie, antes habia uno de check and ajust pero era raro
            UpdateScores();

            if (busted)
            {
                Debug.Log($"El dealer se pasó de {limiteCart}(incluso con ajustes de ases)");
                EventManager.Instance.EndDealerTurn();
                yield break;
            }
        }

        
        EventManager.Instance.EndDealerTurn();
    }



    public void Stand()
    {
        Debug.Log("Turno del jugador finalizado.");
        EventManager.Instance.EndPlayerTurn();
    }

    void EndRound()
    {
        StartCoroutine(EndRoundWithDelay());
    }

    private IEnumerator EndRoundWithDelay()
    {
        UpdateScores();

        int playerScore = GetPlayerHandValue(player1Transform);
        int dealerScore = GetPlayerHandValue(player2Transform);

        bool playerBust = IsBusted(player1Transform);
        bool dealerBust = IsBusted(player2Transform);

        int gemasExt;
        int puntajeExt;

        if (playerBust && dealerBust)
        {
            Debug.Log($"Ambos se pasaron de {limiteCart}. Nadie gana.");
            dialogueManager.Say(commentManager.GetRandomComment("draw"));
        }
        else if (playerBust)
        {
            Debug.Log($"El jugador se pasó de {limiteCart} y ha perdido.");
            dialogueManager.Say(commentManager.GetRandomComment("playerBust"));
            uiManager.MensajePerderRonda();

            InventoryManager.instance.lives--;
            playerBet = Mathf.FloorToInt(playerBet / 2);
            SincronizarEstadisticas();
        }
        else if (dealerBust)
        {
            Debug.Log($"El dealer se pasó de {limiteCart}, el jugador gana.");
            dialogueManager.Say(commentManager.GetRandomComment("dealerBust"));
            puntajeExt = Mathf.RoundToInt(20 * InventoryManager.instance.multiplicadorRecompensas);
            if (playerScore == 21)
            {
                puntajeExt += Mathf.RoundToInt(25 * InventoryManager.instance.multiplicadorRecompensas);
                dialogueManager.Say(commentManager.GetRandomComment("playerBlackjack"));
            }
            gemasExt = Mathf.RoundToInt(playerBet * 2 * InventoryManager.instance.multiplicadorRecompensas);

            puntaje += puntajeExt;
            InventoryManager.instance.playerGems += gemasExt;

            uiManager.MensajeGanarRonda(gemasExt, puntajeExt);
        }
        else if (playerScore > dealerScore)
        {
            Debug.Log($"El jugador gana con {playerScore} puntos contra {dealerScore} del dealer.");

            dialogueManager.Say(commentManager.GetRandomComment("playerWin"));

            puntajeExt = Mathf.RoundToInt(((playerScore - dealerScore) * 5 + 20) * InventoryManager.instance.multiplicadorRecompensas);
            puntaje += puntajeExt;

            gemasExt = Mathf.RoundToInt(playerBet * 3 * InventoryManager.instance.multiplicadorRecompensas);
            InventoryManager.instance.playerGems += gemasExt;

            uiManager.MensajeGanarRonda(gemasExt, puntajeExt);
        }
        else if (playerScore < dealerScore)
        {
            Debug.Log($"El dealer gana con {dealerScore} puntos contra {playerScore} del jugador.");
            uiManager.MensajePerderRonda();
            dialogueManager.Say(commentManager.GetRandomComment("dealerWin"));
            InventoryManager.instance.lives--;
            playerBet = Mathf.FloorToInt(playerBet / 2);
            SincronizarEstadisticas();
        }
        else
        {
            puntajeExt = Mathf.RoundToInt(15 * InventoryManager.instance.multiplicadorRecompensas);
            puntaje += puntajeExt;

            gemasExt = Mathf.RoundToInt(playerBet * InventoryManager.instance.multiplicadorRecompensas);
            InventoryManager.instance.playerGems += gemasExt;
            Debug.Log("Es un empate.");
            uiManager.MensajeEmpate(puntajeExt, gemasExt);
            dialogueManager.Say(commentManager.GetRandomComment("draw"));

        }

        yield return new WaitForSeconds(1f);

        if (InventoryManager.instance.lives > 0)
        {


            Debug.Log("Preparando nueva ronda...");
            StartCoroutine(DelayedStartRound());
        }
        else
        {
            //mensaje de que perdiste
            uiManager.MensajePerderJuego();

            Debug.Log("El jugador se quedó sin vidas. Fin del juego.");
            dialogueManager.Say(commentManager.GetRandomComment("dealerWin")); // comentario final opcional

            // Esperar 1 segundo antes de terminar el juego
            yield return new WaitForSeconds(1f);
            

            SceneManager.LoadScene(0); // Cargar pantalla de inicio

            // Resetear valores en el InventoryManager para una nueva partida
            InventoryManager.instance.ResetInventory();
        }

        // Codigo para actualizar record
        if (puntaje > record)
        {
            record = puntaje;
            SaveManager.SaveHighScore(record); // Guardamos el nuevo puntaje más alto.
        }
        if (puntaje > puntajeObj)
        {
            // Guardar progreso de ronda y gemas

            InventoryManager.instance.AvanzarRound();

            // Esperar 1 segundo antes de cargar la tienda
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(2); // cargar tienda
        }

        uiManager.UpdateUI();
    }

    private IEnumerator DelayedStartRound()
    {
        yield return new WaitForSeconds(3f);
        eventManager.StartRound();
    }

    #endregion

    #region Receptores

    public void OnHitButtonPressed()
    {
        PlayerDrawCard(player1Transform);
        UpdateScores();
        EndPlayerTurnIfBusted(player1Transform);
    }

    #endregion


}
