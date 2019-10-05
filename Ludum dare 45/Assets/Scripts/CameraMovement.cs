using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    internal Vector3 LookDirection => transform.forward;

    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _minimumZoom = 3f;
    [SerializeField] private float _maximumZoom = 10f;
    [SerializeField] private float _zoomSpeed = 0.1f;

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

    private void Update()
    {
        if (_playerInput.ScrollWheel > 0f && CameraCanZoomIn)
        {
            _camera.orthographicSize -= _zoomSpeed;
        }
        else if (_playerInput.ScrollWheel < 0f && CameraCanZoomOut)
        {
            _camera.orthographicSize += _zoomSpeed;
        }
    }

    private bool CameraCanZoomIn => _camera.orthographicSize > _minimumZoom;
    private bool CameraCanZoomOut => _camera.orthographicSize < _maximumZoom;

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
