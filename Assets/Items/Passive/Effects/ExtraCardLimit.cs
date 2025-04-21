using UnityEngine;

[CreateAssetMenu(fileName = "ExtraCardLimit", menuName = "Scriptable Objects/ExtraCardLimit")]
public class ExtraCardLimit : ScriptableObject, IPassiveEffect
{
    public int extraLimit = 2;

    public void Apply(GameManager gameManager)
    {
        gameManager.limiteCart += extraLimit;
        Debug.Log($"[Passive] Liite de puntos de cartas aumentado en {extraLimit}.");
    }
}
