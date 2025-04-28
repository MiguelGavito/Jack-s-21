using TMPro;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [Header("Text Values")]
    public TextMeshProUGUI lives;
    public TextMeshProUGUI limitCards;
    public TextMeshProUGUI gems;
    public TextMeshProUGUI multiplier;

    [Header("Cost Texts")]
    public TextMeshProUGUI livesCost;
    public TextMeshProUGUI limitCardsCost;
    public TextMeshProUGUI multiplierCost;

    [Header("Shop Manager")]
    public ShopManager shopManager;

    private InventoryManager data;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = InventoryManager.instance;

        UpdateUI();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        lives.SetText(data.lives.ToString());
        limitCards.SetText(data.limiteCart.ToString());
        gems.SetText(data.playerGems.ToString());
        multiplier.SetText(data.multiplicadorRecompensas.ToString());
        
        livesCost.SetText((data.mejorasVidasCompradas*50+100).ToString());
        limitCardsCost.SetText((data.mejorasLimiteCompradas * 50 + 100).ToString());
        multiplierCost.SetText((data.mejorasMultiplicadorCompradas * 50 + 100).ToString());
    }
}
