using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform deckTransform, discardTransform;
    public Transform player1Transform, player2Transform;
    public List<Sprite> cardSprites = new List<Sprite>();

    private Stack<GameObject> deck = new Stack<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cardSprites.Count != 52)
        {
            Debug.LogError("¡La lista de sprites no tiene exactamente 52 cartas!");
            return;
        }

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
                // Creo mi nueva carta con el prefab y dentro de deck
                GameObject newCard = Instantiate(cardPrefab, deckTransform);
                int index = i * 13 + j; // indice de la carta en mi lista

                // Obterner sprite directamente de la lista
                Sprite sprite = cardSprites[index];

                //definir valor de numero
                int numa = j >= 8 ? 10 : j + 2;


                // Configurar la carta
                Card cardComponent = newCard.GetComponent<Card>();
                if (cardComponent != null)
                {
                    cardComponent.SetCard(palos[i], numa, palos[i] + (j + 2).ToString(), sprite);
                }

                //Asignar imagen boca abajo prefeffinida
                cardComponent.TurnDown();

                //Necesito ponerle a la carta un componente de esto
                // Asignar el texto del palo
                TextMeshProUGUI titulo = newCard.GetComponentInChildren<TextMeshProUGUI>();
                if (titulo != null)
                {
                    titulo.text = palos[i];
                }

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

            Card cardScript = drawnCard.GetComponent<Card>();
            if (cardScript != null)
            {
                cardScript.faceUp = playerTransform.GetComponent<Card>().IsFaceUp();
            }
            
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
