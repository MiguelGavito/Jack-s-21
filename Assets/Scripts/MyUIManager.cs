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
    public Text resultText;

    public Transform playerCardsContainer;
    public Transform dealerCardContainer;

    public void UpdateHandValues()
    {
        int playerValue = GameManager.instance.deckManager.CalculateHandValue(GameManager.instance.playerHand);
        int dealerValue = GameManager.instance.deckManager.CalculateHandValue(GameManager.instance.dealerHand, hideHoleCard: true);

        playerHandValueText.text = "Jugador: " + playerValue.ToString();

        if (GameManager.instance.eventManager.currentTurn == TurnState.DealerTurn || GameManager.instance.eventManager.currentTurn == TurnState.EndRound)
        {
            dealerHandValueText.text = "Dealer: " + dealerValue.ToString();
        }
        else
        {
            dealerHandValueText.text = "Dealer: ?"; // La carta del dealer está oculta
        }
    }

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

    // Actualiza la UI para cuando es el turno del jugador
    public void UpdateUIForPlayerTurn()
    {
        resultText.text = ""; // Limpia los resultados
        UpdateHandValues();
        dealerHandValueText.text = "Dealer: ?"; // El dealer aún tiene una carta oculta
        SetButtonsInteractable(true); // Habilita los botones de acción del jugador
    }

    // Actualiza la UI para cuando es el turno del dealer
    public void UpdateUIForDealerTurn()
    {
        resultText.text = ""; // Limpia los resultados
        dealerHandValueText.text = "Dealer: ?"; // Inicialmente ocultamos la carta del dealer
        SetButtonsInteractable(false); // Desactiva los botones del jugador mientras el dealer juega
    }

    // Muestra los resultados al final de la ronda
    public void DisplayEndRoundResults(string result)
    {
        resultText.text = result; // Muestra el mensaje de resultado (victoria, derrota, empate, etc.)
    }

    // Agrega una carta a la mano del jugador y la muestra en la UI
    public void AddCardToPlayerHand(Card card)
    {
        GameObject cardObj = Instantiate(card.cardPrefab, playerCardsContainer);
        cardObj.GetComponent<CardVisual>().SetCard(card); // Asegúrate de tener un Script CardVisual para manejar la apariencia
    }

    // Agrega una carta a la mano del dealer y la muestra en la UI
    public void AddCardToDealerHand(Card card)
    {
        GameObject cardObj = Instantiate(card., dealerCardsContainer);
        cardObj.GetComponent<CardVisual>().SetCard(card); // Igual que para el jugador, gestionamos la apariencia
    }

    // Limpia las manos de cartas al comenzar una nueva ronda
    public void ClearHands()
    {
        foreach (Transform card in playerCardsContainer)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform card in dealerCardContainer)
        {
            Destroy(card.gameObject);
        }
    }
}
