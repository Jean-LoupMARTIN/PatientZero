using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelsManager : Singleton<LevelsManager>
{
    public Vector2 spawnRect = new Vector2(16, 9);
    [SerializeField, Range(0, 180)] float spawnRandRotAngle = 90;
    [SerializeField] Level[] levels;
    int crtLevelIdx = 0;
    Level crtLevel;
    [SerializeField] GameObject breakingNews;
    [SerializeField] TMP_Text breakingNewsTextPrefab;
    [SerializeField] Transform breakingNewsTextSpawn;
    [SerializeField] float dtSpawn = 0.3f;
    [SerializeField] GameObject[] humanPrefabs;
    [SerializeField] GameObject policeManPrefab;
    [SerializeField] GameObject helicoPrefab;

    public Level CrtLevel => crtLevel;

    AudioManager audioManager;

    void OnDrawGizmosSelected()
    {
        DrawSpawnRect();
    }

    void DrawSpawnRect()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnRect);
    }

    protected override void Awake()
    {
        base.Awake();
        crtLevel = levels[crtLevelIdx];
    }

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        StartCoroutine(MatchEffectif());
    }

    void Update()
    {
        if (crtLevelIdx+1 < levels.Length &&
            levels[crtLevelIdx+1].time <= Timer.Instance.TimeScore)
        {
            crtLevelIdx++;
            crtLevel = levels[crtLevelIdx];

            audioManager.PlayMusicProgress();

            if (crtLevel.flashInfo != "")
            {
                if (!breakingNews.activeSelf)
                    breakingNews.SetActive(true);
                    
                Instantiate(breakingNewsTextPrefab, breakingNewsTextSpawn.position, Quaternion.identity, breakingNewsTextSpawn).text = crtLevel.flashInfo;
            }

            Debug.Log("Next Level");
        }
    }

    IEnumerator MatchEffectif()
    {
        while (true)
        {
            yield return new WaitUntil(() => NeedSpawn() != null);
            Spawn(NeedSpawn());
            yield return new WaitForSeconds(dtSpawn);
        }
    }

    GameObject NeedSpawn()
    {
        if (HumanMovement.set.Count < crtLevel.nbHumans)
            return humanPrefabs.PickRandom();

        if (PoliceManMovement.set.Count < crtLevel.nbPoliceMen)
            return policeManPrefab;

        if (Helico.set.Count < crtLevel.nbHelicos)
            return helicoPrefab;

        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        float perimeter = (spawnRect.x + spawnRect.y) * 2;
        float perimeterTravel = Random.value * perimeter;

        Vector3 position = transform.position - (Vector3)spawnRect /2;
        
        if (perimeterTravel <= spawnRect.y)
            return position + Vector3.up * perimeterTravel;

        position += Vector3.up * spawnRect.y;
        perimeterTravel -= spawnRect.y;

        if (perimeterTravel <= spawnRect.x)
            return position + Vector3.right * perimeterTravel;

        position += Vector3.right * spawnRect.x;
        perimeterTravel -= spawnRect.x;

        if (perimeterTravel <= spawnRect.y)
            return position + Vector3.down * perimeterTravel;

        position += Vector3.down * spawnRect.y;
        perimeterTravel -= spawnRect.y;

        return position + Vector3.left * perimeterTravel;
    }

    void Spawn(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, GetRandomSpawnPosition(), Quaternion.identity);

        Vector2 dir = transform.position - instance.transform.position;

        if (instance.GetComponent<HumanMovement>())
            dir = Quaternion.Euler(0f, 0f, Random.Range(-spawnRandRotAngle/2f, spawnRandRotAngle/2f)) * dir;

        instance.transform.Find("Mask", true)?.gameObject.SetActive(crtLevel.maskMandatory);

        if (instance.TryGetComponent(out HumanMovement hm))
            hm.MoveDir = dir;

        else if (instance.TryGetComponent(out PoliceManMovement pmm))
            pmm.MoveDir = dir;

        else if (instance.TryGetComponent(out Helico h))
            h.MoveDir = dir;
    }
}

[Serializable]
public class Level
{
    public float time;
    public int nbHumans;
    public int nbPoliceMen;
    public int nbHelicos;
    public bool maskMandatory;
    public bool totalLockdown;
    public string flashInfo;
}