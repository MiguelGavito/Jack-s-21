using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void Inicio()
    {
        SceneManager.LoadScene(1);
    }

    public void InicioNuevoJuego()
    {
        InventoryManager.instance.ResetInventory();
        SceneManager.LoadScene(1);
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Shop()
    {
        SceneManager.LoadScene (2);
    }

    public void Menu()
    {
        //esto seria el boton de ir al menu y perder progreso
        InventoryManager.instance.ResetInventory(); // reiniciamos el inventario

        SceneManager.LoadScene(0); // Volver al menú
    }
}
