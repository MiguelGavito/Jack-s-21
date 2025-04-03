using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Points and stadistics
    public int Gemas;
    public static GameManager instance = null;

    // Manager of the cards
    public DeckManager deckManager;
    public Card cardManager;
    public Transform player1Transform, player2Transform, discardTansform;

    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI dealerScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
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
    }


    /*
    // Despues pasare estos codigos para manejar los score y todo eso a un CardManager
    public void FindSceneReferences()
    {
        deckManager = Object.FindFirstObjectByType<DeckManager>();
        cardManager = Object.FindAnyObjectByType<Card>();

        player1Transform = GameObject.Find("Player1Hand")?.transform;
        player2Transform = GameObject.Find("Player2Hand")?.transform;

        playerScoreText = GameObject.Find("Player Scor")
    }
    */

    void DelayedUpdateScores()
    {
        UpdateScores();
    }

    //averiguar como llamar a esta funcion en vez de directamente a decck
    public void PlayerDrawCard(Transform player)
    {
        deckManager.DrawCard(player);
        UpdateScores();
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
        
    }

    public int GetPlayerHandValue(Transform playerHand)
    {
        int totalValue = 0;

        foreach (Transform cardTransform in playerHand)
        {
            Card card = cardTransform.GetComponent<Card>();
            if(card != null && !card.FlipUp)
            {
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

}
