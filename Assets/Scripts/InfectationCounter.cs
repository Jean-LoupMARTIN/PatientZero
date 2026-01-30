using TMPro;
using UnityEngine;

public class InfectationCounter : Singleton<InfectationCounter>
{
    [SerializeField] TMP_Text tmpText;
    [SerializeField] GameObject icon;
    [SerializeField] float addFont = 1;
    [SerializeField] float speed = 1;
    float targetFontSize;
    int count = 0;

    public int Count => count;

    protected override void Awake()
    {
        base.Awake();
        targetFontSize = tmpText.fontSize;
    }

    void FixedUpdate()
    {
        tmpText.fontSize = Mathf.Lerp(tmpText.fontSize, targetFontSize, speed * Time.fixedDeltaTime);
    }

    public void Add(int add)
    {
        icon.SetActive(true);
        count += add;
        tmpText.text = count.ToString();
        tmpText.fontSize += addFont * add;
    }
}
