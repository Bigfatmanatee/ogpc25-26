using UnityEngine;

public class SlamParticle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SpawnParticles()
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Emit(40); // low particles
        transform.GetChild(1).GetComponent<ParticleSystem>().Emit(50); // high particles
    }
}
