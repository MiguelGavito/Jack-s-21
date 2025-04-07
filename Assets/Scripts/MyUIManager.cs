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

    public Text playerHandValueText;
    public Text dealerHandValueText;
    #endregion

    #region UI Interaction Methods
    // Funciones que interactúan con los botones de la UI
    public void SetButtonsInteractable(bool isInteractable)
    {
        hitButton.interactable = isInteractable;
        standButton.interactable = isInteractable;
        dealButton.interactable = isInteractable;
    }

    public void TogglePauseMenu(bool show)
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(show);
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
}