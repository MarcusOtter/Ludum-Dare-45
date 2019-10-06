using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerMove))]
public class PlayerGraphics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _armAnimator;
    [SerializeField] private GameObject _mainBody;
    [SerializeField] private GameObject _armlessBody;
    [SerializeField] private Hook _hook;

    [Header("Animator settings")]
    [SerializeField] private string _velocityParameterName = "Velocity";
    [SerializeField] private string _chargeParameterName = "Charging";
    [SerializeField] private string _releaseParameterName = "Release";
    [SerializeField] private float _chargeTime = 0.5f;

    private int _velocityParameterHash;
    private int _chargeParameterHash;
    private int _releaseParamaterHash;

    private Animator _movementAnimator;
    private PlayerMove _playerMove;
    private PlayerInput _playerInput;

    private float _holdTimer;

    private void Awake()
    {
        _movementAnimator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();

        _velocityParameterHash = Animator.StringToHash(_velocityParameterName);
        _chargeParameterHash = Animator.StringToHash(_chargeParameterName);
        _releaseParamaterHash = Animator.StringToHash(_releaseParameterName);
    }

    private void Start()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        _movementAnimator.SetFloat(_velocityParameterHash, _playerMove.CurrentVelocity);

        if (_hook.HasObjectGrabbed) { return; }

        bool isHoldingLeftMouse = _playerInput.LeftMouseIsPressed;

        bool isCharging = _armAnimator.GetBool(_chargeParameterHash);
        bool isReleasing = _armAnimator.GetBool(_releaseParamaterHash);

        _mainBody.SetActive(!(isCharging || isReleasing || isHoldingLeftMouse));
        _armlessBody.SetActive(isCharging || isReleasing || isHoldingLeftMouse);

        if (isHoldingLeftMouse)
        {
            _armAnimator.SetBool(_chargeParameterHash, true);
            _holdTimer += Time.deltaTime;
            if (_holdTimer > _chargeTime) { _armAnimator.SetBool(_releaseParamaterHash, true); }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _holdTimer = 0;
            _armAnimator.SetBool(_chargeParameterHash, false);
        }
    }
}
