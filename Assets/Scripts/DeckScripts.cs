using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DeckScripts : MonoBehaviour
{
    public Sprite[] cardSprites;
    public GameObject cartaPrefab;
    private List<Carta> mazo;

    
    public PlayerHandler playerHandler;
    public DealerHandler dealerHandler;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initDeck();
        shufle();
    }

    private void initDeck()
    {
        mazo = new List<Carta>();

        string[] familias = { "Trebol", "Diamante", "Pica", "Corazon" };
        int[] valores = { 2, 3, 4, 5, 6 , 7, 8, 9, 10, 11, 12 , 13, 14};

        foreach (string familia in familias)
        {
            foreach (int valor in valores)
            {
                GameObject cartaObj = Instantiate(cartaPrefab);
                Carta carta = cartaObj.GetComponent<Carta>();
                carta.familia = familia;
                carta.valorBase = valor;
                carta.valorModi = valor;
                carta.spriteRenderer.sprite = cardSprites[(int)valor-2];
                mazo.Add(carta);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void shufle()
    {
        for (int i = 0; i < mazo.Count; i++)
        { 
            Carta temp = mazo[i];
            int randomIndex = Random.Range(i, mazo.Count);
            mazo[i] = mazo[randomIndex];
            mazo[randomIndex] = temp;
        }
    }

    public void hitCard(bool esJugador)
    {
        
        if (mazo.Count == 0) return;

        // Seleccionar una carta aleatoria del mazo
        int randomIndex = Random.Range(0, mazo.Count);
        Carta carta = mazo[randomIndex];

        // Crea un unevo gameObject para la carta seleccionada
        GameObject cartaObj = Instantiate (cartaPrefab);
        cartaObj.transform.SetParent(transform);
        cartaObj.transform.position = new Vector3(0,0,0);

        //Conffigurar la carta, hay que modificar los valores asignados
        Carta nuevaCarta = cartaObj.GetComponent<Carta>();
        nuevaCarta.familia = carta.familia;
        nuevaCarta.valorBase = carta.valorBase;
        nuevaCarta.valorModi = carta.valorModi;
        nuevaCarta.spriteRenderer.sprite = carta.spriteRenderer.sprite;

        mazo.RemoveAt(randomIndex);

        if (esJugador)
        {
            playerHandler.RecibirCarta(nuevaCarta);
        }
        else
        {
            dealerHandler.RecibirCarta(nuevaCarta);
        }

        //Game
        // elige random de lista, la guarda, elminia de la lista y se lo da al jugador, true = jugador, false = dealer
    }
}
