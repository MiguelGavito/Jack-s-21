using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueAPIManager : MonoBehaviour
{
    public static DialogueAPIManager Instance;

    [Header("Configuración de la API")]
    public string apiKey; // Llave de la API
    public string apiUrl = "https://api.gemini.com/dialogue"; // URL de la API

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Envía una solicitud a la API para obtener un diálogo dinámico.
    /// </summary>
    public IEnumerator GetDynamicDialogue(string category, System.Action<string> onSuccess, System.Action<string> onError)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            onError?.Invoke("API Key no configurada.");
            yield break;
        }

        // Crear el cuerpo de la solicitud
        string jsonBody = JsonUtility.ToJson(new { category = category });

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                onSuccess?.Invoke(response);
            }
            else
            {
                onError?.Invoke($"Error en la solicitud: {request.error}");
            }
        }
    }
}