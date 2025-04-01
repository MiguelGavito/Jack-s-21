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
        }

        deckManager = Object.FindFirstObjectByType<DeckManager>();
        cardManager = Object.FindFirstObjectByType<Card>();

    }

    public void PlayerDrawCard(int player)
    {
        if (player == 1)
            deckManager.DrawCard(player1Transform); // cambiar funcion por la de traslado
        else
        {
            deckManager.DrawCard(player2Transform);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
