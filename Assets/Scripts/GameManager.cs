using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.playerInput.Enable();
    }

    private void OnDisable()
    {
        InputManager.playerInput.Enable();
    }
}
