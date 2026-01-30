using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    [SerializeField] float acceleration = 70;
    [SerializeField] float drag = 10;
    [SerializeField] float radius = 0.5f;

    [SerializeField] KeyCode upKey = KeyCode.UpArrow;
    [SerializeField] KeyCode leftKey = KeyCode.LeftArrow;
    [SerializeField] KeyCode downKey = KeyCode.DownArrow;
    [SerializeField] KeyCode rightKey = KeyCode.RightArrow;

    [SerializeField] float walkSpeedCoef = 1;

    Vector2 velocity = Vector2.zero;
    Vector2 lookDir = Vector2.down;
    Animator animator;

    public Vector2 LookDir => lookDir;

    void OnDrawGizmosSelected()
    {
        DrawRadius();   
    }

    void DrawRadius()
    {
        Gizmos.color = Color.white;
        GizmosExtension.DrawCircle(transform.position, transform.forward, radius);
    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateLookDir();
        ApplyVelocity();
        ClampPosInScreen();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        ApplyAcceleration();
        ApplyDrag();
    }

    void ApplyAcceleration()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(upKey)     || Input.GetKey(KeyCode.UpArrow))      input += Vector2.up;
        if (Input.GetKey(leftKey)   || Input.GetKey(KeyCode.LeftArrow))    input += Vector2.left;
        if (Input.GetKey(downKey)   || Input.GetKey(KeyCode.DownArrow))    input += Vector2.down;
        if (Input.GetKey(rightKey)  || Input.GetKey(KeyCode.RightArrow))   input += Vector2.right;

        if (input == Vector2.zero)
            return;
        
        input.Normalize();

        velocity += input * acceleration * Time.fixedDeltaTime;
    }

    void ApplyDrag()
    {
        velocity *= 1 - drag * Time.fixedDeltaTime;
    }

    void ApplyVelocity()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    void ClampPosInScreen()
    {
        Vector3 pos = transform.position;

        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x + radius, topRight.x - radius);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y + radius, topRight.y - radius);

        transform.position = pos;
    }

    void UpdateLookDir()
    {
        if (velocity.sqrMagnitude < 0.01f)
            return;

        lookDir = velocity.normalized;
    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", velocity.sqrMagnitude > 0.1f);
        animator.SetFloat("walkSpeed", velocity.magnitude * walkSpeedCoef);
    }
}
