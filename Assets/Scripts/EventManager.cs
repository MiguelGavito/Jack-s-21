using UnityEngine;
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
    public static EventManager Instance;
    public TurnState currentTurn;

    public event Action OnPlayerTurn;
    public event Action OnDealerTurn;
    public event Action OnEndRound;

    public float delayBetweenTurns = 1.5f;

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

    public void StartRound()
    {
        currentTurn = TurnState.PlayerTurn;
        OnPlayerTurn?.Invoke();
    }

    public void EndPlayerTurn()
    {
        StartCoroutine(TransitionToDealerTurn());
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
    }

    private IEnumerator TransitionToEndRound()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentTurn = TurnState.EndRound;
        OnEndRound?.Invoke();
    }
}
