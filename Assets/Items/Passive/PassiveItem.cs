using UnityEngine;

public enum PassiveEffectType
{
    IncreaseMaxCards,
    BonusGems,
    IncreaseMultScore
    //agregar mas aqui en caso de necesitar
}

[CreateAssetMenu(fileName = "Passive Item", menuName = "Scriptable Objects/PassiveItem")]
public abstract class PassiveItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int price;
    public bool isUsed = false;

    public int bonusValue;

    public PassiveEffectType effectType;

    public abstract void Apply(GameManager gameManager);

    public void UseItem(GameManager gameManager)
    {
        switch (effectType)
        {
            case PassiveEffectType.IncreaseMaxCards:
                gameManager.limiteCart += bonusValue;
                break;

            case PassiveEffectType.BonusGems:
                gameManager.playerGems += bonusValue;
                break;

            case PassiveEffectType.IncreaseMultScore:
                gameManager.puntaje *= bonusValue;
                break;

            // otros casos aquí...
            default:
                Debug.LogWarning("No se definió un efecto para este objeto pasivo.");
                break;
        }
    }
    
    public void ResetItem()
    {
        isUsed = false;
    }

    public void ApplyEffect(GameManager gameManager)
    {
        UseItem(gameManager);  // Esto llama a UseItem, que aplica el efecto
    }
}
