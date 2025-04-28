using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private InventoryManager data;
    public ShopUIManager shopUIManager; // Referencia al UI Manager de la tienda



    public List<PassiveItem> PlayerItems;

    public int gems;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        data = InventoryManager.instance;
        PlayerItems = data.GetPlayerItems();
        gems = data.playerGems;
        
    }

    // Comprar mejora de vidas
    public void ComprarMejoraVidas()
    {
        int costo = CalcularCostoMejora(data.mejorasVidasCompradas);
        if (data.playerGems >= costo)
        {
            data.playerGems -= costo;
            data.AumentarVidas(1);
            Debug.Log($"Mejora de vidas comprada. Costo: {costo}. Vidas actuales: {data.lives}");

            // Actualizar la interfaz
            shopUIManager.UpdateUI();
        }
        else
        {
            Debug.Log("No tienes suficientes gemas para comprar esta mejora.");
        }
    }

    // Comprar mejora de límite de cartas
    public void ComprarMejoraLimiteCartas()
    {
        int costo = CalcularCostoMejora(data.mejorasLimiteCompradas);
        if (data.playerGems >= costo)
        {
            data.playerGems -= costo;
            data.AumentarLimiteCartas(1);
            Debug.Log($"Mejora de límite de cartas comprada. Costo: {costo}. Límite actual: {data.limiteCart}");

            // Actualizar la interfaz
            shopUIManager.UpdateUI();
        }
        else
        {
            Debug.Log("No tienes suficientes gemas para comprar esta mejora.");
        }
    }

    // Comprar mejora de multiplicador de recompensas
    public void ComprarMejoraMultiplicadorRecompensas()
    {
        int costo = CalcularCostoMejora(data.mejorasMultiplicadorCompradas);
        if (data.playerGems >= costo)
        {
            data.playerGems -= costo;
            data.AumentarMultiplicador(0.1f); // Incremento de 0.1 por compra
            Debug.Log($"Mejora de multiplicador de recompensas comprada. Costo: {costo}. Multiplicador actual: {data.multiplicadorRecompensas}");

            // Actualizar la interfaz
            shopUIManager.UpdateUI();
        }
        else
        {
            Debug.Log("No tienes suficientes gemas para comprar esta mejora.");
        }
    }

    // Calcular el costo de una mejora en función de cuántas veces se ha comprado
    private int CalcularCostoMejora(int mejorasCompradas)
    {
        return 100 + (mejorasCompradas * 50); // Costo base de 100, aumenta 50 por cada compra
    }
}
