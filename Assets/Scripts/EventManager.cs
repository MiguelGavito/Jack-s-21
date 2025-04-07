using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum TurnState
{
    PlayerTurn,
    DealerTurn,
    EndRound
}

public class EventManager : MonoBehaviour
{
    #region Variables
    // Variables p�blicas del EventManager
    public static EventManager Instance;
    public TurnState currentTurn;

    public event Action OnPlayerTurn;
    public event Action OnDealerTurn;
    public event Action OnEndRound;

    public float delayBetweenTurns = 1.5f;

    public MyUIManager uiManager;
    #endregion

    #region Initialization
    // M�todos de inicializaci�n
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

    #region Round Management
    // M�todos para manejar los turnos y rondas
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
    }

    private IEnumerator TransitionToDealerTurn()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentTurn = TurnState.DealerTurn;
        OnDealerTurn?.Invoke();
    }

    public void EndDealerTurn()
    {
        StartCoroutine(TransitionToEndRound());
        GameManager.instance.FlipDealerCards();
    }

    private IEnumerator TransitionToEndRound()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentTurn = TurnState.EndRound;
        OnEndRound?.Invoke();
    }
    #endregion
}
