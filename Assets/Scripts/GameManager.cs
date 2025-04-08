using System.Collections;
using UnityEngine;
using TMPro;

// HAY QUE CORREGIR EL ERROR DE QUE EL DEALER NO REDUCE EL VALOR DE SU AS EN CASO DE NECESITARLO

public class GameManager : MonoBehaviour
{
    #region Variables Generales

    public int playerGems = 100;
    public int playerBet = 0;

    public int lives = 5;

    public int puntaje = 0;

    public int record = 0;

    public int puntajeObj = 0;

    public static GameManager instance = null;

    public DeckManager deckManager;
    // public Card cardManager;
    public Transform player1Transform, player2Transform, discardTansform;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI dealerScoreText;

    public MyUIManager uiManager;
    public EventManager eventManager;

    #endregion

    #region Inicialización

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
        }

        deckManager = Object.FindFirstObjectByType<DeckManager>();
        // cardManager = Object.FindFirstObjectByType<Card>();

        Invoke("DelayedUpdateScores", 0.5f);
        EventManager.Instance.OnPlayerTurn += HandlePlayerTurn;
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

    #endregion

    #region Rondas

    public void LimpiarManos()
    {
        deckManager.ClearHand(player1Transform);
        deckManager.ClearHand(player2Transform);
        Debug.Log("Limpiar Manos, se limpio la mano");
        UpdateScores();
    }

    //Esta funcion genera muchos problemas, creo que hasta convendria quitarla pero pierde sentido la modulacion si asi lo hago
    public void SetupNewRound()
    {
        
        

        PlayerDrawCard(player1Transform);
        PlayerDrawCard(player1Transform);

        PlayerDrawCardFaceDown(player2Transform);
        PlayerDrawCard(player2Transform);

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
        deckManager.DrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        deckManager.DrawCard(player2Transform);
        yield return new WaitForSeconds(0.5f);

        deckManager.DrawCard(player1Transform);
        yield return new WaitForSeconds(0.5f);

        Card hiddenCard = deckManager.DrawCard(player2Transform);
        hiddenCard?.TurnDown();
        yield return new WaitForSeconds(0.5f);

        //uiManager.UpdateHandValues();
        EventManager.Instance.StartRound();
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

    public void PlayerDrawCard(Transform player)
    {
        if (player.childCount <= 5)
        {
            Card newCard = deckManager.DrawCard(player);
            if (newCard != null)
            {
                UpdateCard(newCard, true);
            }

            
            
            UpdateScores();

        }
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
            UpdateScores();
        }
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

    public void EndPlayerTurnIfBusted(Transform player)
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
        foreach (Transform cardTransform in playerHand)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null && card.faceUp)
            {
                totalValue += card.numero;
            }
        }
        return totalValue;
    }

    public bool IsBusted(Transform playerHand)
    {
        return GetPlayerHandValue(playerHand) > 21;
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
        FlipDealerCards();

        yield return new WaitForSeconds(1f);

        while (GetPlayerHandValue(player2Transform) < 17)
        {
            yield return new WaitForSeconds(1f);

            Card newCard = deckManager.DrawCard(player2Transform);
            if (newCard != null) UpdateCard(newCard, true);

            UpdateScores();

            bool busted = CheckAndAdjustIfBusted(player2Transform);
            UpdateScores();

            if (busted)
            {
                Debug.Log("El dealer se pasó de 21(incluso con ajustes de ases)");
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
        UpdateScores();
        int playerScore = GetPlayerHandValue(player1Transform);
        int dealerScore = GetPlayerHandValue(player2Transform);

        bool playerBust = IsBusted(player1Transform);
        bool dealerBust = IsBusted(player2Transform);

        UpdateScores();

        if (playerBust && dealerBust)
        {
            Debug.Log("Ambos se pasaron de 21. Nadie gana.");
        }
        else if (playerBust)
        {
            Debug.Log("El jugador se pasó de 21 y ha perdido.");
            lives--;
        }
        else if (dealerBust)
        {
            Debug.Log("El dealer se pasó de 21, el jugador gana.");
        }
        else if (playerScore > dealerScore)
        {
            Debug.Log($"El jugador gana con {playerScore} puntos contra {dealerScore} del dealer.");
        }
        else if (playerScore < dealerScore)
        {
            Debug.Log($"El dealer gana con {dealerScore} puntos contra {playerScore} del jugador.");
        }
        else
        {
            Debug.Log("Es un empate.");
        }

        if (lives > 0)
        {
            Debug.Log("Preparando nueva ronda...");
            StartCoroutine(DelayedStartRound());
        }
        else
        {
            Debug.Log("El jugador se quedó sin vidas. Fin del juego.");
        }
    }

    private IEnumerator DelayedStartRound()
    {
        yield return new WaitForSeconds(1f);
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
