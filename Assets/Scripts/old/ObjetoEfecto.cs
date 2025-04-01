using UnityEngine;

public class ObjetoEfecto : MonoBehaviour
{
    public int cambioValor; // +2, -1, etc.

    public void AplicarEfecto(Carta carta)
    {
        carta.ModificarValor(cambioValor);
    }
}
