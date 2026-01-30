using System.Collections.Generic;
using UnityEngine;

public class PoliceManMovement : MonoBehaviour
{    
    public static HashSet<PoliceManMovement> set = new();

    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 4;
    [SerializeField] float runTime = 5;
    [SerializeField] float randRotSpeed = 1;
    [SerializeField] float centerRotSpeed = 1;
    [SerializeField] float runRotSpeed = 1;
    [SerializeField] float perlinTimeCoef = 1;
    float perlinOffset;
    [SerializeField] float walkSpeedCoef = 1;
    [SerializeField] float detectionRadius = 1;
    [SerializeField] Transform detectionArea;
    [SerializeField] float catchRadius = 1;
    [SerializeField] GameObject exPoint;

    Vector2 moveDir = Vector2.down;
    bool playerDetected = false;

    Animator animator;

    public AudioSource[] audioDetectedList;
    AudioManager audioManager;

    public Vector2 MoveDir
    {
        get => moveDir;
        set => moveDir = value.normalized;
    }

    public bool PlayerDetected => playerDetected;

    void OnDrawGizmosSelected()
    {
        DrawCatchRadius();
        DrawDetectionRadius();
    }

    void DrawCatchRadius()
    {
        Gizmos.color = Color.red;
        GizmosExtension.DrawCircle(transform.position, Vector3.forward, catchRadius);
    }

    void DrawDetectionRadius()
    {
        Gizmos.color = Color.yellow;
        GizmosExtension.DrawCircle(detectionArea.position, Vector3.forward, detectionRadius);
    }

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        animator = GetComponent<Animator>();
        animator.SetFloat("walkSpeed", walkSpeed * walkSpeedCoef);
        animator.SetBool("isWalking", true);
        perlinOffset = Random.value * 1000;
        detectionArea.localScale = Vector3.one * detectionRadius * 2;
        DetectionAreaMatchDir();
        set.Add(this);
    }

    void OnDestroy()
    {
        set.Remove(this);
    }
    
    void Update()
    {
        if (!playerDetected)
        {
            RandomRotate();
            RotateToCenter();
            DetectionAreaMatchDir();
            Move(walkSpeed);
            TriggerDetectionArea();
        }

        else {
            RandomRotate();
            RotateToPlayer();
            DetectionAreaMatchDir();
            Move(runSpeed);
            TryCatchPlayer();
        }
    }

    void LateUpdate()
    {
        DetectionAreaMatchDir();

    }

    void RandomRotate()
    {
        float rotSpeed = Mathf.Lerp(-randRotSpeed, randRotSpeed, Mathf.PerlinNoise1D(perlinOffset + Time.time * perlinTimeCoef));
        moveDir = Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime) * moveDir;
    }

    void RotateToCenter()
    {
        Vector2 toCenter = -transform.position;
        float angle = Vector2.SignedAngle(moveDir, toCenter);
        float rotSpeed = centerRotSpeed * angle / 180f;
        moveDir = Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime) * moveDir;
    }

    void RotateToPlayer()
    {
        Vector2 toPlayer = PlayerSneeze.Instance.transform.position - transform.position;
        float angle = Vector2.SignedAngle(moveDir, toPlayer);
        float rotSpeed = angle < 0 ? -runRotSpeed : runRotSpeed;
        moveDir = Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime) * moveDir;
    }

    void Move(float speed)
    {
        transform.position += (Vector3)moveDir * Time.deltaTime * speed;
    }

    void DetectionAreaMatchDir()
    {
        detectionArea.position = transform.position + (Vector3)moveDir * detectionRadius;
    }

    void TriggerDetectionArea()
    {
        if (playerDetected)
            return;

        if ((LevelsManager.Instance.CrtLevel.totalLockdown || !PlayerSneeze.Instance.HasMask) &&
            (PlayerSneeze.Instance.transform.position - detectionArea.position).sqrMagnitude < detectionRadius * detectionRadius)
            SetPlayerDetected(true);
    }

    public void SetPlayerDetected(bool playerDetected)
    {
        if (this.playerDetected == playerDetected)
            return;
        
        this.playerDetected = playerDetected;

        detectionArea.gameObject.SetActive(!playerDetected);
        animator.SetFloat("walkSpeed", (playerDetected ? runSpeed : walkSpeed) * walkSpeedCoef);

        exPoint.SetActive(playerDetected);

        if (playerDetected)
        {
            Invoke("ResetPlayerDetected", runTime);

            animator.Play("HumanSquish", 0, 0f);

            Vector2 toPlayer = PlayerSneeze.Instance.transform.position - transform.position;
            moveDir = toPlayer.normalized;

            audioManager.PlayVoices(audioDetectedList);
        }

        else {
            //moveDir = Quaternion.Euler(0, 0, Random.value * 360f) * Vector2.up;
        }
    }

    void ResetPlayerDetected() => SetPlayerDetected(false);

    void TryCatchPlayer()
    {
        if (!playerDetected)
            return;

        if ((PlayerSneeze.Instance.transform.position - transform.position).sqrMagnitude < catchRadius * catchRadius)
            GameOverMenu.Instance.gameObject.SetActive(true);
    }
}
