using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    public static HashSet<HumanMovement> set = new();

    [SerializeField] Vector2 speedRange = new Vector2(1, 3);
    [SerializeField] float walkSpeedCoef = 1;

    float speed;
    Vector2 moveDir = Vector2.down;

    Animator animator;

    public Vector2 MoveDir
    {
        get => moveDir;
        set => moveDir = value.normalized;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        speed = Random.Range(speedRange.x, speedRange.y);
        animator.SetFloat("walkSpeed", speed * walkSpeedCoef);
        animator.SetBool("isWalking", true);
        set.Add(this);
    }

    void OnDestroy()
    {
        set.Remove(this);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += (Vector3)moveDir * Time.deltaTime * speed;
    }
}
