using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyUIManager : MonoBehaviour
{
    public Button hitButton;
    public Button standButton;
    public Button dealButton;
    public GameObject pauseMenu;

    public Text playerHandValueText;
    public Text dealerHandValueText;

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
}
