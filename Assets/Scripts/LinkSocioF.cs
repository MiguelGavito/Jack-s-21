using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class HyperlinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textMeshPro;
    private Camera mainCamera;

    void Awake()
    {
        // Obtén el componente TMP_Text
        textMeshPro = GetComponent<TMP_Text>();

        // Obtén la cámara principal
        mainCamera = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Depuración: Verifica si el evento de clic se detecta
        Debug.Log($"Pointer clicked at position: {eventData.position}");

        // Encuentra el índice del enlace en la posición del clic
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, null);

        // Depuración: Verifica si se detectó un enlace
        if (linkIndex == -1)
        {
            Debug.Log("No link detected at the clicked position.");
            return;
        }

        TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
        string linkId = linkInfo.GetLinkID(); // Obtén el ID del enlace (URL)

        // Depuración: Verifica el ID del enlace
        Debug.Log($"Link detected! Link ID: {linkId}");

        if (!string.IsNullOrEmpty(linkId))
        {
            Debug.Log($"Opening URL: {linkId}");
            Application.OpenURL(linkId); // Abre la URL en el navegador predeterminado
        }
        else
        {
            Debug.Log("Link ID is empty or null.");
        }
    }
}
