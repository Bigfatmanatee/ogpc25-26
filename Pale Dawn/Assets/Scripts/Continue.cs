using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    private InputAction cont;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cont = new InputAction("continue");
        cont.AddBinding("<Gamepad>/buttonSouth");
        cont.AddBinding("<Keyboard>/space");
        cont.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (cont.ReadValue<float>() > 0.2f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
