using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyUIManager : MonoBehaviour
{
    #region Variables
    // Variables públicas para los botones y textos de la interfaz
    [Header("Botones")]
    public Button hitButton;
    public Button standButton;
    public Button dealButton;

    [Header("Cuadro Menu")]
    public GameObject pauseMenu;
    public bool isMenuActive = false;

    [Header("Cuadre de Reglas")]
    public GameObject rulesPanel;
    public bool isRulesActive = false;


    [Header("Puntaciones")]
    public TextMeshProUGUI playerHandValueText;
    public TextMeshProUGUI dealerHandValueText;

    public TextMeshProUGUI ScoreUIText;
    public TextMeshProUGUI ObjScoreUIText;

    [Header("Modi de Estadisticas")]
    public TextMeshProUGUI cantExLimText;
    public TextMeshProUGUI MultText;

    [Header("UI References")]
    public TextMeshProUGUI RecordText;
    public TextMeshProUGUI HandsText;
    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI LimitCart;
    public TextMeshProUGUI GemsText;
    public TextMeshProUGUI BetText;

    [Header("Mensajero")]
    public TextMeshProUGUI Informante;
    public bool isInfoDisplay = false;

    [Header("Referencias externas")]
    public GameManager manager;
    public InventoryManager inventoryManager;
    public TextManager roundMessenger;



    #endregion

    private void Start()
    {

        ReconnectInventoryManager();

        // Asegurarse de que InventoryManager esté referenciado
        if (inventoryManager == null)
        {
            inventoryManager = InventoryManager.instance;
            if (inventoryManager == null)
            {
                Debug.LogError("No se encontró una instancia de InventoryManager.");
            }
        }

        // Actualizar la UI si las referencias están configuradas
        if (manager != null && inventoryManager != null)
        {
            UpdateUI();
        }
    }

    #region UI Interaction Methods
        // Funciones que interactúan con los botones de la UI
    public void SetButtonsInteractable(bool isInteractable)
    {
        hitButton.interactable = isInteractable;
        standButton.interactable = isInteractable;
        dealButton.interactable = isInteractable;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }


    }

    public void TogglePauseMenu()
    {
        isMenuActive = !isMenuActive;

        pauseMenu.SetActive(isMenuActive);

        if (isMenuActive)
        {
            Time.timeScale = 0f; //Pause the game
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void CloseMenu()
    {
        isMenuActive = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    
    public void OpenMenu()
    {
        isMenuActive = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ToggleRulesPanel()
    {
        isRulesActive = !isRulesActive;

        rulesPanel.SetActive(isRulesActive);

        if (isRulesActive)
        {
            Time.timeScale = 0f; //Pause the game
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void CloseTuto()
    {
        isRulesActive = false;
        rulesPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    public void OpenTuto()
    {
        isRulesActive = true;
        rulesPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }


    #endregion

    #region Hand Value Updates
    // Funciones relacionadas con la actualización de los valores de las manos
    public void UpdateHandValues()
    {
        int playerValue = GameManager.instance.deckManager.CalculateHandValue(GameManager.instance.player1Transform);
        int dealerValue = GameManager.instance.deckManager.CalculateHandValue(GameManager.instance.player2Transform, hideHoleCard: true);

        playerHandValueText.text = "Jugador: " + playerValue.ToString();

        if (GameManager.instance.eventManager.currentTurn == TurnState.DealerTurn || GameManager.instance.eventManager.currentTurn == TurnState.EndRound)
        {
            dealerHandValueText.text = "Dealer: " + dealerValue.ToString();
        }
        else
        {
            dealerHandValueText.text = "Dealer: ?";
        }
    }
    #endregion

    #region Data UI Updates

    private void Update()
    {
        ReconnectInventoryManager();


        // Actualizar la UI solo si las referencias son válidas
        if (manager != null && inventoryManager != null)
        {
            ScoreUIText.SetText(manager.puntaje.ToString());
            HandsText.SetText(inventoryManager.lives.ToString());
            ObjScoreUIText.SetText(manager.puntajeObj.ToString());
            RecordText.SetText(manager.record.ToString());
            RoundText.SetText(inventoryManager.round.ToString());
            LimitCart.SetText(inventoryManager.limiteCart.ToString());
            GemsText.SetText(inventoryManager.playerGems.ToString());
            BetText.SetText(manager.playerBet.ToString());

            // Actualizar las estadísticas específicas de mejoras
            cantExLimText.SetText(inventoryManager.mejorasLimiteCompradas.ToString());
            MultText.SetText(inventoryManager.multiplicadorRecompensas.ToString("F2"));
        }

        // Escuchar tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        // Escuchar tecla de Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleRulesPanel();
        }
    }

    public void UpdateUI()
    {
        // Reconectar InventoryManager si es necesario
        ReconnectInventoryManager();

        if (manager == null || inventoryManager == null)
        {
            Debug.LogError("MyUIManager: Referencias a GameManager o InventoryManager no están configuradas.");
            return;
        }
        ScoreUIText.SetText(manager.puntaje.ToString());
        HandsText.SetText(inventoryManager.lives.ToString());
        ObjScoreUIText.SetText(manager.puntajeObj.ToString());
        RecordText.SetText(manager.record.ToString());
        RoundText.SetText(inventoryManager.round.ToString());
        LimitCart.SetText(inventoryManager.limiteCart.ToString());
        GemsText.SetText(inventoryManager.playerGems.ToString());
        BetText.SetText(manager.playerBet.ToString());

        // Actualizar las estadísticas específicas de mejoras
        cantExLimText.SetText(inventoryManager.mejorasLimiteCompradas.ToString());
        MultText.SetText(inventoryManager.multiplicadorRecompensas.ToString("F2")); // Formato con 2 decimales
    }

    public void MensajeGanarRonda(int gemas, int puntos)
    {
        string mensaje = $"You Won the round! You earned {puntos} points and {gemas} gems.";
        roundMessenger.Announce(mensaje);
    }

    public void MensajePerderRonda()
    {
        string mensaje = $"You lost the round. You lose one life. You lose half of your bet.";
        roundMessenger.Announce(mensaje);
    }

    public void MensajePerderJuego()
    {
        string mensaje = "Game Over! Better luck next time.";
        roundMessenger.Announce(mensaje);
    }

    public void MensajeEmpate(int gemas, int puntos)
    {
        string mensaje = $"DRAW, you get {gemas} gems and {puntos} points.";
        roundMessenger.Announce(mensaje);
    }

    public void LimpiarMensaje()
    {
        string mensaje = "";
        roundMessenger.Announce(mensaje);
    }
    private void ReconnectInventoryManager()
    {
        if (inventoryManager == null)
        {
            inventoryManager = InventoryManager.instance;
            if (inventoryManager == null)
            {
                Debug.LogError("MyUIManager: No se pudo reconectar con InventoryManager.");
            }
            else
            {
                Debug.Log("MyUIManager: Reconectado con InventoryManager.");
            }
        }
    }
    #endregion
}