using UnityEngine;

public class PlayerSneeze : Singleton<PlayerSneeze>
{
    [SerializeField] KeyCode key = KeyCode.Space;
    [SerializeField] Transform sneezeArea;
    [SerializeField] float radius = 1;

    [SerializeField, Min(0)] float reloadTime = 1;
    [SerializeField, Min(0.01f)] float putBackMaskTime = 2;
    [SerializeField] GameObject mask;
    [SerializeField] Particle particlePrefab;
    [SerializeField] Transform particlesSpawn;
    [SerializeField] int nbParticles = 5;
    
    bool reloading = false;
    bool hasMask = true;

    PlayerMovement playerMovement;
    Animator animator;

    public bool HasMask => hasMask;

    public AudioSource[] caughList;
    public AudioSource audioMask;
    AudioManager audioManager;
    
    void OnDrawGizmosSelected()
    {
        DrawSneezeZone();
    }

    void DrawSneezeZone()
    {
        Gizmos.color = Color.yellow;
        GizmosExtension.DrawCircle(sneezeArea.position, transform.forward, radius);
    }

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
        sneezeArea.localScale = Vector3.one * radius * 2;
    }

    void Update()
    {
        SneezeAreaMatchDir();

        if (Input.GetKeyDown(key) &&
            !reloading &&
            Time.timeScale > 0)
            Sneeze();
    }

    void Sneeze()
    {
        animator.Play("HumanSquish", 0, 0f);

        audioManager.PlayVoices(caughList);

        InfectsInCircle(sneezeArea.position, radius);

        SpawnParticles(sneezeArea.position + Vector3.up * (particlesSpawn.position.y - transform.position.y));

        if (reloadTime > 0)
            Reload();

        LoseMask();
    }

    void InfectsInCircle(Vector3 center, float radius)
    {
        foreach (Infectable infectable in Infectable.notInfectedSet.Clone())
        {
            float radiusSum = radius + infectable.radius;
            float sqrRadius = radiusSum * radiusSum;

            if ((center - infectable.transform.position).sqrMagnitude > sqrRadius)
                continue;

            infectable.SetInfected(true);
                 
            if (!Timer.Instance.Started)
                Timer.Instance.StartTimer();

            if (InputsDisplay.Instance &&
                !InputsDisplay.Instance.FadeStarted)
                InputsDisplay.Instance.StartFade();
        }
    }

    void Reload()
    {
        if (reloading)
            CancelInvoke("StopReloading");

        reloading = true;
        Invoke("StopReloading", reloadTime);
    }

    void StopReloading() => reloading = false;

    void LoseMask()
    {
        if (!hasMask)
            CancelInvoke("PutMask");

        PutMask(false);
        Invoke("PutMask", putBackMaskTime);
    }

    void PutMask(bool putMask = true)
    {
        if (hasMask == putMask)
            return;

        hasMask = putMask;
        mask.SetActive(hasMask);

        if (putMask)
        {
            animator.Play("HumanSquish", 0, 0f);
            audioManager.PlayMask(audioMask);
        }
    }

    void PutMask() => PutMask(true);

    void SpawnParticles(Vector3 center)
    {
        for (int i = 0; i < nbParticles; i++)
        {
            Vector3 targetPosition = center + Quaternion.Euler(0, 0, Random.value * 360) * Vector3.up * Random.value * radius;
            Particle particle = Instantiate(particlePrefab, particlesSpawn.position, Quaternion.identity);
            particle.StartGoToTarget(targetPosition);
        }
    }

    void SneezeAreaMatchDir()
    {
        sneezeArea.position = transform.position + (Vector3)playerMovement.LookDir * radius;
    }
}
