using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("__ Main __")]
    [SerializeField] private float speed;
    [SerializeField] private Joystick joystick;

    [Header("__ Animation __")]
    [SerializeField] private GameObject idleAnim;
    [SerializeField] private GameObject runAnim;

    [Header("__ Camera animation __")]
    [SerializeField] private GameObject cameraAnim;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    private bool facingRight = true;
    private bool isIdle = true;

    private bool lockedInput = false;

    private Animation cameraAnimation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (cameraAnim != null)
        {
            cameraAnimation = cameraAnim.GetComponent<Animation>();

            if (cameraAnimation == null)
                Debug.LogWarning("На cameraAnim нет компонента Animation!");
        }
    }

    private void Update()
    {
        if (lockedInput)
        {
            moveInput = new Vector2(0, 0);
            moveVelocity = moveInput.normalized * 0f;
            return;
        }

        moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        moveVelocity = moveInput.normalized * speed;

        if (!facingRight && moveInput.x > 0)
            Flip();
        else if (facingRight && moveInput.x < 0)
            Flip();

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

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void SetCameraIdle()
    {
        if (cameraAnimation == null) return;

        if (!cameraAnimation.IsPlaying("Idle"))
            cameraAnimation.Play("Idle");
    }

    private void SetCameraRun()
    {
        if (cameraAnimation == null) return;

        if (!cameraAnimation.IsPlaying("Run"))
            cameraAnimation.Play("Run");
    }

    internal void LockInput()
    {
        lockedInput = true;
    }

    internal void UnlockInput()
    {
        lockedInput = false;
    }
}
