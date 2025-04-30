using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum TurnState
{   
    //RepartitionTurn, // turno donde reparte cartas
    PlayerTurn,
    //FlipUpTurn, // turno donde se voltean las cartas
    DealerTurn,
    //ObjectionTurn, //turno de accion de objeto para el jugador
    EndRound
}

public class EventManager : MonoBehaviour
{
    #region Variables
    // Variables públicas del EventManager
    public static EventManager Instance;
    public TurnState currentTurn;

    public event Action OnPlayerTurn;
    public event Action OnDealerTurn;
    public event Action OnEndRound;

    public float delayBetweenTurns = 1.5f;

    public MyUIManager uiManager;
    public GameManager gameManager;
    public DeckManager deckManager;
    #endregion

    #region Initialization
    // Métodos de inicialización
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (deckManager == null)
        {
            deckManager = FindFirstObjectByType<DeckManager>();
        }
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        if (uiManager == null)
            uiManager = FindFirstObjectByType<MyUIManager>();
    }

    private IEnumerator Start()
    {


        if (deckManager == null)
        {
            deckManager = FindFirstObjectByType<DeckManager>();
            if (deckManager == null)
            {
                Debug.LogError("DeckManager no encontrado.");
                yield break;
            }
        }

        Debug.Log("Esperando que deckManager esté listo...");
        yield return new WaitUntil(() => deckManager.IsInitialized());

        Debug.Log("DeckManager listo, iniciando ronda.");
        StartRound();
    }
    #endregion

    #region Round Management
    // Métodos para manejar los turnos y rondas
    public void StartRound()
    {
        Debug.Log("== INICIO DE NUEVA RONDA ==");
        StartCoroutine(StartRoundCoroutine());
    }

    private IEnumerator StartRoundCoroutine()
    {
        SetCurrentTurn(TurnState.PlayerTurn);

        gameManager.LimpiarManos();

        // Esperar a que StartNewRound termine
        yield return StartCoroutine(gameManager.StartNewRound());

        Debug.Log("StartNewRound ha terminado, ahora comienza el turno del jugador");

        // Invocar el evento del turno del jugador
        OnPlayerTurn?.Invoke();

        // Actualizar puntajes y UI
        gameManager.UpdateScores();
        uiManager.UpdateUI();
        uiManager.SetButtonsInteractable(true);
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Termina el turno del juegador y se voltean las cartas boca abajo");

        if (gameManager == null)
        {
            Debug.LogError("gameManager está NULL antes de iniciar Coroutine");
        }

        StartCoroutine(TransitionToDealerTurn());
        uiManager.SetButtonsInteractable(false);
    }

    private IEnumerator TransitionToDealerTurn()
    {
        yield return new WaitForSeconds(delayBetweenTurns);

        if (gameManager == null)
        {
            Debug.LogError("gameManager está NULL en TransitionToDealerTurn");
            yield break;
        }

        gameManager.FlipDealerCards(); // girar las cartas del dealer
        gameManager.UpdateScores(); // actualizamos los scores

        currentTurn = TurnState.DealerTurn;
        OnDealerTurn?.Invoke();
    }

    public void EndDealerTurn()
    {
        StartCoroutine(TransitionToEndRound());
        gameManager.FlipDealerCards();
    }

    private IEnumerator TransitionToEndRound()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentTurn = TurnState.EndRound;
        OnEndRound?.Invoke();

        yield return new WaitForSeconds(1f);

        StopAllCoroutines();

        // Agregar una bandera para evitar iniciar otra ronda si ya está en curso
        if (currentTurn != TurnState.EndRound)
        {
            
            StartRound();  // Solo reiniciar la ronda si estamos en el estado adecuado
        }
    }

    private IEnumerator StartNextRoundSafely()
    {
        // Verifica si no estamos ya en una ronda activa
        if (currentTurn == TurnState.PlayerTurn || currentTurn == TurnState.DealerTurn || currentTurn == TurnState.EndRound)
        {
            yield break;  // Si ya hay una ronda activa, no hacemos nada
        }

        yield return null;
        StartRound();  // Solo inicia la nueva ronda si está en el estado correcto
    }

    private void SetCurrentTurn(TurnState turn)
    {
        currentTurn = turn;
        Debug.Log(">> Turno actual " + currentTurn);
    }
    #endregion
}
