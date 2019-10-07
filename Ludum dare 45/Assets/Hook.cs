using UnityEngine;

public class Hook : MonoBehaviour
{
    internal bool HasObjectGrabbed => _grabbedObject != null;

    [SerializeField] private Transform _sheepHolder;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _throwDuration = 2f;
    [SerializeField] private SoundEffect _sheepSoundEffect;

    private IGrabbable _grabbedObject;

    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.OnLeftMouseChanged += HandleLeftMouseChanged;
    }

    private void HandleLeftMouseChanged(bool isPressed)
    {
        if (!isPressed) { return; }
        if (_grabbedObject == null) { return; }
        SetGrabbedObject(null);
    }

    internal void SetGrabbedObject(IGrabbable newGrabbedObject)
    {
        if (_grabbedObject != null)
        {
            _grabbedObject.Released(_sheepHolder.forward * _throwForce, _throwDuration);
            AudioManager.Instance.PlaySoundEffect(_sheepSoundEffect);
        }

        newGrabbedObject?.Grabbed(_sheepHolder);
        _grabbedObject = newGrabbedObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.GetComponent<IGrabbable>();
        if (grabbable == null) { return; }
        SetGrabbedObject(grabbable);
    }

    private void OnDisable()
    {
        _playerInput.OnLeftMouseChanged -= HandleLeftMouseChanged;
    }
}
