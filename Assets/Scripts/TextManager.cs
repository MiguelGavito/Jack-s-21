using UnityEngine;
using TMPro;
using System.Collections;

public class TextManager : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_Text messageText;
    public AudioSource audioSource;
    public AudioClip typingSound;

    [Header("Ajustes")]
    public float typingSpeed = 0.04f;
    public float postTypingPause = 1.5f;

    private Coroutine currentMessage;

    /// <summary>
    /// Llama este método para mostrar un mensaje animado
    /// </summary>
    public void Announce(string message)
    {
        if (currentMessage != null)
            StopCoroutine(currentMessage);

        currentMessage = StartCoroutine(TypeMessage(message));
    }

    private IEnumerator TypeMessage(string message)
    {
        messageText.text = "";

        foreach (char letter in message)
        {
            messageText.text += letter;

            if (!char.IsWhiteSpace(letter) && typingSound != null && audioSource != null)
                audioSource.PlayOneShot(typingSound);

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(postTypingPause);

        currentMessage = null;
    }
}