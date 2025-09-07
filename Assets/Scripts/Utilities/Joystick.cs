using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Joystick Settings")]
    public float joystickRange = 50f;
    public float deadZone = 0.1f;
    
    [Header("Visual")]
    public RectTransform background;
    public RectTransform handle;
    
    private Vector2 inputVector;
    private bool isPressed = false;
    
    public Vector2 InputVector => inputVector;
    public bool IsPressed => isPressed;
    
    void Start()
    {
        if (background == null)
            background = GetComponent<RectTransform>();
        if (handle == null)
            handle = transform.GetChild(0).GetComponent<RectTransform>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        OnDrag(eventData);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x) * 2;
            pos.y = (pos.y / background.sizeDelta.y) * 2;
            
            inputVector = (pos.magnitude > 1f) ? pos.normalized : pos;
            inputVector = (inputVector.magnitude < deadZone) ? Vector2.zero : inputVector;
            
            handle.anchoredPosition = inputVector * joystickRange;
        }
    }
}
