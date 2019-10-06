using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    internal float MouseDeltaX { get; private set; }
    internal float ScrollWheel { get; private set; }

    internal float HorizontalAxis { get; private set; }
    internal float VerticalAxis { get; private set; }

    internal event MouseState OnRightMouseChanged;
    internal event MouseState OnLeftMouseChanged;
    internal delegate void MouseState(bool isPressed);

    internal bool LeftMouseIsPressed { get; private set; }

    [SerializeField] private KeyCode _restartKey = KeyCode.R;

    private void Update()
    {
        MouseDeltaX = Input.GetAxis("Mouse X");
        ScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        HorizontalAxis = Input.GetAxisRaw("Horizontal");
        VerticalAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseChanged?.Invoke(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnRightMouseChanged?.Invoke(false);
        }

        LeftMouseIsPressed = Input.GetMouseButton(0);

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseChanged?.Invoke(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnLeftMouseChanged?.Invoke(false);
        }

        if (Input.GetKeyDown(_restartKey))
        {
            SceneChanger.Instance?.ReloadScene();
        }
    }
}
