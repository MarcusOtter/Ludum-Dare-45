using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Sheep : MonoBehaviour, IGrabbable
{
    internal bool IsDead { get; private set; }
    internal bool IsSafe { get; private set; }

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _detectionRange;
    [SerializeField] [Range(0f, 0.2f)] private float _breakAmount = 0.05f;

    private Transform _playerTransform;
    private Transform _attatchPoint;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private bool _beingHeld;
    private bool _beingThrown;
    private bool _isPaused;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<PlayerMove>().transform;
        LevelManager.Instance.OnPauseChanged += HandlePauseChanged;
    }

    private void HandlePauseChanged(bool paused)
    {
        _isPaused = paused;
        _rigidbody.isKinematic = paused;
    }

    private void Update()
    {
        if (_isPaused) { return; }

        _collider.enabled = !_beingHeld;

        if (_beingHeld)
        {
            transform.position = _attatchPoint.position;
            return;
        }

        Vector3 currentVelocity = _rigidbody.velocity;

        if (_beingThrown) { return; }

        Vector3 playerDirection = _playerTransform.position - transform.position;

        // Start breaking if the sheep stops detecting the player
        if (playerDirection.magnitude > _detectionRange)
        {

            if (currentVelocity.sqrMagnitude < 0.05f)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            float velocityFactor = 1 - _breakAmount;
            _rigidbody.velocity = new Vector3(currentVelocity.x * velocityFactor,
                                              currentVelocity.y,
                                              currentVelocity.z * velocityFactor);
            return;
        }

        Vector3 movementDirection = playerDirection.normalized * -_movementSpeed;
        movementDirection.y = currentVelocity.y;
        _rigidbody.velocity = movementDirection;
    }

    public void Grabbed(Transform attatchPoint)
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _beingHeld = true;
        _attatchPoint = attatchPoint;
    }

    public void Released(Vector3 force, float throwDuration)
    {
        _beingHeld = false;
        _beingThrown = true;

        _rigidbody.useGravity = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(WaitForThrowDelay(throwDuration));
    }

    private IEnumerator WaitForThrowDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _beingThrown = false;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnPauseChanged -= HandlePauseChanged;
    }
}
