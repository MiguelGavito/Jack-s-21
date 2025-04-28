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

    [Header("Puntaciones")]
    public TextMeshProUGUI playerHandValueText;
    public TextMeshProUGUI dealerHandValueText;

    public TextMeshProUGUI ScoreUIText;
    public TextMeshProUGUI ObjScoreUIText;

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

        inventoryManager = data;
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
    
        // Escuchar tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
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
    }

    public void MensajeGanarRonda(int gemas, int puntos)
    {
        string mensaje = $"¡You Won the round! Obtuviste {puntos} puntos y {gemas} gemas.";
        roundMessenger.Announce(mensaje);
    }

    public void MensajePerderRonda()
    {
        string mensaje = $"Perdiste la ronda. Pierdes una vida. Pierdes la mitad de tu apuesta.";
        roundMessenger.Announce(mensaje);
    }

    public void MensajePerderJuego()
    {
        string mensaje = "¡Game Over! Mejor suerte la próxima.";
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