using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.playerInput.Player.Reset.performed += ResetLevel;
        InputManager.playerInput.Enable();
    }

    private void OnDisable()
    {
        InputManager.playerInput.Player.Reset.performed -= ResetLevel;
        InputManager.playerInput.Enable();
    }

    private void ResetLevel(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
