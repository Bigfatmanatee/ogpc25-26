using System.Collections;
using UnityEngine;

public class EnemyDeathParticles : MonoBehaviour
{
    public void PlayParticles(int count)
    {
        transform.parent = null;
        transform.GetComponent<ParticleSystem>().Emit(count);

        StartCoroutine(Kill(2));
    }

    private IEnumerator Kill(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
