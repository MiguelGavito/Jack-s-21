using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendHighScore : MonoBehaviour
{
    private string apiUrl = "https://api-crm-livid.vercel.app/";

    public void EnviarHighScore(int OwnerID, int highScore)
    {
        StartCoroutine(PostHighScore(OwnerID, highScore));
    }

    IEnumerator PostHighScore(int OwnerID, int highScore)
    {
        string fullURL = apiUrl + OwnerID + "/highscore";

        // Creamos el objeto con el score
        HighScoreData data = new HighScoreData { highScore = highScore };
        string jsonData = JsonUtility.ToJson(data);

        // Preparamos el request POST
        UnityWebRequest web = new UnityWebRequest(fullURL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        web.uploadHandler = new UploadHandlerRaw(jsonToSend);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");

        // Enviamos y esperamos la respuesta
        yield return web.SendWebRequest();

        if (web.result == UnityWebRequest.Result.ConnectionError || web.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al enviar high score: " + web.error);
        }
        else
        {
            Debug.Log("High score enviado correctamente");
            Debug.Log("Respuesta: " + web.downloadHandler.text);
        }
    }

    [System.Serializable]
    public class HighScoreData
    {
        public int highScore;
    }
}
