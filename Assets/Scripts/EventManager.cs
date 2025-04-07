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
        }

        if (deckManager == null)
        {
            deckManager = FindFirstObjectByType<DeckManager>();
        }
    }

    private IEnumerator Start()
    {
        

        Debug.Log("Empieza el Start de EventManager y espera a que cargue el deck");
        yield return null; //esperar un frame para que el Start() de deckManager se ejecute
        yield return new WaitUntil(() =>
        {
            Debug.Log("Esperando que deckManager esté listo... ¿deckManager null? " + (deckManager == null) + " ¿isInitialized? " + (deckManager?.isInitialized));
            return deckManager != null && deckManager.IsInitialized();
        });
        Debug.Log("DeckManager esta listo, comenzando ronda");
        StartRound();
    }
    #endregion

    #region Round Management
    // Métodos para manejar los turnos y rondas
    public void StartRound()
    {

        Debug.Log("== INICIO DE NUEVA RONDA ==");

        SetCurrentTurn(TurnState.PlayerTurn);



        gameManager.SetupNewRound(); // toma 2 cartas player
        Debug.Log("Empieza StartRound se va aejecutar SetupNewRound y se robaran 2 cartas del jugador y dos para el dealer");
        
        OnPlayerTurn?.Invoke();
        
        
        uiManager.SetButtonsInteractable(true);
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Termina el turno del juegador y se voltean las cartas boca abajo");
        StartCoroutine(TransitionToDealerTurn());
        uiManager.SetButtonsInteractable(false);
    }

    private IEnumerator TransitionToDealerTurn()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        gameManager.FlipDealerCards(); // girar las cartas del dealer
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
        if (currentTurn == TurnState.EndRound || currentTurn == TurnState.PlayerTurn || currentTurn == TurnState.DealerTurn)
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
