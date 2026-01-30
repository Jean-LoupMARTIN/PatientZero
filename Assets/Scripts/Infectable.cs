using System.Collections.Generic;
using UnityEngine;

public class Infectable : MonoBehaviour
{
    public static HashSet<Infectable> notInfectedSet = new();
    public float radius = 0.5f;
    [SerializeField] float healTime = -1;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color infectedColor = Color.green;
    [SerializeField] int addScore = 1;
    bool isInfected = false;
    Animator animator;

    public AudioSource[] audioScreamList;
    AudioManager audioManager;

    void OnDrawGizmosSelected()
    {
        DrawRadius();
    }

    void DrawRadius()
    {
        Gizmos.color = Color.green;
        GizmosExtension.DrawCircle(transform.position, transform.forward, radius);
    }

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        animator = GetComponent<Animator>();
        notInfectedSet.Add(this);
    }

    void OnDestroy()
    {
        if (!isInfected)
            notInfectedSet.Remove(this);
    }
    public void SetInfected(bool isInfected)
    {
        if (this.isInfected == isInfected)
            return;

        if (audioScreamList != null &&
            audioScreamList.Length > 0)
            audioManager.PlayVoices(audioScreamList);

        this.isInfected = isInfected;

        spriteRenderer.color = isInfected ? infectedColor : Color.white;

        if (isInfected) notInfectedSet.Remove(this);
        else            notInfectedSet.Add(this);

        animator.Play("HumanSquish", 0, 0f);

        if (isInfected &&
            healTime > 0)
            Invoke("Heal", healTime);

        if (isInfected)
            InfectationCounter.Instance.Add(addScore);
    }

    public void Heal()
    {
        if (!isInfected)
            return;

        SetInfected(false);
    }
}
