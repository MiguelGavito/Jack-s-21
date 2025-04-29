using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void Inicio()
    {
        StartCoroutine(CargarEscenaAsync(1)); // Cambiar a carga asíncrona
    }

    public void InicioNuevoJuego()
    {
        InventoryManager.instance.ResetInventory();
        StartCoroutine(CargarEscenaAsync(1)); // Cambiar a carga asíncrona
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
        StartCoroutine(CargarEscenaAsync(2)); // Cambiar a carga asíncrona
    }

    public void Menu()
    {
        //esto seria el boton de ir al menu y perder progreso
        InventoryManager.instance.ResetInventory(); // reiniciamos el inventario

        StartCoroutine(CargarEscenaAsync(0)); // Cambiar a carga asíncrona
    }

    private IEnumerator CargarEscenaAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
