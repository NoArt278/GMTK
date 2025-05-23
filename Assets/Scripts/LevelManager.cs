using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxSize;
    [SerializeField] private SidePanel levelCompleteUI;
    private GameManager gameManager;
    public static string[] levelTitles = {
        "Rock Bottom", 
        "Mom Meets Dad", 
        "No Pressure", 
        "Get Me Out",
        "One Step Forward",
        "Alien Crossing",
        "Easy One",
        "Rounded Corner",
        "Higher Order",
        "3 Body Problem"
    };

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
        gameManager.ChangeBGM(true);
        SceneManager.LoadScene(0);
    }
}
