using UnityEngine;

public class ChangeMap : MonoBehaviour
{
    [SerializeField] GameObject DefaultMap;
    [SerializeField] GameObject TempMap;

    private void switchMap()
    {
        TempMap.SetActive(true);
        DefaultMap.SetActive(false);
    }
    private void resetMap()
    {
        DefaultMap.SetActive(true);
        TempMap.SetActive(false);
    }

    public void bossStart()
    {
        switchMap();
    }
    public void bossKilled()
    {
        resetMap();
    }
}
