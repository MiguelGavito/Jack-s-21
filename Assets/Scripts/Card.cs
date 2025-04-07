using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Propiedades de la carta
    public string id; // ID único de la carta
    public bool faceUp = false; // Estado de la carta (volteada o no)
    public string palo; // Palo de la carta
    public int numero; // Número de la carta
    public string rank; // Rango de la carta (A, 2-10, J, Q, K)
    public Sprite cartImg; // Sprite de la carta
    public Sprite downFace; // Sprite boca abajo

    // Métodos

    // Configurar los valores de la carta
    public void SetCard(string _palo, int _numero, string _id, Sprite _cartImg, string _rank)
    {
        palo = _palo;
        numero = _numero;
        id = _id;
        cartImg = _cartImg;
        rank = _rank;
    }

    // Voltear la carta hacia arriba
    public void TurnUp()
    {
        faceUp = true;
        Image imageComponent = GetComponentInChildren<Image>();
        imageComponent.sprite = cartImg;
        Debug.Log($"Carta {id} volteada boca arriba. Valor: {numero}, Palo: {palo}");
    }

    // Voltear la carta hacia abajo
    public void TurnDown()
    {
        faceUp = false;
        Image imageComponent = GetComponentInChildren<Image>();
        imageComponent.sprite = downFace;
    }

    // Verificar si la carta está boca arriba
    public bool IsFaceUp()
    {
        return faceUp;
    }

    // Verificar si la carta está boca abajo
    public bool IsFaceDown()
    {
        return !faceUp;
    }

    // Obtener el valor de la carta
    public int GetValue()
    {
        return numero;
    }

    // Establecer un nuevo valor a la carta
    public void SetValue(int _value)
    {
        numero = _value;
    }

    // Verificar si la carta es un As
    public bool IsAce()
    {
        return rank == "A";
    }
}
