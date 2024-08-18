using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int maxLevel = 10;
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        if (instance == this)
            InputManager.playerInput.Enable();
    }

    private void OnDisable()
    {
        if (instance == this) 
            InputManager.playerInput.Disable();
    }
}
