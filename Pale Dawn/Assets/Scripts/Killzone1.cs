using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone1 : MonoBehaviour
{
    string currentSceneName;
    void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
