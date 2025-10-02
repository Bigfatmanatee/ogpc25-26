using UnityEngine;

public class HitboxPass : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject Host;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject passHost()
    {
        return Host;
    }
}
