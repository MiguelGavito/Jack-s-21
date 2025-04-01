using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public string palo; // palo de la carta
    public int numero; // Numero de la carta
    public int numMod; // inutil por ahora
    public Image spriteVis; // sprite de la carta
    public TextMeshProUGUI textoValor; // texto mostrando su valor

    public void SetCard(string _palo, int _numero, int _numMod, Image _imageVis, TextMeshProUGUI _textoValor) //
    {
        palo = _palo;
        numero = _numero;
        numMod = _numero;
        // Asignar el sprite al SpriteRenderer correcto
        if (_imageVis != null)
        {
            spriteVis = _imageVis; // Guarda la referencia
            spriteVis.sprite = Resources.Load<Sprite>($"sprites/Deck-of-Cards/{_numero}");

            if (spriteVis.sprite == null)
                Debug.LogError($" No se encontró la imagen: sprites/Deck-of-Cards/{_numero}");
            else
                Debug.Log($" Imagen asignada correctamente: sprites/Deck-of-Cards/{_numero}");
        }

        if (_textoValor != null)
        {
            textoValor = _textoValor;
            textoValor.text = _palo;
        }
    }
}
