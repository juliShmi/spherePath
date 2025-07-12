using UnityEngine;
using UnityEngine.EventSystems;

public class CursorPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D pointerCursor;
    private Texture2D defaultCursor;

    void Start()
    {
        defaultCursor = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
