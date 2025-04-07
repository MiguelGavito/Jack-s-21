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

    #region Inicializaci�n

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

    public void SetupNewRound()
    {
        
        deckManager.ClearHand(player1Transform);
        deckManager.ClearHand(player2Transform);
        Debug.Log("SetupNewRound, se limpio la mano");
        UpdateScores();

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


            //quitar luego
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

    #endregion

    #region Puntajes

    public void UpdateScores()
    {
        if (playerScoreText != null)
        {
            int playerScore = GetPlayerHandValue(player1Transform);
            playerScoreText.text = playerScore.ToString();
        }
        else Debug.LogError("playerScoreText no est� asignado en GameManager.");

        if (dealerScoreText != null)
        {
            int dealerScore = GetPlayerHandValue(player2Transform);
            dealerScoreText.text = dealerScore.ToString();
        }
        else Debug.LogError("dealerScoreText no est� asignado en GameManager.");
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
        while (GetPlayerHandValue(player2Transform) < 17)
        {
            yield return new WaitForSeconds(1f);

            Card newCard = deckManager.DrawCard(player2Transform);
            if (newCard != null) UpdateCard(newCard, true);

            UpdateScores();

            if (IsBusted(player2Transform))
            {
                Debug.Log("El dealer se pas� de 21");
                EventManager.Instance.EndDealerTurn();
                yield break;
            }
        }

        FlipDealerCards();
        EventManager.Instance.EndDealerTurn();
    }

    public void Stand()
    {
        Debug.Log("Turno del jugador finalizado.");
        EventManager.Instance.EndPlayerTurn();
    }

    void EndRound()
    {
        int playerScore = GetPlayerHandValue(player1Transform);
        int dealerScore = GetPlayerHandValue(player2Transform);

        bool playerBust = IsBusted(player1Transform);
        bool dealerBust = IsBusted(player2Transform);

        if (playerBust && dealerBust)
        {
            Debug.Log("Ambos se pasaron de 21. Nadie gana.");
        }
        else if (playerBust)
        {
            Debug.Log("El jugador se pas� de 21 y ha perdido.");
            lives--;
        }
        else if (dealerBust)
        {
            Debug.Log("El dealer se pas� de 21, el jugador gana.");
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
            Debug.Log("El jugador se qued� sin vidas. Fin del juego.");
        }
    }

    private IEnumerator DelayedStartRound()
    {
        yield return new WaitForSeconds(1f);
        eventManager.StartRound();
    }

    #endregion
}
