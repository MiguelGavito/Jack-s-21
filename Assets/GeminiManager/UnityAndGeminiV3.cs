using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using TMPro;
using System.IO; 
using System;

[System.Serializable]
public class UnityAndGeminiKey
{
    public string key;
}

[System.Serializable]
public class Response
{
    public Candidate[] candidates;
}

public class ChatRequest
{
    public Content[] contents;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class Content
{
    public string role; 
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
    public InlineData inlineData;
}



[System.Serializable]
public class InlineData
{
    public string mimeType;
    public string data;
}


public class UnityAndGeminiV3 : MonoBehaviour
{
    [Header("JSON API Configuration")]
    public TMP_InputField apiKeyInputField; // Campo para ingresar la API Key
    private string apiKey = "";
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    [Header("Dealer Comment Manager")]
    public DealerCommentManager dealerCommentManager; // Referencia al DealerCommentManager

    [Header("UI Elements")]
    public TMP_Text uiText; // Texto para mostrar la respuesta de Gemini

    public void SetApiKey()
    {
        apiKey = apiKeyInputField.text; // Obtiene la API Key del campo de entrada
        Debug.Log("API Key configurada: " + apiKey);
    }

    public void GenerateDealerComments(string sceneType)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key no configurada.");
            return;
        }

        // Construir el prompt con el contexto del dealer y los comentarios del JSON
        string prompt = $"Eres un NPC de un videojuego con temática ligeramente tétrica de un casino. " +
                        $"Tu papel es ser un dealer con cabeza de cabra, ligeramente sarcástico. " +
                        $"Tus comentarios deben ajustarse a este papel. Aquí tienes ejemplos de comentarios:\n\n" +
                        $"{JsonUtility.ToJson(dealerCommentManager.comments, true)}\n\n" +
                        $"Genera comentarios para la escena: {sceneType}.";

        StartCoroutine(SendPromptRequestToGemini(prompt));
    }

    private IEnumerator SendPromptRequestToGemini(string promptText)
    {
        string url = $"{apiEndpoint}?key={apiKey}";

        string jsonData = $@"{{
                ""contents"": [{{
                    ""parts"": [{{
                        ""text"": ""{promptText}""
                    }}]
                }}]
            }}";

        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al contactar con Gemini: " + www.error);
            }
            else
            {
                Debug.Log("Solicitud completada con éxito.");
                string responseText = www.downloadHandler.text;
                Debug.Log("Respuesta de Gemini: " + responseText);

                // Procesar la respuesta
                Response response = JsonUtility.FromJson<Response>(responseText);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    string generatedComment = response.candidates[0].content.parts[0].text;
                    Debug.Log("Comentario generado: " + generatedComment);

                    // Mostrar el comentario en la UI
                    uiText.text = generatedComment;
                }
                else
                {
                    Debug.Log("No se encontraron comentarios generados.");
                }
            }
        }
    }
}


