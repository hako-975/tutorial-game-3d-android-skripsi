using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cam;
    public GameObject spawnPoint;

    public float turnSmoothTime = 0.1f;
    public float movementSpeed = 4f;
    public float maxFallZone = -100f;

    float gravity = -9.81f;

    Joystick joystick;

    CharacterController cc;

    Animator animator;

    Vector3 move;
    Vector3 velocity;

    float turnSmoothVelocity;
    float horizontal;
    float vertical;

    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();

        // set posisi awal karakter
        cc.enabled = false;
        cc.transform.position = spawnPoint.transform.position;
        cc.enabled = true;

        joystick = FindAnyObjectByType<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        // cek karakter jatuh
        if (cc.transform.position.y < maxFallZone)
        {
            cc.enabled = false;
            cc.transform.position = spawnPoint.transform.position;
            cc.enabled = true;
        }

        horizontal = Input.GetAxisRaw("Horizontal") + joystick.Horizontal;
        vertical = Input.GetAxisRaw("Vertical") + joystick.Vertical;

        if (velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        move = new Vector3(horizontal, 0f, vertical).normalized;

        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(movementSpeed * Time.deltaTime * moveDirection.normalized);
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);

        // animator
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        isRunning = hasHorizontalInput || hasVerticalInput;

        animator.SetBool("IsRunning", isRunning);

    }
}
