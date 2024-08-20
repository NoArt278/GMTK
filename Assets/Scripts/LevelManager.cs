using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxSize;
    [SerializeField] private SidePanel levelCompleteUI;
    [SerializeField] private GameObject nextLvlBtn;
    [SerializeField] private TMP_Text lvlClearedText;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

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
        levelCompleteUI.ShowClearUI();
        gameManager.CompleteLevel();
    }

    public void ActivateCutoff(AudioSource source)
    {
        gameManager.ActivateCutoff(source);
    }

    private void ResetLevelWithInput(InputAction.CallbackContext context)
    {
        ResetLevel();
    }

    public void ResetLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
