using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
[SerializeField] Slider slider;
[SerializeField] Image fill;
[SerializeField] Image border;
[SerializeField] GameObject text;
[SerializeField] BossScript boss;
    void Start()
    {
        show(false);
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = boss.getHealthPercent();
        //color change? smooth health drop?
    }
    private void show(bool a)
    {
        fill.enabled = a;
        border.enabled = a;
        text.SetActive(a);
    }

    public void bossStart()
    {
        show(true);
    }
    public void bossKilled()
    {
        show(false);
    }
}
