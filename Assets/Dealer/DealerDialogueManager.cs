using UnityEngine;
using System.Collections;
using TMPro;
//using System.Diagnostics;

public class DealerDialogueManager : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_Text dialogueText;
    public AudioSource audioSource;
    public AudioClip japingSound;

    [Header("Ajustes")]
    public float typingSpeed = 0.04f;
    public float postDialoguePause = 1.5f;

    [Header("Configuración de Diálogos")]
    public bool useDynamicDialogue = false; // Alternar entre JSON y API
    public string fallbackJsonCategory = "default"; // Categoría por defecto si falla la API

    private Coroutine currentTyping;

    // Diccionario para almacenar diálogos locales
    private Dictionary<string, List<string>> localDialogues;

    private void Start()
    {
        // Cargar diálogos locales desde un JSON al iniciar
        LoadLocalDialogues();
    }

    public void Say(string category)
    {
        if (currentTyping != null)
            StopCoroutine(currentTyping);

        if (useDynamicDialogue)
        {
            StartCoroutine(DialogueAPIManager.Instance.GetDynamicDialogue(
                category,
                (response) => currentTyping = StartCoroutine(TypeText(response)), // Éxito
                (error) =>
                {
                    Debug.LogError($"Error al obtener diálogo dinámico: {error}");
                    string fallbackDialogue = GetFallbackDialogue(category);
                    currentTyping = StartCoroutine(TypeText(fallbackDialogue));
                }
            ));
        }
        else
        {
            string fallbackDialogue = GetFallbackDialogue(category);
            currentTyping = StartCoroutine(TypeText(fallbackDialogue));
        }
    }

    private IEnumerator TypeText(string message)
    {
        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;

            if (!char.IsWhiteSpace(letter) && japingSound != null && audioSource != null)
                audioSource.PlayOneShot(japingSound);

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(postDialoguePause);

        currentTyping = null;
    }

    private string GetFallbackDialogue(string category)
    {
        // Verificar si la categoría existe en los diálogos locales
        if (localDialogues != null && localDialogues.ContainsKey(category))
        {
            // Seleccionar un diálogo aleatorio de la categoría
            List<string> dialogues = localDialogues[category];
            return dialogues[Random.Range(0, dialogues.Count)];
        }

        // Si no se encuentra la categoría, devolver un mensaje predeterminado
        return $"No se encontró un diálogo para la categoría: {category}";
    }

    private void LoadLocalDialogues()
    {
        // Simulación de carga de un JSON local
        // En un proyecto real, aquí leerías un archivo JSON y lo convertirías en un diccionario
        localDialogues = new Dictionary<string, List<string>>
        {
            { "greetings", new List<string> { "¡Hola!", "¿Cómo estás?", "¡Bienvenido!" } },
            { "victory", new List<string> { "¡Felicidades, has ganado!", "¡Eres el mejor!", "¡Victoria!" } },
            { "defeat", new List<string> { "No te preocupes, inténtalo de nuevo.", "¡Casi lo logras!", "No fue tu día, pero sigue adelante." } }
        };
    }

    public void ToggleDynamicDialogue(bool isEnabled)
    {
        useDynamicDialogue = isEnabled;
        Debug.Log($"Diálogos dinámicos: {(isEnabled ? "Activados" : "Desactivados")}");
    }
}
