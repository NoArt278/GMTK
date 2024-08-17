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
        //if (moveInput.x < 0) // Move left
        //{
        //    possiblePos.ForEach(pos =>
        //        {
        //            if (pos.x < transform.position.x && Mathf.Abs(pos.z - transform.position.z) < offsetCheck)
        //            {
        //                targetPos = pos + posOffset;
        //                justCalculated = false;
        //            }
        //        }
        //    );
        //}
        //else if (moveInput.x > 0) // Move right
        //{
        //    possiblePos.ForEach(pos =>
        //        {
        //            if (pos.x > transform.position.x && Mathf.Abs(pos.z - transform.position.z) < offsetCheck)
        //            {
        //                targetPos = pos + posOffset;
        //                justCalculated = false;
        //            }
        //        }
        //    );
        //}
        //else if (moveInput.y > 0) // Move up
        //{
        //    possiblePos.ForEach(pos =>
        //        {
        //            if (pos.z > transform.position.z && Mathf.Abs(pos.x - transform.position.x) < offsetCheck)
        //            {
        //                targetPos = pos + posOffset;
        //                justCalculated = false;
        //            }
        //        }
        //    );
        //}
        //else if (moveInput.y < 0) // Move down
        //{
        //    possiblePos.ForEach(pos =>
        //        {
        //            if (pos.z < transform.position.z && Mathf.Abs(pos.x - transform.position.x) < offsetCheck)
        //            {
        //                targetPos = pos + posOffset;
        //                justCalculated = false;
        //            }
        //        }
        //    );
        //}
    }

    //private void CalculatePossiblePositions()
    //{
    //    if (justCalculated)
    //    {
    //        return;
    //    }
    //    possiblePos.Clear();
    //    List<Transform> currPlatformGroup = new List<Transform>();
    //    foreach (Transform platform in neighboringPlatforms)
    //    {
    //        if (size == 1)
    //        {
    //            possiblePos.Add(platform.position);
    //            continue;
    //        }
    //        currPlatformGroup.Clear();
    //        currPlatformGroup.Add(platform);
    //        foreach (Transform otherPlatform in neighboringPlatforms)
    //        {
    //            if (Vector3.Distance(otherPlatform.position, platform.position) <= Mathf.Sqrt(Mathf.Pow(Mathf.Pow(2,size-1), 2) * 2) && otherPlatform != platform)
    //            {
    //                currPlatformGroup.Add(otherPlatform);
    //            }
    //            if (currPlatformGroup.Count == Mathf.Pow(size, 2))
    //            {
    //                Vector3 avgPos = Vector3.zero;
    //                currPlatformGroup.ForEach(currPlat =>
    //                    {
    //                        avgPos += currPlat.position;
    //                    }
    //                );
    //                avgPos /= currPlatformGroup.Count;
    //                if (!possiblePos.Contains(avgPos) && Mathf.Abs(Vector3.Distance(avgPos + posOffset, transform.position) - 2) < offsetCheck)
    //                {
    //                    possiblePos.Add(avgPos);
    //                }
    //                currPlatformGroup.Clear();
    //                currPlatformGroup.Add(platform);
    //            }
    //        }
    //    }
    //    justCalculated = true;
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Platform"))
    //    {
    //        if (!neighboringPlatforms.Contains(other.transform))
    //        {
    //            neighboringPlatforms.Add(other.transform);
                
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Platform"))
    //    {
    //        if (neighboringPlatforms.Contains(other.transform))
    //        {
    //            neighboringPlatforms.Remove(other.transform);
    //        }
    //    }
    //}
}
