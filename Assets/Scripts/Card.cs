using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public string id; // ID único de la carta
    public bool FlipUp = false; // Estado de la carta (volteada o no)
    public string palo; // Palo de la carta
    public int numero; // Número de la carta
    public Sprite cartImg; // Sprite de la carta

    //public TextMeshProUGUI textoValor; // texto mostrando su valor

    public void SetCard(string _palo, int _numero, string _id, Sprite _cartImg) //, string _textoValor
    {
        palo = _palo;
        numero = _numero;
        id = _id;
        cartImg = _cartImg;

        Image imageComponent = GetComponentInChildren<Image>(); // obtengo el componente imagen del prefab card
        if (imageComponent != null)
        {
            imageComponent.sprite = cartImg;
        }
        else
        {
            Debug.LogError("No se encontro el componente Image en la carta.");
        }


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

    //cambiaremos el estado de la carta y se volteara (mas tarde pondremos una annimacion o algo a esto pero no se como se hace eso
    public void FlipOn()
    {
        FlipUp = FlipUp == true ? false : true;
    }

    
}
