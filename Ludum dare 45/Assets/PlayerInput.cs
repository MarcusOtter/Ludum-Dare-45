using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    internal float MouseDeltaX;
    internal event MouseState OnRightMouseChanged;

    internal delegate void MouseState(bool isPressed);


    private void Update()
    {
        MouseDeltaX = Input.GetAxis("Mouse X");

        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseChanged?.Invoke(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnRightMouseChanged?.Invoke(false);
        }
    }
}
