using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour 
{
    private static string saveFilePath = Application.persistentDataPath + "/gameData.json"; // ruta donde guardamos el archivo

    // Guardamos el pnnuntaje mas alto en un archivo json
    public static void SaveHighScore(int score)
    {
        GameData gameData = new GameData(); // creamos una nueva instancia de game data
        gameData.highScore = score; // asignamos el puntaje mas alto a la variable

        // Convertimos el objeto gamedata a formato json
        string json = JsonUtility.ToJson(gameData);

        // Guardamos el archivo json en la ruta especificada
        File.WriteAllText(saveFilePath, json);
    }

    // Cargar el puntaje mas alto desde el archivo JSON
    public static int LoadHighScore()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                // Si el archivo existe, lo leemos y lo convertimos de json a un objeto GameData
                string json = File.ReadAllText(saveFilePath);
                GameData gameData = JsonUtility.FromJson<GameData>(json);
                return gameData.highScore; // Devolvemos el puntaje mas alto cargado
            } 
            catch (System.Exception e) 
            {
                Debug.LogError("Error al leer el archivo de guardado: " + e.Message);
                return 0; // En caso de error, devolvemos un valor por defecto
            }
            
        }
        return 0;
    }
}
