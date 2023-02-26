using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Coots : MonoBehaviour
{
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] Animator animator;
    [SerializeField] new Transform camera;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform paw;
    [SerializeField] Transform pawSpawn;
    [SerializeField] Transform pawDisplay;
    [SerializeField] LayerMask solid;

    [SerializeField] float walkingSpeed = 3f;
    [SerializeField] float runningSpeed = 6f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpForceBonus = 25f;

    public float turnSmoothTime = 0.1f;

    [Header("Sounds")]
    [SerializeField] private float runSoundDelay = 0.5f;
    [SerializeField] private AudioSource runSoundEffect;
    [SerializeField] private float stepSoundDelay = 0.3f;
    [SerializeField] private AudioSource stepSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;

    bool isPaused = true;
    float turnSmoothVelocity;
    float timeSinceMove = 10f;
    bool isFiring = false;
    float pawForce = 50f;
    float pawMass = 10f;
    float pawHitTime = 0.1f;
    bool isJumpBonusActive = false;

    public float GetJumpForce() {
        if (!isJumpBonusActive)
        {
            return jumpForce;
        }

        return jumpForce + jumpForceBonus;
    }

    public void HandleGameStarted(Component sender, object data) {
        isPaused = false;
    }

    public void HandleJumpBonusActivated(Component sender, object data) {
        isJumpBonusActive = true;
    }

    public void HandleJumpBonusDeactivated(Component sender, object data) {
        isJumpBonusActive = false;
    }

    public void HandlePauseToggled(Component sender, object data) {
        if (data is bool)
        {
            isPaused = (bool)data;
        }
    }

    void FixedUpdate() {
        HandleMove();
    }

    void Start() {
        PreparePaw();
    }

    void Update() {
        HandleJump();

        HandleFire();

        AnimateMove();
    }

    void PreparePaw() {
        Physics.IgnoreCollision(paw.GetComponent<Collider>(), GetComponent<Collider>());
        Rigidbody pawRb = paw.GetComponent<Rigidbody>();
        pawRb.mass = 0;
        pawDisplay.gameObject.SetActive(false);
    }

    void HandleJump() {
        if (isPaused)
        {
            return;
        }

        bool isInputJumping = Input.GetButtonDown("Jump");

        bool isGrounded = IsGrounded();

        if (isInputJumping && isGrounded)
        {
            VoiceJumping();

            rigidbody.velocity = new Vector3(
                rigidbody.velocity.x,
                GetJumpForce(),
                rigidbody.velocity.z
            );

        }
    }

    void HandleMove() {
        if (isPaused)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isGrounded = IsGrounded();

        bool isInputWalking = Input.GetKey(KeyCode.LeftShift);

        bool animatorIsWalking = animator.GetInteger("Movement") == 1;
        bool animatorIsRunning = animator.GetInteger("Movement") == 2;

        if (direction.magnitude >= 0.0005f)
        {
            if (isInputWalking && !animatorIsWalking) {
                animator.SetInteger("Movement", 1);
            } else if (!isInputWalking && !animatorIsRunning) {
                animator.SetInteger("Movement", 2);
            }

            float speed;
            if (isInputWalking) {
                speed = walkingSpeed;
                VoiceWalking();
            } else {
                speed = runningSpeed;
                VoiceRunning();
            }

            float targetAngle = (
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                camera.eulerAngles.y
            );
            float smoothedAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rigidbody.velocity = new Vector3(
                moveDirection.normalized.x * speed,
                rigidbody.velocity.y,
                moveDirection.normalized.z * speed
            );
        }
        else
        {
            if (animatorIsRunning || animatorIsWalking)
            {
                animator.SetInteger("Movement", 0);
            }
        }
    }

    async void HandleFire() {
        if (isFiring || isPaused)
        {
            return;
        }

        bool isInputFire = Input.GetButtonDown("Fire1");

        if (isInputFire)
        {
            Collider collider = paw.GetComponent<Collider>();
            collider.enabled = true;
            Rigidbody pawRb = paw.GetComponent<Rigidbody>();
            pawRb.mass = pawMass;
            pawRb.isKinematic = false;

            pawDisplay.gameObject.SetActive(true);

            pawRb.AddForce(transform.forward * pawForce, ForceMode.Impulse);

            float delay = pawHitTime * 1000;
            await Task.Delay((int)delay);

            pawRb.velocity = Vector3.zero;
            pawRb.angularVelocity = Vector3.zero;
            paw.transform.position = pawSpawn.transform.position;
            paw.transform.rotation = Quaternion.identity;
            collider.enabled = false;
            pawRb.mass = 0;
            pawRb.isKinematic = true;

            pawDisplay.gameObject.SetActive(false);
        }
    }

    void AnimateMove() {
        bool isGrounded = IsGrounded();

        int animatorMovement = animator.GetInteger("Movement");
        bool animatorIsJumping = animatorMovement == 3;
        bool animatorIsRunning = animatorMovement == 2;
        bool animatorIsWalking = animatorMovement == 1;
        if (!isGrounded && !animatorIsJumping)
        {
            animator.SetInteger("Movement", 3);
        } else if (
            isGrounded &&
            animatorIsJumping &&
            !animatorIsRunning &&
            !animatorIsWalking
        )
        {
            animator.SetInteger("Movement", 0);
        }
    }

    void VoiceRunning() {
        if (!IsGrounded())
        {
            return;
        }

        if (timeSinceMove < runSoundDelay)
        {
            timeSinceMove += 1 * Time.deltaTime;
        }
        else
        {
            runSoundEffect.Play();

            timeSinceMove = 0;
        }
    }

    void VoiceWalking() {
        if (!IsGrounded())
        {
            return;
        }

        if (timeSinceMove < stepSoundDelay)
        {
            timeSinceMove += 1 * Time.deltaTime;
        }
        else
        {
            stepSoundEffect.Play();

            timeSinceMove = 0;
        }
    }

    void VoiceJumping() {
        jumpSoundEffect.Play();
    }

    bool IsGrounded() {
        return Physics.CheckSphere(groundCheck.position, .1f, solid);
    }
}
