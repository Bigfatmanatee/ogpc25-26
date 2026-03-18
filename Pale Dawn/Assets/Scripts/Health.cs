// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] Image sprite;
    [SerializeField] ParticleSystem fire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //comment 2
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Hide()
    {
        sprite.enabled = false;
        fire.Stop();
        fire.Clear();
    }
    public void Show()
    {
        sprite.enabled = true;
    }
    public void FireOff()
    {
        fire.Stop();
    }
    public void FireOn()
    {
        fire.Play();
    }
}
