using UnityEngine;
using UnityEngine.SceneManagement;

public class door : interactable
{
    [SerializeField] int levelLoad;
    void FixedUpdate()
    {
        if (target != null && yMove > 0.5)
        {
            SceneManager.LoadScene(levelLoad);
        }
    }
}
