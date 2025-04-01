using UnityEngine;
using TMPro;

public class Carta : MonoBehaviour
{
    public string familia; // Picas, Corazones, etc.
    public int valorBase;
    public int valorModi;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI textoValor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        valorModi = valorBase; // inicialmente igual al valor base
        ActualizarVisuales();
    }

    public void ModificarValor(int cantidad)
    {
        valorModi += cantidad;
        ActualizarVisuales();
    }

    private void ActualizarVisuales()
    {
        textoValor.text = valorModi.ToString();
        if (valorModi > valorBase)
            spriteRenderer.color = Color.green; // Verde si aumento
        else if (valorModi < valorBase)
            spriteRenderer.color = Color.red; // Rojo si disminuyo
        else
            spriteRenderer.color = Color.white; // Normal
    }
}
