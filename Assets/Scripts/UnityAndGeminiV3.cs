public class UnityAndGeminiV3 : MonoBehaviour
{
    // Mantén las variables existentes...

    public void SetApiKey(string key)
    {
        apiKey = key;
    }

    public IEnumerator GetDealerResponse(string prompt, System.Action<string> onResponse)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key no configurada.");
            onResponse?.Invoke("Error: No se configuró la API key.");
            yield break;
        }

        string url = $"{apiEndpoint}?key={apiKey}";
        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"" + prompt + "\"}]}]}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onResponse?.Invoke("Error al conectar con la API.");
            }
            else
            {
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    string reply = response.candidates[0].content.parts[0].text;
                    onResponse?.Invoke(reply);
                }
                else
                {
                    onResponse?.Invoke("No se recibió respuesta válida.");
                }
            }
        }
    }
}
