using UnityEngine;

public enum PassiveEffectType
{
    ExtraCardLimit,
    BonusGems,
    BonusScore
}

public class PassiveItemHolder : MonoBehaviour
{
    [Header("UI References")]
    public string itemName;
    public string itemDescription;
    public int bonusValue;
    public PassiveEffectType effectType;

    public PassiveItem passiveItemData;
    public GameManager gameManager;
    // Método para inicializar y configurar los datos de PassiveItem
    public void SetItem(PassiveItem newItem)
    {
        passiveItemData = newItem;
        itemName = passiveItemData.itemName;
        itemDescription = passiveItemData.itemDescription;
        bonusValue = passiveItemData.bonusValue;
// effectType = passiveItemData.effectObjects;
    }

    // Método para aplicar el efecto en el GameManager
    public void ApplyPassive(GameManager gameManager)
    {
        if (passiveItemData != null)
        {
            switch (effectType)
            {
                case PassiveEffectType.ExtraCardLimit:
                    gameManager.limiteCart += bonusValue;
                    break;
                case PassiveEffectType.BonusGems:
                    InventoryManager.instance.AgregarGemas(bonusValue);
                    break;
                case PassiveEffectType.BonusScore:
                    gameManager.bonus += bonusValue;
                    break;
                default:
                    Debug.LogWarning("No effect type assigned!");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Passive Item data not assigned!");
        }
    }
    public void ActivateItem()
    {
        // Aquí activamos los efectos de acuerdo a los datos del item
        ApplyPassive(gameManager);
    }
}
