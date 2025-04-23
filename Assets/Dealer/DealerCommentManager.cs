using UnityEngine;

public class DealerCommentManager : MonoBehaviour
{
    public DealerComments comments;

    void Awake()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("Dialogos/dealer_comments");
        if (jsonText != null)
        {
            comments = JsonUtility.FromJson<DealerComments>(jsonText.text);
        }
        else
        {
            Debug.LogError("dealer_comments.json not found in Resources folder.");
        }
    }

    public string GetRandomComment(string category)
    {
        string[] commentArray = (string[])typeof(DealerComments)
            .GetField(category)
            .GetValue(comments);

        if (commentArray != null && commentArray.Length > 0)
        {
            int index = Random.Range(0, commentArray.Length);
            return commentArray[index];
        }

        return "[No comment available]";
    }
}