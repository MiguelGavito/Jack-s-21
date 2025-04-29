using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    private IEnumerator CargarEscenaAsync(int sceneIndex)
    {
        Debug.Log($"Cargando escena: {sceneIndex}");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("Escena cargada. Activando...");
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        Debug.Log("Cambio de escena completado.");
    }

    public void Inicio()
    {
        StartCoroutine(CargarEscenaAsync(1));
    }

    public void InicioNuevoJuego()
    {
        InventoryManager.instance.ResetInventory();
        StartCoroutine(CargarEscenaAsync(1));
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
        StartCoroutine(CargarEscenaAsync(2));
    }

    public void Menu()
    {
        InventoryManager.instance.ResetInventory();
        StartCoroutine(CargarEscenaAsync(0));
    }
}
