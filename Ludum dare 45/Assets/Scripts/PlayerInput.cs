using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    internal float MouseDeltaX { get; private set; }
    internal float ScrollWheel { get; private set; }

    internal float HorizontalAxis { get; private set; }
    internal float VerticalAxis { get; private set; }

    internal bool LeftMouseIsPressed { get; private set; }

    internal event EventHandler OnRestartKeyPressed;
    internal event EventHandler OnPauseKeyPressed;

    internal event MouseState OnRightMouseChanged;
    internal event MouseState OnLeftMouseChanged;
    internal delegate void MouseState(bool isPressed);

    [SerializeField] private KeyCode _restartKey = KeyCode.R;
    [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;

    private bool _isPaused;

    private void Start()
    {
        LevelManager.Instance.OnPauseChanged += HandlePauseChanged;
    }

    private void HandlePauseChanged(bool paused)
    {
        _isPaused = paused;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_pauseKey))
        {
            OnPauseKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(_restartKey))
        {
            OnRestartKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        if (_isPaused) { return; }

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

    }

    private void OnDisable()
    {
        LevelManager.Instance.OnPauseChanged -= HandlePauseChanged;
    }
}
