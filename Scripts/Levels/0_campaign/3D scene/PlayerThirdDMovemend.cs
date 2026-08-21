using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;
    [Header("Camera rotation speed")]
    [SerializeField] private float rotateSpeed = 75f;
    [Space(10)]
    [Header("Field Of View")]
    [SerializeField] private float minFov = -80f;
    [SerializeField] private float maxFov = 80f;
    [Space(10)]
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [Header("Character Controller")]
    [SerializeField] private CharacterController character;
    [SerializeField] private Joystick joystick;
    [Header("Gravity")]
    [SerializeField] private float gravityForce = -9.81f;
    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [Header("Input")]
    [SerializeField] private bool isEnabledJump = true;
    [SerializeField] private LookArea lookArea;
    [SerializeField] private bool mobileInput;
    [SerializeField] private float mobileSensitivity = 0.35f;
    [Header("Audio System")]
    [SerializeField] private GameObject movingSource;

    private Vector3 _velocity;
    private Vector2 _direction;

    private Camera _camera;
    private Vector2 _rotation;

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Awake()
    {
        if(!mobileInput) { joystick.gameObject.SetActive(false); }
    }

    private void Start()
    {
        _camera = this.GetComponentInChildren<Camera>();
        movingSource.SetActive(false);
    }

    private void Update() {
        Vector2 mouseVector;
        if (mobileInput)
        {
            _direction = new Vector2(joystick.Horizontal, joystick.Vertical);
            mouseVector = lookArea.IsDragging
                ? lookArea.Delta * mobileSensitivity
                : Vector2.zero;
        }
        else
        {
            _direction = moveAction.action.ReadValue<Vector2>();
            mouseVector = lookAction.action.ReadValue<Vector2>();
        }

        _direction *= moveSpeed;
        Vector3 move = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * new Vector3(_direction.x, 0, _direction.y);
        _velocity = new Vector3(move.x, _velocity.y, move.z);

        if(_direction.x > 0f || _direction.y > 0f) { movingSource.SetActive(true); }
        else { movingSource.SetActive(false); }
        
        character.Move(_velocity * Time.deltaTime);

        if (character.isGrounded && isEnabledJump)
        {
            _velocity.y = jumpAction.action.WasPressedThisFrame()
                ? jumpForce
                : -0.25f;
        }
        else { _velocity.y += gravityForce * Time.deltaTime; }

        mouseVector *= rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseVector.x);
        _rotation.y -= mouseVector.y;
        _rotation.y = Mathf.Clamp(_rotation.y, minFov, maxFov);
        _camera.transform.localRotation = Quaternion.Euler(_rotation.y, 0, 0);
    }
}
