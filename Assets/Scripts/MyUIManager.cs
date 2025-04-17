using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyUIManager : MonoBehaviour
{
    #region Variables
    // Variables públicas para los botones y textos de la interfaz
    public Button hitButton;
    public Button standButton;
    public Button dealButton;

    public GameObject pauseMenu;
    public bool isMenuActive = false;

    public TextMeshProUGUI playerHandValueText;
    public TextMeshProUGUI dealerHandValueText;

    public TextMeshProUGUI ScoreUIText;
    public TextMeshProUGUI ObjScoreUIText;

    public TextMeshProUGUI RecordText;
    public TextMeshProUGUI HandsText;

    public GameManager manager;



    #endregion

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

        HandsText.SetText(manager.lives.ToString());

        ObjScoreUIText.SetText(manager.puntajeObj.ToString());

        RecordText.SetText(manager.record.ToString());

        // Escuchar tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void UpdateUI()
    {
        // Actualiza el puntaje del jugador
        ScoreUIText.SetText(manager.puntaje.ToString());

        // Actualiza las vidas del jugador
        HandsText.SetText(manager.lives.ToString());

        // Actualiza el puntaje objetivo de la ronda
        ObjScoreUIText.SetText(manager.puntajeObj.ToString());

        // Actualiza el record (puntaje más alto)
        if (RecordText != null)
        {
            RecordText.SetText(manager.record.ToString());
        }
    }

    #endregion
}