using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Data;

public enum TurnState
{
    PlayerTurn,
    DealerTurn,
    EndRound
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }


    public TurnState currentTurn;

    public event Action OnPlayerTurn;
    public event Action OnDealerTurn;
    public event Action OnEndRound;

    public float delayBetweenTurns = 1.5f;
    public MyUIManager uiManager;
    public GameManager gameManager;

    #region UnityMethods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartRound();
    }

    #endregion

    #region Game Flow

    public void StartRound()
    {
        currentTurn = TurnState.PlayerTurn;
        OnPlayerTurn?.Invoke();
        uiManager.SetButtonsInteractable(true);
    }

    public void EndPlayerTurn()
    {
        StartCoroutine(TransitionToDealerTurn());
        uiManager.SetButtonsInteractable(false);
        gameManager.FlipDealerCards();
    }

    public void EndDealerTurn()
    {
        StartCoroutine(TransitionToEndRound());
    }

    #endregion

    #region Turn Transitions
    private IEnumerator TransitionToDealerTurn()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentTurn = TurnState.DealerTurn;
        OnDealerTurn?.Invoke();
    }

    private IEnumerator TransitionToEndRound()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentTurn = TurnState.EndRound;
        OnEndRound?.Invoke();

        EvaluateResults();
    }

    #endregion

    #region Round Evaluation

    private void EvaluateResults()
    {
        gameManager.EvaluateResults();
    }
    #endregion

    #region Accesors

    public TurnState GetCurrentTurn()
    {
        return currentTurn;
    }
    #endregion

}
