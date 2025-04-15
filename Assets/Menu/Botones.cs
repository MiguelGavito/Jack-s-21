using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public void Inicio()
    {
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
        SceneManager.LoadScene (0);
    }
}
