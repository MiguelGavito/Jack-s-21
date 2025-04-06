using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    // Points and stadistics
    public int playerGems = 100;
    public int playerBet = 0;

    public static GameManager instance = null;

    // Manager of the cards
    public DeckManager deckManager;
    public Card cardManager;
    public Transform player1Transform, player2Transform, discardTansform;

    // Puntajes
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI dealerScoreText;

    public MyUIManager uiManager;

    public EventManager eventManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            //return;
        }

        deckManager = Object.FindFirstObjectByType<DeckManager>();
        cardManager = Object.FindFirstObjectByType<Card>();

        //FindSceneReferences();
        // Asegurar que se repartan cartas antes de actualizar el puntaje
        Invoke("DelayedUpdateScores", 0.5f); // Espera 0.5 segundos


        EventManager.Instance.OnPlayerTurn += HandlePlayerTurn;
    }

    public void SetupNewRound()
    {
        deckManager.ClearHand(player1Transform);
        deckManager.ClearHand(player2Transform);

        PlayerDrawCard(player1Transform);
        PlayerDrawCard(player1Transform);

        PlayerDrawCardFaceDown(player2Transform);
        PlayerDrawCard(player2Transform);

        uiManager.UpdateHandValues();
    }

    public void ResetRound()
    {
        // Limpiar manos
        deckManager.ClearHand(player1Transform);
        deckManager.ClearHand(player2Transform);

        // Opcional: Resetear apuesta si lo manejas de forma dinámica
        // playerBet = 0;

        // Resetear textos de puntaje
        playerScoreText.text = "Jugador: 0";
        dealerScoreText.text = "Dealer: ?";

        // Repartir nuevas cartas
        StartCoroutine(DealInitialCards());
    }

    public IEnumerator DealInitialCards()
    {
        yield return new WaitForSeconds(1f); // Un poco de delay visual

        deckManager.DrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        deckManager.DrawCard(player2Transform);
        yield return new WaitForSeconds(0.5f);

        deckManager.DrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        Card hiddenCard = deckManager.DrawCard(player2Transform);
        hiddenCard?.TurnDown(); // Voltea la segunda carta del dealer
        yield return new WaitForSeconds(0.5f);

        // Actualizar valores en UI
        uiManager.UpdateHandValues();

        // Empezar turno del jugador
        EventManager.Instance.StartRound();
    }


    void HandlePlayerTurn()
    {
        Debug.Log("Es el turno del jugador. Puede robar cartas o plantarse.");
        // Aquí puedes activar botones de UI o controles para que el jugador actúe.
    }

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

    public void Stand()
    {
        Debug.Log("Turno del jugador finalizado.");
        EventManager.Instance.EndPlayerTurn();
        //StartDealerTurn(); // Iniciar el turno del dealer
    }

    void DelayedUpdateScores()
    {
        UpdateScores();
    }

    //averiguar como llamar a esta funcion en vez de directamente a decck
    public void PlayerDrawCard(Transform player)
    {
        if (player.childCount <= 5)
        {
            Card newCard = deckManager.DrawCard(player);
            if (newCard != null)
            {
                UpdateCard(newCard, true); // Voltear la carta
            }
            bool bustCheck = CheckAndAdjustIfBusted(player);
            UpdateScores();

            if (IsBusted(player))
            {
                Debug.Log("El jugador se paso de 21. Fin de turno en auto");
                EventManager.Instance.EndPlayerTurn();
            }
        }
    }

    public void PlayerDrawCardFaceDown(Transform player)
    {
        if (player.childCount <= 5)
        {
            Card newCard = deckManager.DrawCard(player);
            if (newCard != null)
            {
                UpdateCard(newCard, false); // Voltear la carta
            }
            UpdateScores();

        }
    }




    // Update is called once per frame
    void Update()
    {
        //Actualizar luego
        //
        //
        //


    }

    public int GetPlayerHandValue(Transform playerHand)
    {
        int totalValue = 0;

        foreach (Transform cardTransform in playerHand)
        {

            Card card = cardTransform.GetComponent<Card>();

            Debug.Log($"Mano {card.numero}");

            if (card != null && card.faceUp)
            {
                Debug.Log($"Mano {card.numero} y flip is {card.faceUp}");
                totalValue += card.numero; // Suma de valor de la carta
            }
        }
        return totalValue;
    }


    public void UpdateScores()
    {
        Debug.Log($"Cartas en la mano del jugador: {player1Transform.childCount}");
        Debug.Log($"Cartas en la mano del dealer: {player2Transform.childCount}");

        // Obtener y actualizar puntaje del jugador
        if (playerScoreText != null)
        {
            int playerScore = GetPlayerHandValue(player1Transform);
            Debug.Log($"Puntaje del jugador: {playerScore}");
            playerScoreText.text = playerScore.ToString();
        }
        else
        {
            Debug.LogError("playerScoreText no está asignado en GameManager.");
        }

        // Obtener y actualizar puntaje del dealer
        if (dealerScoreText != null)
        {
            int dealerScore = GetPlayerHandValue(player2Transform);
            Debug.Log($"Puntaje del dealer: {dealerScore}");
            dealerScoreText.text = dealerScore.ToString();
        }
        else
        {
            Debug.LogError("dealerScoreText no está asignado en GameManager.");
        }
    }

    public void UpdateCard(Card card, bool faceUp)
    {
        if (card != null)
        {
            if (faceUp)
            {
                card.TurnUp();
            }
            else
            {
                card.TurnDown();
            }
        }
    }

    public bool IsBusted(Transform playerHand)
    {
        return GetPlayerHandValue(playerHand) > 21;
    }

    void EndRound()
    {
        //Obtener el puntaje del jugador y el dealer
        int playerScore = GetPlayerHandValue(player1Transform);
        int dealerScore = GetPlayerHandValue(player2Transform);

        //verificar si se pasaron o no
        if (IsBusted(player1Transform))
        {
            Debug.Log("El jugador se pasó de 21 y ha perdido.");
            // El jugador pierde
            return;
        }
        if (IsBusted(player2Transform))
        {
            Debug.Log("El dealer se pasó de 21, el jugador gana.");
            // El jugador gana
            return;
        }

        //Comparar los puntajes
        if (playerScore > dealerScore)
        {
            Debug.Log($"El jugador gana con {playerScore} puntos contra {dealerScore} del dealer.");
            // El jugador gana
        }
        else if (playerScore < dealerScore)
        {
            Debug.Log($"El dealer gana con {dealerScore} puntos contra {playerScore} del jugador.");
            // El dealer gana
        }
        else
        {
            Debug.Log("Es un empate.");
            // Empate
        }
    }

    public void PlayerTurn()
    {
        Debug.Log("Empieza turno del jugador");
        // Activar botones de acción, permitir robar carta, etc.
    }

    public void DealerTurn()
    {
        Debug.Log("Empieza turno del dealer");
        StartCoroutine(DealerPlays());
    }

    private IEnumerator DealerPlays()
    {
        while (GetPlayerHandValue(player2Transform) < 17)
        {
            yield return new WaitForSeconds(1f);


            // El dealer sigeu tomando cartas hasta alcanzar al menos 17
            Card newCard = deckManager.DrawCard(player2Transform);
            if (newCard != null)
            {
                UpdateCard(newCard, true); // aniadir carta boca abajo
            }

            UpdateScores(); // Actualizar puntajes despues de tomar una carta

            // Si el dealer se pasa de 21 busted, termina el turno
            if (IsBusted(player2Transform))
            {
                Debug.Log("El dealer se paso de 21");
                EventManager.Instance.EndDealerTurn();
                yield break; // Termina la ejecucion si el dealer se pasa
            }
        }

        // Una vez el dealer tiene al menos 17 puntos, voltear las cartas
        FlipDealerCards(); // voltear las cartas boca abajo del dealer
        EventManager.Instance.EndDealerTurn(); //Terminar el turno del dealer
    }

    public void DealCardToDealer()
    {
        PlayerDrawCardFaceDown(player2Transform);
    }

    public void ScoreTurn()
    {
        Debug.Log("Ronda terminada, se evaluan resultados");
        int playerScore = GetPlayerHandValue(player1Transform);
        int dealerScore = GetPlayerHandValue(player2Transform);

        if (playerScore > 21)
        {
            Debug.Log("Jugador se paso, Pierde");
        }
        else if (dealerScore > 21 || playerScore > dealerScore)
        {
            Debug.Log("¡Jugador gana!");
        }
        else if (dealerScore == playerScore)
        {
            Debug.Log("Empate.");
        }
        else
        {
            Debug.Log("Dealer gana.");
        }

        // Aquí puedes reiniciar la ronda o ir a siguiente escena
    }

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerTurn += PlayerTurn;
        EventManager.Instance.OnDealerTurn += DealerTurn;
        EventManager.Instance.OnEndRound += EndRound;
    }

    private void OnDisable()
    {
        if (EventManager.Instance == null) return;
        EventManager.Instance.OnPlayerTurn -= PlayerTurn;
        EventManager.Instance.OnDealerTurn -= DealerTurn;
        EventManager.Instance.OnEndRound -= EndRound;
    }

    public void FlipDealerCards()
    {
        //Recorremos todas las cartas del dealer (su mano)
        foreach (Transform cardTransform in player2Transform)
        {
            Card card = cardTransform.GetComponent<Card>();

            if (card != null && card.IsFaceDown())
            {
                card.TurnUp();
            }
        }
    }


    public bool CheckAndAdjustIfBusted(Transform hand)
    {
        int total = deckManager.CalculateRawHandValue(hand);

        while (total > 21 && deckManager.CountAces(hand) > 0)
        {
            deckManager.AdjustAceValue(hand);
            total = deckManager.CalculateRawHandValue(hand);
        }

        return total > 21;
    }

    // EN ESTA SECCION VAN TODAS LAS FUNCIONES RELACIONADAS CON LA LECTURA Y COMPRBACION DE LA MANO

    public void EvaluateHands()
    {
        bool playerBusted = CheckAndAdjustIfBusted(player1Transform);
        bool dealerBusted = CheckAndAdjustIfBusted(player1Transform);

        int playerValue = deckManager.CalculateHandValue(player1Transform);
        int dealerValue = deckManager.CalculateHandValue(player2Transform);

        if (playerBusted && dealerBusted)
        {
            Debug.Log("Ambos perdieron. Empate triste ");
        }
        else if (playerBusted)
        {
            Debug.Log("Jugador se pasó. Dealer gana.");
        }
        else if (dealerBusted)
        {
            Debug.Log("Dealer se pasó. Jugador gana.");
        }
        else
        {
            if (playerValue > dealerValue)
            {
                Debug.Log("Jugador gana con " + playerValue);
            }
            else if (dealerValue > playerValue)
            {
                Debug.Log("Dealer gana con " + dealerValue);
            }
            else
            {
                Debug.Log("Empate.");
            }
        }
    }


    

}