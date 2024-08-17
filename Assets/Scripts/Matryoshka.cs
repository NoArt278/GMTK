using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Matryoshka : MonoBehaviour
{
    public int size;
    private Vector3 targetPos;
    public float moveSpeed = 5f;
    public List<Transform> neighboringPlatforms;
    public List<Vector3> possiblePos;
    private Matryoshka childMatryoshka;
    public Vector3 posOffset = new (0, 0.95f, 0);
    private bool justCalculated = false;
    private const float offsetCheck = 0.3f;
    public bool isActive = false;

    private void Awake()
    {
        neighboringPlatforms = new List<Transform>();
        possiblePos = new List<Vector3>();
        targetPos = transform.position;
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
        if (moveInput != Vector2.zero)
        {
            LayerMask platformMask = LayerMask.GetMask("Platform");
            Vector3 moveDir = Vector3.zero;
            if (moveInput.x != 0)
            {
                moveDir.x = moveInput.x;
            } else if (moveInput.y != 0)
            {
                moveDir.z = moveInput.y;
            }
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + moveDir * 2, Mathf.Max(size-1, 0.5f), Vector3.down, size, platformMask);
            if (hits.Length >= Mathf.Pow(size, 2))
            {
                targetPos = transform.position + moveDir * 2;
            }
        }
    }
}
