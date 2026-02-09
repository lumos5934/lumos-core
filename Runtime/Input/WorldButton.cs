using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Button.ButtonClickedEvent onClick = new();
    private bool _isPressed;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isPressed)
        {
            onClick.Invoke();
        }

        _isPressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPressed = false;
    }
}
