using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] GameObject boss;
    void Start()
    {
        boss.SetActive(false);
    }

    public void bossStart()
    {
        boss.SetActive(true);
    }
}
