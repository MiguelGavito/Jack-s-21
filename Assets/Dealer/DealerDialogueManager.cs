using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Para usar Toggle
using System;
using System.Diagnostics;

public class DealerDialogueManager : MonoBehaviour
{
    [Header("Dynamic Dialogue Settings")]
    public TMP_InputField apiKeyInput; // Campo de entrada para la API Key
    public Toggle dynamicDialogueToggle; // Casilla para activar/desactivar el diálogo dinámico
    public UnityAndGeminiV3 geminiAPI; // Referencia al script UnityAndGeminiV3
    public TMP_Text dialogueText; // Texto en pantalla para mostrar el diálogo
    public AudioSource dialogueAudio; // AudioSource para reproducir sonidos
    public AudioClip typingSound; // Sonido de escritura

    [Header("Static Dialogue Settings")]
    public TextAsset staticDialogueJson; // Archivo JSON con los diálogos estáticos
    private Dictionary<string, string[]> staticDialogues; // Diccionario para almacenar los diálogos estáticos

    private void Start()
    {
        // Cargar los diálogos estáticos desde el JSON
        if (staticDialogueJson != null)
        {
            staticDialogues = LoadStaticDialogues(staticDialogueJson.text);
        }
        else
        {
            UnityEngine.Debug.LogError("No se asignó un archivo JSON para los diálogos estáticos.");
        }

        // Configurar la API Key en UnityAndGeminiV3 si se proporciona
        if (geminiAPI != null && apiKeyInput != null)
        {
            geminiAPI.SetApiKey(apiKeyInput.text);
        }
    }

    public void Say(string message)
    {
        if (apiKeyInput != null)
        {
            geminiAPI.SetApiKey(apiKeyInput.text); // Actualiza la clave API
        }
        if (dynamicDialogueToggle.isOn && geminiAPI != null)
        {
            // Usar diálogo dinámico
            StartCoroutine(SayDynamic(message));
        }
        else
        {
            // Usar diálogo estático con el mensaje directamente
            SayStatic(message);
        }
    }

    private void SayStatic(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            StartCoroutine(TypeText(message));
        }
        else
        {
            UnityEngine.Debug.LogWarning("El mensaje proporcionado está vacío o es nulo.");
        }
    }

    private IEnumerator SayDynamic(string type)
    {
        // Crear el prompt para la IA
        string prompt = CreatePrompt(type);

        // Enviar el prompt a la API de Gemini
        yield return geminiAPI.SendPromptRequestToGemini(prompt);

        // Obtener la respuesta generada por la IA
        string generatedText = geminiAPI.GetLastResponse();

        if (!string.IsNullOrEmpty(generatedText))
        {
            StartCoroutine(TypeText(generatedText));
        }
        else
        {
            UnityEngine.Debug.LogWarning("No se recibió texto generado por la IA. Usando diálogo estático como respaldo.");
            SayStatic(type); // Volver al modo estático si no hay respuesta
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;

            // Reproducir sonido de escritura
            if (typingSound != null && dialogueAudio != null)
            {
                dialogueAudio.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(0.05f); // Velocidad de escritura
        }
    }

    private string CreatePrompt(string type)
    {
        // Crear un prompt claro para la IA
        return $"Eres un dealer en un juego de cartas. Tu trabajo es interactuar con los jugadores de forma breve y en personaje. " +
               $"El tipo de frase que necesitas generar es: \"{type}\". " +
               $"Ejemplo de frases previas: {GetExamplePhrases(type)}.";
    }

    private string GetExamplePhrases(string type)
    {
        if (staticDialogues != null && staticDialogues.ContainsKey(type))
        {
            string[] phrases = staticDialogues[type];
            return string.Join(", ", phrases);
        }
        return "No hay ejemplos disponibles.";
    }

    private Dictionary<string, string[]> LoadStaticDialogues(string jsonText)
    {
        // Cargar el JSON en un diccionario
        return JsonUtility.FromJson<DialogueDictionary>(jsonText).ToDictionary();
    }

    [System.Serializable]
    public class DialogueDictionary
    {
        public List<DialogueEntry> entries;

        public Dictionary<string, string[]> ToDictionary()
        {
            Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
            foreach (var entry in entries)
            {
                dictionary[entry.key] = entry.values;
            }
            return dictionary;
        }
    }

    [System.Serializable]
    public class DialogueEntry
    {
        public string key;
        public string[] values;
    }
}