using TMPro;
using UnityEngine;

public class GameOverMenu : Singleton<GameOverMenu>
{
    [SerializeField] TMP_Text timeScoreText;
    [SerializeField] TMP_Text infScoreText;

    AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindAnyObjectByType<AudioManager>();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        audioManager.PlayGameOver();
        Time.timeScale = 0;

        if (Timer.Instance)
        {
            int min = Mathf.FloorToInt(Timer.Instance.TimeScore / 60f);
            int sec = Mathf.FloorToInt(Timer.Instance.TimeScore % 60f);
            timeScoreText.text = $"You survived {min:D2} : {sec:D2}";
        }

        if (InfectationCounter.Instance)
        {
            infScoreText.text = $"Score : {InfectationCounter.Instance.Count}";
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }
}
