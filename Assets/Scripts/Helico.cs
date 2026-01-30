using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Helico : MonoBehaviour
{    
    public static HashSet<Helico> set = new();

    [SerializeField] float speed = 2;
    [SerializeField] float randRotSpeed = 1;
    [SerializeField] float centerRotSpeed = 1;
    [SerializeField] float perlinTimeCoef = 1;
    float perlinOffset;
    [SerializeField] float radius = 4;
    Vector2 moveDir = Vector2.down;

    public AudioSource audioAlarm;
    AudioManager audioManager;

    public Vector2 MoveDir
    {
        get => moveDir;
        set => moveDir = value.normalized;
    }


    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        perlinOffset = Random.value * 1000;
        transform.localScale = Vector3.one * radius * 2;
        set.Add(this);

        audioManager.PlayAlarm(audioAlarm);
    }

    void OnDestroy()
    {
        set.Remove(this);
    }
    
    void Update()
    {
        RandomRotate();
        RotateToCenter();
        Move();

        if ((LevelsManager.Instance.CrtLevel.totalLockdown || !PlayerSneeze.Instance.HasMask) &&
            (PlayerSneeze.Instance.transform.position - transform.position).sqrMagnitude < radius * radius)
            foreach (PoliceManMovement pmm in PoliceManMovement.set.Where((pmm) => !pmm.PlayerDetected))
                pmm.SetPlayerDetected(true);
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

    void Move()
    {
        transform.position += (Vector3)moveDir * Time.deltaTime * speed;
    }

}
