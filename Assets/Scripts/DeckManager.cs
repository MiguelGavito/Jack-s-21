using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform deckTransform, discardTransform;
    public Transform player1Transform, player2Transform;
    private Stack<GameObject> deck = new Stack<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateDeck();
        ShuffleDeck();
    }

    public void PruebaAddCard()
    {
        deck.Push(cardPrefab);
    }

    void GenerateDeck()
    {
        string[] palos = { "Trebol", "Diamante", "Picas", "Corazon" };

        for (int i = 0; i < palos.Length; i++)
        {
            for (int j = 0; j <= 12; j++)
            {
                GameObject newCard = Instantiate(cardPrefab, deckTransform);
                string iden = (i * 13 + j).ToString();
                Sprite sprite = Resources.Load<Sprite>($"sprites/Deck-of-Cards/{iden}");

                Image image = newCard.GetComponentInChildren<Image>();
                if (image == null)
                    Debug.LogError(" No se encontró el Image en la carta.");
                else
                {
                    // Asignar el sprite al Image
                    image.sprite = Resources.Load<Sprite>($"sprites/Deck-of-Cards/{iden}");
                }

                // Cargar el texto
                TextMeshProUGUI titulo = newCard.GetComponentInChildren<TextMeshProUGUI>();
                if (titulo == null)
                    Debug.LogError(" No se encontró TextMeshProUGUI en la carta.");
                else
                    titulo.text = palos[i];

                newCard.GetComponent<Card>().SetCard(palos[i], j + 2, j + 2, image, titulo);
                deck.Push(newCard);
            }
        }

    }

    void ShuffleDeck()
    {
        List<GameObject> deckList = new List<GameObject>(deck);
        deck.Clear();

        while (deckList.Count > 0)
        {
            int randomIndex = Random.Range(0, deckList.Count);
            deck.Push(deckList[randomIndex]);
            deckList.RemoveAt(randomIndex);
        }
    }

    public void DrawCard(Transform playerTransform)
    {
        if (deck.Count > 0)
        {
            GameObject drawnCard = deck.Pop();

            drawnCard.transform.SetParent(playerTransform);

            drawnCard.transform.localPosition = new Vector3(playerTransform.childCount * 1.5f, 0, 0);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
