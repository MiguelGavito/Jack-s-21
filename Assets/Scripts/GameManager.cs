using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    // Referencias a botones, UI y otros elementos
    public Button increaseBetButton;
    public Button standButton;
    public Button hitButton;

    // Referencias a las manos del jugador y el dealer
    public Transform playerHand, dealerHand, discardTansform;
    public TextMeshProUGUI playerScoreText, dealerScoreText;
    public TextMeshProUGUI winText;
    public GameObject loseScreen;
    public DeckManager deckManager;
    public EventManager eventManager;

    private int currentBet = 0;
    private bool isPlayerTurn = true;
    private bool gameOver = false;
    #endregion

    #region Unity Methods

    private void Start()
    {
        increaseBetButton.onClick.AddListener(IncreaseBet);
        standButton.onClick.AddListener(Stand);
        hitButton.onClick.AddListener(Hit);

        eventManager.StartRound();
    }

    void Update()
    {
        if (!gameOver)
        {
            if (isPlayerTurn)
            {
                playerScoreText.text = "Player: " + deckManager.CalculateHandValue(playerHand);
            }
            else
            {
                dealerScoreText.text = "Dealer: " + deckManager.CalculateHandValue(dealerHand);
            }
        }
    }
    #endregion

    public void EvaluateResults()
    {
        int playerScore = GetPlayerHandValue(playerHand);
        int dealerScore = GetPlayerHandValue(dealerHand);

        if (playerScore > 21)
        {
            Debug.Log("Jugador se pasó, pierde.");
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
    }

    #region Player Actions
    public void IncreaseBet()
    {
        currentBet += 10;
        Debug.Log("Apuesta aumentada a " + currentBet);
    }

    public void Stand()
    {
        Debug.Log("Turno del jugador finalizado.");
        EventManager.Instance.EndPlayerTurn();
        //StartDealerTurn(); // Iniciar el turno del dealer
    }

    public void Hit()
    {
        Debug.Log("Jugador pide carta");
        PlayerDrawCard(playerHand);
    }

    #endregion

    #region Round Control
    public void SetupNewRound()
    {
        deckManager.ClearHand(playerHand);
        deckManager.ClearHand(dealerHand);

        // Distribuir cartas iniciales
        eventManager.StartRound();
    }

    //public void EndTurn()
    //{
    //    if (isPlayerTurn)
    //    {
    //        isPlayerTurn = false;
    //        eventManager.DealerTurn();
    //    }
    //    else
    //    {
    //        EndGame();
    //    }
    //}

    public void EndGame()
    {
        // Lógica de fin de partida: comparar puntajes, mostrar resultados
    }
    #endregion


    public static GameManager instance = null;
    private int playerScore, dealerScore;
    public int playerGems = 100;
    public int playerBet = 0;
    // Manager of the cards
    //public DeckManager deckManager;
    //public Card cardManager;
    


    public MyUIManager uiManager;

    //public EventManager eventManager;

    public int playerLives = 5;
    //public int playerScore = 0;
    public int scoreGoal = 100;
    public int currentLevel = 1;





    // Regiones del codigo
    #region Inicializacion y Setup
    // Awake, SetupNewRound, StartNewGame, etc.

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
        //cardManager = Object.FindFirstObjectByType<Card>();

        //FindSceneReferences();
        // Asegurar que se repartan cartas antes de actualizar el puntaje
        Invoke("DelayedUpdateScores", 0.5f); // Espera 0.5 segundos


        EventManager.Instance.OnPlayerTurn += HandlePlayerTurn;
        EventManager.Instance.OnEndRound += EndRound;
    }

    //public void SetupNewRound()
    //{
    //    deckManager.ClearHand(playerHand);
    //    deckManager.ClearHand(dealerHand);

    //    PlayerDrawCard(playerHand);
    //    PlayerDrawCard(playerHand);

    //    PlayerDrawCardFaceDown(dealerHand);
    //    PlayerDrawCard(dealerHand);

    //    uiManager.UpdateHandValues();
    //}

    public void StartNewGame()
    {
        playerScore = 0;
        currentLevel = 1;
        playerLives = 5;
        playerGems = 100;
        playerBet = 0;

        ResetRound();
    }

    public void ResetRound()
    {
        // Limpiar manos
        deckManager.ClearHand(playerHand);
        deckManager.ClearHand(dealerHand);

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

        deckManager.DrawCard(playerHand);
        yield return new WaitForSeconds(0.5f);

        deckManager.DrawCard(dealerHand);
        yield return new WaitForSeconds(0.5f);

        deckManager.DrawCard(playerHand);
        yield return new WaitForSeconds(0.5f);

        Card hiddenCard = deckManager.DrawCard(dealerHand);
        hiddenCard?.TurnDown(); // Voltea la segunda carta del dealer
        yield return new WaitForSeconds(0.5f);

        // Actualizar valores en UI
        uiManager.UpdateHandValues();

        // Empezar turno del jugador
        EventManager.Instance.StartRound();
    }


    #endregion

    #region Turnos
    // PlayerTurn, DealerTurn, DealerPlays, Stand, etc.
    #endregion

    #region Puntajes
    // GetPlayerHandValue, UpdateScores, ScoreTurn, IsBusted, etc.
    #endregion

    #region Interacción Cartas
    // PlayerDrawCard, FlipDealerCards, etc.
    #endregion







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
        int playerScore = GetPlayerHandValue(playerHand);
        playerScoreText.text = $"{playerScore}";

        bool hasFaceDown = false;
        foreach (Transform cardTransform in dealerHand)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null && !card.faceUp)
            {
                hasFaceDown = true;
                break;
            }
        }

        if (hasFaceDown)
        {
            dealerScoreText.text = "Dealer: ?";
        }
        else
        {
            int dealerScore = GetPlayerHandValue(dealerHand);
            dealerScoreText.text = $"Dealer: {dealerScore}";
        }
    }


    /*
    public void UpdateScores()
    {
        Debug.Log($"Cartas en la mano del jugador: {playerHand.childCount}");
        Debug.Log($"Cartas en la mano del dealer: {dealerHand.childCount}");

        // Obtener y actualizar puntaje del jugador
        if (playerScoreText != null)
        {
            int playerScore = GetPlayerHandValue(playerHand);
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
            int dealerScore = GetPlayerHandValue(dealerHand);
            Debug.Log($"Puntaje del dealer: {dealerScore}");
            dealerScoreText.text = dealerScore.ToString();
        }
        else
        {
            Debug.LogError("dealerScoreText no está asignado en GameManager.");
        }
    }
    */

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
        int playerScore = deckManager.GetHandValue(playerHand);
        int dealerScore = deckManager.GetHandValue(dealerHand);

        //verificar si se pasaron o no
        if (IsBusted(playerHand))
        {
            Debug.Log("El jugador se pasó de 21 y ha perdido.");
            // El jugador pierde
            return;
        }
        if (IsBusted(dealerHand))
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
        FlipDealerCards(); // voltear las cartas boca abajo del dealer
        while (GetPlayerHandValue(dealerHand) < 17)
        {
            yield return new WaitForSeconds(1f);


            // El dealer sigeu tomando cartas hasta alcanzar al menos 17
            Card newCard = deckManager.DrawCard(dealerHand);
            if (newCard != null)
            {
                UpdateCard(newCard, true); // aniadir carta boca abajo
            }

            UpdateScores(); // Actualizar puntajes despues de tomar una carta

            // Si el dealer se pasa de 21 busted, termina el turno
            if (IsBusted(dealerHand))
            {
                Debug.Log("El dealer se paso de 21");
                EventManager.Instance.EndDealerTurn();
                yield break; // Termina la ejecucion si el dealer se pasa
            }
        }

        
        
        EventManager.Instance.EndDealerTurn(); //Terminar el turno del dealer
    }

    public void DealCardToDealer()
    {
        PlayerDrawCardFaceDown(dealerHand);
    }

    public void ScoreTurn()
    {
        Debug.Log("Ronda terminada, se evaluan resultados");
        int playerScore = GetPlayerHandValue(playerHand);
        int dealerScore = GetPlayerHandValue(dealerHand);

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
        foreach (Transform cardTransform in dealerHand)
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
        bool playerBusted = CheckAndAdjustIfBusted(playerHand);
        bool dealerBusted = CheckAndAdjustIfBusted(playerHand);

        int playerValue = deckManager.CalculateHandValue(playerHand);
        int dealerValue = deckManager.CalculateHandValue(dealerHand);

        if (playerBusted)
        {
            // mensaje de pérdida
            playerLives--;
        }
        else if (dealerBusted || playerValue > dealerValue)
        {
            // ganó jugador
            playerScore += 20;// puntosGanados;
        }
        else
        {
            // perdió o empate
            playerLives--;
        }

        // luego checas si continuar
        CheckGameProgress();
        /*
        if (playerBusted && dealerBusted)
        {
            Debug.Log("Ambos perdieron. Empate triste ");
            playerLives--;
        }
        else if (playerBusted)
        {
            Debug.Log("Jugador se pasó. Dealer gana.");
            playerLives--;
        }
        else if (dealerBusted)
        {
            Debug.Log("Dealer se pasó. Jugador gana.");
            playerLives--;
        }
        else
        {
            if (playerValue > dealerValue)
            {
                Debug.Log("Jugador gana con " + playerValue);
                playerScore += 20; //cambiar luego por puntosGanados
            }
            else if (dealerValue > playerValue)
            {
                Debug.Log("Dealer gana con " + dealerValue);
                playerLives--;
            }
            else
            {
                Debug.Log("Empate.");
            }
        }
        */
    }

    void CheckGameProgress()
    {
        if(playerLives <= 0 && playerScore < scoreGoal)
        {
            //Perdiste
            ShowLoseScreen();
        } else if (playerScore >= scoreGoal)
        {
            // Ganaste, pasas al siguietne nivel
            currentLevel++;
            scoreGoal += 50;
            playerLives = 5;
            SetupNewRound();
        }
        else
        {
            SetupNewRound();
        }
    }

    #region Show Results

    public void ShowWinScreen()
    {
        winText.text = "You Win!";
        winText.gameObject.SetActive(true);
        loseScreen.SetActive(false);  // Desactivar la pantalla de pérdida
    }

    public void ShowLoseScreen()
    {
        winText.text = "You Lose!";
        winText.gameObject.SetActive(true);
        loseScreen.SetActive(true);  // Activar la pantalla de pérdida
    }

    public void ShowDrawScreen()
    {
        winText.text = "It's a Draw!";
        winText.gameObject.SetActive(true);
        loseScreen.SetActive(false);  // Desactivar la pantalla de pérdida en caso de empate
    }
    #endregion
}