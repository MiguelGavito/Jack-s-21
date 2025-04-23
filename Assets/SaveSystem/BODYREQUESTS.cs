using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadPlayerScore : MonoBehaviour
{
    private string APIurl = "http://localhost:5000/";
    private int OwnerID;

    void Start()
    {
        // Leer el ID del jugador desde la URL (si aplica)
        int questionIndex = Application.absoluteURL.IndexOf("?");
        if (questionIndex != -1)
        {
            string param = Application.absoluteURL.Split('?')[1];
            int equalIndex = param.IndexOf("=");
            if (equalIndex != -1)
            {
                OwnerID = int.Parse(param.Split('=')[1]);
            }
        }
        else
        {
            OwnerID = 1; // valor por defecto
        }

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        string fullURL = APIurl + OwnerID;
        Debug.Log("Consultando: " + fullURL);

        using (UnityWebRequest web = UnityWebRequest.Get(fullURL))
        {
            yield return web.SendWebRequest();

            if (web.result == UnityWebRequest.Result.ConnectionError || web.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error al conectar con la API: " + web.error);
            }
            else
            {
                Debug.Log("Respuesta: " + web.downloadHandler.text);

                // Si tienes una clase User con highScore
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(web.downloadHandler.text);
                Debug.Log("High Score del jugador: " + playerData.highScore);

                // Aquí podrías guardar el high score localmente si quieres
                SaveManager.SaveHighScore(playerData.highScore);
            }
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public int OwnerID;
        public int highScore;
    }
}
