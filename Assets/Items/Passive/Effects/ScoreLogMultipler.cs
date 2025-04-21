using UnityEngine;

[CreateAssetMenu(fileName = "ScoreLogMultipler", menuName = "Scriptable Objects/ScoreLogMultipler")]
public class ScoreLogMultipler : ScriptableObject, IPassiveEffect
{
    public void Apply(GameManager gameManager)
    {
        int baseScore = gameManager.puntaje;
        int extra = Mathf.FloorToInt(Mathf.Log10(baseScore + 10));
        gameManager.puntaje += extra;
        Debug.Log($"[Passive] Puntaje aumentado en {extra} por efecto logarítmico.");
    }
}
