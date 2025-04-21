using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Variables
    [Header("UI Elements")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI bonusText;
    public Image backgroundImage;

    private PassiveItem itemData;
    #endregion

    public void SetItem(PassiveItem newItem)
    {
        itemData = newItem;
        UpdateTooltipInfo();
    }

    private void Start()
    {
        tooltipPanel.SetActive(false);
    }

    private void UpdateTooltipInfo()
    {
        if (itemData == null) return;

        nameText.text = itemData.itemName;
        descriptionText.text = itemData.itemDescription;
        bonusText.text = $"+{itemData.bonusValue}";
        backgroundImage.color = new Color(1f, 0.5f, 0f); // naranja
    }

    public  void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false );
    }

}
