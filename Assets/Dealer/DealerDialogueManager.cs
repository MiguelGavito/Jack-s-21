using UnityEngine;
using UnityEditor.UI;
using System.Collections;
using TMPro;

public class DealerDialogueManager : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_Text dialogueText;
    public AudioSource audioSource;
    public AudioClip japingSound;

    [Header("Ajustes")]
    public float typingSpeed = 0.04f; // Velocidad por letra
    public float postDialoguePause = 1.5f; // Tiempo de espera al final del texto

    private Coroutine currentTyping;

    /// <summary>
    /// Llama este metodo para mostrar un mensaje con animacion y sonido
    /// </summary>
    public void Say(string message)
    {
        if (currentTyping != null)
            StopCoroutine(currentTyping);

        currentTyping = StartCoroutine(TypeText(message));
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

}
