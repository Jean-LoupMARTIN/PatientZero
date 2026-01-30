using UnityEngine;

public class BreakingNews : MonoBehaviour
{
    [SerializeField] float life = 20;
    [SerializeField] float speed = 1;

    void Start()
    {
        Invoke("Destroy", life);
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
