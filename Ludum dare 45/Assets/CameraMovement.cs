using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    internal Vector3 LookDirection => transform.forward;

    [SerializeField] private float _rotationSpeed;

    private bool _draggingCamera;

    private PlayerInput _playerInput;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.OnRightMouseChanged += SetCameraDragActive;
    }

    private void SetCameraDragActive(bool activate)
    {
        _draggingCamera = activate;
    }

    private void LateUpdate()
    {
        if (!_draggingCamera) { return; }
        transform.rotation *= Quaternion.Euler(0, _playerInput.MouseDeltaX * _rotationSpeed, 0);
    }

    /// <summary> Returns a value between 0 and 1, 0 being returned when the mouse is all the way to the left. </summary>
    private float GetViewportPositionX(Vector2 mousePosition) => _camera.ScreenToViewportPoint(mousePosition).x;

    private void OnDisable()
    {
        _playerInput.OnRightMouseChanged -= SetCameraDragActive;
    }
}
