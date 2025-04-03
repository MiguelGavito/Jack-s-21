using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public string id; // ID único de la carta
    public bool faceUp = false; // Estado de la carta (volteada o no)
    public string palo; // Palo de la carta
    public int numero; // Número de la carta
    public Sprite cartImg; // Sprite de la carta
    public Sprite downFace; // SPrite boca abajo

    //public TextMeshProUGUI textoValor; // texto mostrando su valor



    public void SetCard(string _palo, int _numero, string _id, Sprite _cartImg) //, string _textoValor
    {
        palo = _palo;
        numero = _numero;
        id = _id;
        cartImg = _cartImg;

        /*
        Image imageComponent = GetComponentInChildren<Image>(); // obtengo el componente imagen del prefab card
        if (imageComponent != null)
        {
            imageComponent.sprite = cartImg;
        }
        else
        {
            Debug.LogError("No se encontro el componente Image en la carta.");
        }
        */


        /*
         * 
         *  LUEGO LO VUELVO A ACTIVAR MAS ADELANTE
         * 
        // Asignar el texto de la carta
        if (textoValor != null)
        {
            textoValor.text = _textoValor;
        }
        else
        {
            Debug.LogError(" No se encontró el componente TextMeshProUGUI en la carta.");
        }
        */

    }


    // ponemos carta boca arriba
    public void TurnUp()
    {
        faceUp = true;
        Image imageComponent = GetComponentInChildren<Image>();
        imageComponent.sprite = cartImg;
        Debug.Log($"Carta {id} volteada boca arriba. Valor: {numero}, Palo: {palo}");
    }

    // Ponemos la carta boca abajo
    public void TurnDown()
    {
        faceUp = false;
        Image imageComponent = GetComponentInChildren<Image>();
        imageComponent.sprite = downFace;
    }

    // Esta boca arriba?
    public bool IsFaceUp()
    {
        return faceUp;
    }

    // Esta boca abajo?
    public bool IsFaceDown()
    {
        return !faceUp;
    }

}
