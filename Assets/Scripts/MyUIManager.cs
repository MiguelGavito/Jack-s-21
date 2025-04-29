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
    public bool isRulesActive = true;


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
        InventoryManager data = InventoryManager.instance;

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager no está inicializado en MyUIManager.");
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

    void Update()
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
        MultText.SetText(inventoryManager.multiplicadorRecompensas.ToString("F2")); // Formato con 2 decimales

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
    #endregion
}