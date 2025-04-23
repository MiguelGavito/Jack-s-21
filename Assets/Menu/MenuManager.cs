using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public TextMeshProUGUI recordText;

    public AudioClip musicClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int highScore = SaveManager.LoadHighScore();

        if (recordText != null)
        {
            recordText.text = highScore.ToString();
        }
        else
        {
            Debug.LogError("No se ha asignado el TextMeshProUGUI para mostrar el record.");
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(musicClip);
        }

        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
