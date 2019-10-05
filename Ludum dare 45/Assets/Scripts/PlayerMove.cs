using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private Rigidbody _rigibody;

    private Vector3 _movementVector;

    private void Awake()
    {
        _rigibody = GetComponent<Rigidbody>();   
    }

    private void Update()
    {
        float velocityX = Input.GetAxis("Horizontal");
        float velocityZ = Input.GetAxis("Vertical");

        _movementVector = new Vector3(velocityX, 0, velocityZ).normalized * _movementSpeed;
        _movementVector.y = _rigibody.velocity.y;

        _rigibody.velocity = _movementVector;
    }
}
