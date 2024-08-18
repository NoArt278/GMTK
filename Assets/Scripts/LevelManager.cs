using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxSize;
    [SerializeField] private GameObject levelCompleteUI;

    private void OnEnable()
    {
        InputManager.playerInput.Player.Reset.performed += ResetLevelWithInput;
    }

    private void OnDisable()
    {
        InputManager.playerInput.Player.Reset.performed -= ResetLevelWithInput;
    }

    public void LevelComplete()
    {
        levelCompleteUI.SetActive(true);
    }

    private void ResetLevelWithInput(InputAction.CallbackContext context)
    {
        ResetLevel();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
