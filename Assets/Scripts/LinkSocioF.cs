using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Hyperlink : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text pTextMeshPro;
    private Camera cam;

    void Awake()
    {
        pTextMeshPro = GetComponent<TMP_Text>();
        cam = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, cam); // PASA LA CÁMARA!

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID(); // URL o "debug"

            if (linkId == "debug")
            {
                Debug.Log("link clicked");
            }
            else
            {
                Application.OpenURL(linkId);
            }
        }
    }
}