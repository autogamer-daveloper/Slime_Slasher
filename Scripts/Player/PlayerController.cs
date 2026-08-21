using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("__ Input Type __")]
    [SerializeField] private InputType type;

    [Header("__ Main __")]
    [SerializeField] private float speed;
    [SerializeField] private Joystick joystick;

    [Header("__ Animation __")]
    [SerializeField] private GameObject idleAnim;
    [SerializeField] private GameObject runAnim;

    [Header("__ Camera animation __")]
    [SerializeField] private GameObject cameraAnim;
    [SerializeField] private bool isAnimated = true;

    [Header("__ Movement vector __")]
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform aimVector;
    [SerializeField] private float aimMaxDistance = 2f;
    [SerializeField] private float aimDeadzone = 0.25f;
    [SerializeField] private float angleOffset = 0f;

    [Header("__ Audio Settings __")]
    [SerializeField] private AudioSource src;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Vector2 _lastAimDir = Vector2.right;

    private bool facingRight = true;
    private bool isIdle = true;

    private bool lockedInput = false;

    private Animation cameraAnimation;

    private bool isMobileInput = false;
    private bool dialogueBlock = false;

    private void Awake()
    {
        if (type != null) { isMobileInput = type.IsMobileInput(); }
        else { isMobileInput = true; }

        if (isMobileInput) { joystick.gameObject.SetActive(true); }
        else { joystick.gameObject.SetActive(false); }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (cameraAnim != null && isAnimated)
        {
            cameraAnimation = cameraAnim.GetComponent<Animation>();
            if (cameraAnimation == null) { Debug.LogWarning("На cameraAnim нет компонента Animation!"); }
        }

        if (src != null) { src.mute = true; }
    }

    private void Update()
    {
        if (dialogueBlock)
        {
            moveInput = new Vector2(0, 0);
            return;
        }

        if (lockedInput)
            {
                moveInput = new Vector2(0, 0);
                moveVelocity = moveInput.normalized * 0f;
                return;
            }

        if (isMobileInput) { moveInput = new Vector2(joystick.Horizontal, joystick.Vertical); }
        else
        {
            Vector2 keyboardInput = Vector2.zero;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed) { keyboardInput.x -= 1f; }
                if (Keyboard.current.dKey.isPressed) { keyboardInput.x += 1f; }
                if (Keyboard.current.sKey.isPressed) { keyboardInput.y -= 1f; }
                if (Keyboard.current.wKey.isPressed) { keyboardInput.y += 1f; }
            }
            moveInput = keyboardInput.normalized;
        }
        moveVelocity = moveInput.normalized * speed;

        if (isMobileInput) { HandleAimWithJoystick(); }
        else { HandleAimWithMouse(); }

        if (!facingRight && moveInput.x > 0) { Flip(); }
        else if (facingRight && moveInput.x < 0) { Flip(); }

        bool nowIdle = moveInput.x == 0f && moveInput.y == 0f;

        if (nowIdle && !isIdle)
        {
            if (idleAnim != null) idleAnim.SetActive(true);
            if (runAnim != null) runAnim.SetActive(false);

            SetCameraIdle();
            isIdle = true;
        }
        else if (!nowIdle && isIdle)
        {
            if (idleAnim != null) idleAnim.SetActive(false);
            if (runAnim != null) runAnim.SetActive(true);

            SetCameraRun();
            isIdle = false;
        }
    }

    private void FixedUpdate() { rb.MovePosition(rb.position + moveVelocity * Time.deltaTime); }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void SetCameraIdle()
    {
        if (!isAnimated) return;
        if (cameraAnimation == null) return;
        if (!cameraAnimation.IsPlaying("Idle")) { cameraAnimation.Play("Idle"); }
        if (src != null) { src.mute = true; }
    }

    private void SetCameraRun()
    {
        if (!isAnimated) return;
        if (cameraAnimation == null) return;
        if (!cameraAnimation.IsPlaying("Run")) { cameraAnimation.Play("Run"); }
        if (src != null) { src.mute = false; }
    }

    internal void LockInput() { lockedInput = true; }

    internal void UnlockInput() { lockedInput = false; }

    private void HandleAimWithJoystick()
    {
        if (joystick != null)
        {
            Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);

            if (input.sqrMagnitude > aimDeadzone * aimDeadzone)
            {
                Vector2 norm = input.normalized;
                _lastAimDir = norm;

                if (aimTarget != null)
                {
                    Vector3 worldPos = transform.position + new Vector3(norm.x, norm.y, 0f) * aimMaxDistance;
                    aimTarget.transform.position = worldPos;
                }

                if (aimVector != null)
                {
                    float angle = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;
                    aimVector.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                }
            }
            else
            {
                if (aimTarget != null)
                {
                    Vector3 worldPos = transform.position + new Vector3(_lastAimDir.x, _lastAimDir.y, 0f) * aimMaxDistance;
                    aimTarget.transform.position = worldPos;
                }
                if (aimVector != null)
                {
                    float angle = Mathf.Atan2(_lastAimDir.y, _lastAimDir.x) * Mathf.Rad2Deg;
                    aimVector.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                }
            }
        }
        else
        {
            if (Camera.main == null) return;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Vector3 dir = mouseWorld - transform.position;
            if (dir.sqrMagnitude > 0.001f)
            {
                Vector2 norm = dir.normalized;
                _lastAimDir = norm;
                if (aimTarget != null) { aimTarget.transform.position = transform.position + (Vector3)(norm * aimMaxDistance); }

                if (aimVector != null)
                {
                    float angle = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;
                    aimVector.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                }
            }
        }
    }

    private void HandleAimWithMouse()
    {
        if (Camera.main == null || Mouse.current == null) { return; }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        mouseWorld.z = transform.position.z;

        Vector2 direction = mouseWorld - transform.position;

        if (direction.sqrMagnitude <= 0.001f) { return; }

        Vector2 normalizedDirection = direction.normalized;

        _lastAimDir = normalizedDirection;

        if (aimTarget != null)
        {
            aimTarget.position = transform.position + (Vector3)(normalizedDirection * aimMaxDistance);
        }

        if (aimVector != null)
        {
            float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;

            aimVector.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
        }
    }

    internal void DialogueBlock(bool status) { dialogueBlock = status; }
}
