using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public TextMeshProUGUI recordText;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
