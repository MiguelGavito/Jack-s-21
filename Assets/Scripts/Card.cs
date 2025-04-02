using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public string id; //luego la usare para que cada carta sea unica (usare numeros simples)

    public bool FlipUp = false; // cuando true, solo se vera el otro lado de la carta


    public string palo; // palo de la carta
    public int numero; // Numero de la carta
    public int numMod; // inutil por ahora
    public Sprite spriteVis; // sprite de la carta
    public TextMeshProUGUI textoValor; // texto mostrando su valor

    public void SetCard(string _palo, int _numero, int _numMod, Sprite _sprite, string _textoValor)
    {
        palo = _palo;
        numero = _numero;
        numMod = _numero;


        // Asignar el sprite correctamente
        if (spriteVis != null && _sprite != null)
        {
            spriteVis = _sprite; // Asigna el sprite
            Debug.Log($" Imagen asignada correctamente: sprites/Deck-of-Cards/{_numero}");
            
        }
        else
        {
            Debug.LogError($" Error asignando sprite: {(_sprite == null ? "Sprite no encontrado" : "Image no asignado en prefab")}");
        }

        // Asignar el texto de la carta
        if (textoValor != null)
        {
            textoValor.text = _textoValor;
        }
        else
        {
            Debug.LogError(" No se encontró el componente TextMeshProUGUI en la carta.");
        }
    }

    //cambiaremos el estado de la carta y se volteara (mas tarde pondremos una annimacion o algo a esto pero no se como se hace eso
    public void FlipOn()
    {
        FlipUp = true;
    }

    
}
