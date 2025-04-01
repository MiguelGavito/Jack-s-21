using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public Transform cartaContainer;

    public void RecibirCarta(Carta carta)
    {
        GameObject cartaObj = Instantiate(carta.gameObject);
        cartaObj.transform.SetParent(cartaContainer);
        cartaObj.transform.localPosition = new Vector3(0, 0, 0); // ajustar posicion luego
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
