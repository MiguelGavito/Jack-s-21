using UnityEngine;

[CreateAssetMenu(fileName = "GemBonus", menuName = "Scriptable Objects/GemBonus")]
public class GemBonus : ScriptableObject, IPassiveEffect
{
    public int gemAmount = 5;

    public void Apply(GameManager gameManager)
    {
        gameManager.playerGems += gemAmount; // cambiar el gamemanager para tener una variable de recompensa
        Debug.Log($"[Passive] Ganas {gemAmount} gemas adicionales.");
    }
}
