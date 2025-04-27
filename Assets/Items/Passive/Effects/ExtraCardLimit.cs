using UnityEngine;

[CreateAssetMenu(fileName = "ExtraCardLimit", menuName = "Scriptable Objects/ExtraCardLimit")]
public class ExtraCardLimit : ScriptableObject, IPassiveEffect
{
    public int extraLimit = 2;

    public void Apply(GameManager gameManager)
    {
        gameManager.limiteCart += extraLimit;
        Debug.Log($"[Passive] Límite de cartas aumentado en {extraLimit}.");
    }

    public string GetEffectName()
    {
        return "Extra Card Limit";
    }

    public string GetEffectDescription()
    {
        return "Aumenta el límite de cartas.";
    }

    public int GetBonusValue()
    {
        return extraLimit;
    }
}
