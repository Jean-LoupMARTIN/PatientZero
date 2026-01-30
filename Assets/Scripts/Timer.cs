using TMPro;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    [SerializeField] TMP_Text tmpText;
    bool started = false;
    float time = 0;

    public bool Started => started;
    public float TimeScore => time;

    public void StartTimer()
    {
        if (started)
        {
            Debug.LogError("Timer.StartTimer : Timer already started...");
            return;
        }

        started = true;    
    }

    void Update()
    {
        if (started)
            AddTime();
    }

    void AddTime()
    {
        time += Time.deltaTime;
        TextMatchTime();
    }

    void TextMatchTime()
    {
        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);
        tmpText.text = $"{min:D2} : {sec:D2}";
    }
}
