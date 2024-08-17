using System.Collections.Generic;
using UnityEngine;

public class Matryoshka : MonoBehaviour
{
    public int size;
    private Vector3 targetPos;
    [SerializeField] private Transform startPlatform;
    public float moveSpeed = 5f;
    public List<Platform> neighboringPlatforms;
    private Matryoshka childMatryoshka;
    public Vector3 posOffset = new (0, 0.95f, 0);
    public bool isActive = false;

    private void Awake()
    {
        neighboringPlatforms = new List<Platform>();
        targetPos = startPlatform.position + posOffset;
        transform.position = targetPos;
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        if (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            return;
        }
        Vector2 moveInput = InputManager.playerInput.Player.Move.ReadValue<Vector2>();
        if (moveInput.x < 0) // Move left
        {
            neighboringPlatforms.ForEach(platform =>
                {
                    if (platform.transform.position.x < transform.position.x && platform.transform.position.z == transform.position.z)
                    {
                        targetPos = platform.transform.position + posOffset;
                    }
                }
            );
        }
        else if (moveInput.x > 0) // Move right
        {
            neighboringPlatforms.ForEach(platform =>
                {
                    if (platform.transform.position.x > transform.position.x && platform.transform.position.z == transform.position.z)
                    {
                        targetPos = platform.transform.position + posOffset;
                    }
                }
            );
        }
        else if (moveInput.y > 0) // Move up
        {
            neighboringPlatforms.ForEach(platform =>
            {
                if (platform.transform.position.z > transform.position.z && platform.transform.position.x == transform.position.x)
                {
                    targetPos = platform.transform.position + posOffset;
                }
            }
            );
        }
        else if (moveInput.y < 0) // Move down
        {
            neighboringPlatforms.ForEach(platform =>
                {
                    if (platform.transform.position.z < transform.position.z && platform.transform.position.x == transform.position.x)
                    {
                        targetPos = platform.transform.position + posOffset;
                    }
                }
            );
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Platform")
        {
            Platform otherPlatform = other.GetComponent<Platform>();
            if (!neighboringPlatforms.Contains(otherPlatform) && otherPlatform.size == size)
            {
                neighboringPlatforms.Add(other.GetComponent<Platform>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform" && neighboringPlatforms.Contains(other.GetComponent<Platform>()))
        {
            Platform otherPlatform = other.GetComponent<Platform>();
            if (neighboringPlatforms.Contains(otherPlatform))
            {
                neighboringPlatforms.Remove(other.GetComponent<Platform>());
            }
        }
    }
}
