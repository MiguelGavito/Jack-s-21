using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.XR;
using Unity.VisualScripting;

public class DeckManager : MonoBehaviour
{
    #region Variables
    // Variables públicas para las cartas y transformaciones de los jugadores y mazo
    public GameObject cardPrefab;
    public Transform deckTransform, discardTransform;
    public Transform player1Transform, player2Transform;
    public List<Sprite> cardSprites = new List<Sprite>();
    public Sprite downFace;

    private Stack<GameObject> deck = new Stack<GameObject>();
    public bool isInitialized = false;
    #endregion

    #region Initialization
    // Métodos relacionados con la inicialización y configuración
    void Start()
    {
        if (cardSprites.Count != 52)
        {
            Debug.LogError("¡La lista de sprites no tiene exactamente 52 cartas!");
            return;
        }
        Debug.Log("Generar deck y revolver deck");
        GenerateDeck();
        ShuffleDeck();
        isInitialized = true;
    }
    

    public bool IsInitialized()
    {
        return isInitialized;
    }

    public bool IsEmpty()
    {
        return cardSprites.Count == 0;
    }
    #endregion

    #region Deck Management
    // Métodos relacionados con la creación, generación y barajado del mazo
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

                // Obtener sprite directamente de la lista
                Sprite sprite = cardSprites[index];

                // Definir valor de número y nombre
                string rank = "";
                int numa = 0;

                if (j <= 8)
                {
                    rank = (j + 2).ToString(); // 2 al 10
                    numa = j + 2;
                }
                else if (j == 9)
                {
                    rank = "J";
                    numa = 10;
                }
                else if (j == 10)
                {
                    rank = "Q";
                    numa = 10;
                }
                else if (j == 11)
                {
                    rank = "K";
                    numa = 10;
                }
                else if (j == 12)
                {
                    rank = "A";
                    numa = 11;
                }

                // Configurar la carta
                Card cardComponent = newCard.GetComponent<Card>();
                if (cardComponent != null)
                {
                    cardComponent.SetCard(palos[i], numa, palos[i] + (j + 2).ToString(), sprite, rank);
                }

                cardComponent.TurnDown();

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
    #endregion

    #region Card Drawing
    
    //Funcion para mover cartas entre transforms
    public Card MoveCard(Transform StartTransform, Transform EndTransform, int index)
    {
        if (StartTransform.childCount > index)
        {
            Transform cardTransform = StartTransform.GetChild(index);

            cardTransform.SetParent(EndTransform);

            cardTransform.localPosition = new Vector3(EndTransform.childCount * 1.5f, 0, 0);

            // Obtener el componente Card de la carta movida
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                // Aquí puedes agregar alguna acción adicional si la carta se ha movido correctamente
                return card;
            }
        }
        return null;
    }
    
    // Métodos relacionados con el robo de cartas
    public Card DrawCard(Transform playerTransform)
    {
        if (deck.Count > 0)
        {
            GameObject drawnCardObj = deck.Pop(); // Sacar carta del mazo
            drawnCardObj.transform.SetParent(playerTransform); // Asignarla al jugador
            drawnCardObj.transform.localPosition = new Vector3(playerTransform.childCount * 1.5f, 0, 0);

            Card drawnCard = drawnCardObj.GetComponent<Card>(); // Obtener el script de la carta
            if (drawnCard != null)
            {
                drawnCard.TurnDown(); // Asegurar que inicia boca abajo
                return drawnCard; // Devolver la carta para que `GameManager` la pueda modificar
            }
        }
        else
        {
//Revisar luego
            GenerateDeck();
            ShuffleDeck();
            DrawCard(playerTransform);
        }
        return null; // Si el mazo está vacío, devolver `null`
    }
    #endregion

    #region Cards manage

    public Card TakeCardFromHand(Transform handTransform, Transform targetTransform, bool fromEnd = false)
    {
        if (handTransform.childCount > 0)
        {
            Transform cardTransform = fromEnd
                ? handTransform.GetChild(handTransform.childCount - 1) // Última carta
                : handTransform.GetChild(0); // Primera carta

            cardTransform.SetParent(targetTransform);
            cardTransform.localPosition = new Vector3(targetTransform.childCount * 1.5f, 0, 0);

            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                card.TurnDown(); // o card.TurnUp() según el contexto
                return card;
            }
        }
        return null;
    }


    #endregion

    #region Hand Value Calculations
    // Métodos relacionados con el cálculo del valor de la mano
    public int CalculateRawHandValue(Transform hand)
    {
        int total = 0;

        foreach (Transform cardTransform in hand)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                total += card.GetValue();
            }
        }

        return total;
    }

    public int CalculateHandValue(Transform hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (Transform cardTransform in hand)
        {
            Card card = cardTransform.GetComponent<Card>();
            if (card != null)
            {
                int value = card.GetValue();

                if (card.IsAce())
                {
                    aceCount++;
                    value = 11;
                }

                total += value;
            }
        }

        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }

    // Contador de ases en la mano
    public int CountAces(Transform hand)
    {
        int count = 0;
        foreach (Transform cardTransform in hand)
        {
            // Obtener la carta desde el objeto Transform
            Card card = cardTransform.GetComponent<Card>();

            if (card != null && card.IsAce())  // Verificar si es un As
            {
                count++;
            }
        }
        return count;
    }

    public void AdjustAceValue(Transform hand)
    {
        foreach (Transform cardTransform in hand)
        {
            Card card = cardTransform.GetComponent<Card>();

            if (card != null && card.IsAce() && card.GetValue() == 11)
            {
                card.SetValue(1); // Aquí asegúrate de que SetValue existe en tu clase Card
                break; // Solo ajustamos un As por llamada
            }
        }
    }
    #endregion

    #region Hand Management
    // Métodos relacionados con la gestión de la mano
    public void ClearHand(Transform playerHand)
    {
        foreach (Transform cardTransform in playerHand)
        {
            Debug.Log($"Destruyendo carta: {cardTransform.name} de {playerHand.name}");
            Destroy(cardTransform.gameObject);
        }
    }

    public int CalculateHandValue(Transform hand, bool hideHoleCard = false)
    {
        int value = 0;
        int aceCount = 0;

        for (int i = 0; i < hand.childCount; i++)
        {
            Transform cardTransform = hand.GetChild(i);
            Card card = cardTransform.GetComponent<Card>();

            // Omitir la segunda carta si se debe ocultar (carta boca abajo)
            if (hideHoleCard && i == 1)
                continue;

            value += card.numero;

            if (card.IsAce())
                aceCount++;
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10; // convertir As de 11 a 1
            aceCount--;
        }

        return value;
    }
    #endregion
}
