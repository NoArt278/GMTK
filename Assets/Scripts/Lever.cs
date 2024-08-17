using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour
{
    public List<MovablePlatform> movablePlatforms = new List<MovablePlatform>();
    private bool isPlayerInRange = false;

    private void OnEnable()
    {
        InputManager.playerInput.Player.Interact.performed += MoveConnectedPlatforms;
    }

    private void OnDisable()
    {
        InputManager.playerInput.Player.Interact.performed -= MoveConnectedPlatforms;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.GetComponent<Matryoshka>().isActive)
            {
                isPlayerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void MoveConnectedPlatforms(InputAction.CallbackContext callbackContext)
    {
        if (isPlayerInRange)
        {
            foreach (var platform in movablePlatforms)
            {
                platform.TriggerMove();
            }
        }
    }
}
