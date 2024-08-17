using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class Matryoshka : MonoBehaviour
{
    public int size;
    private Vector3 targetPos;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    private Matryoshka childMatryoshka;
    public Vector3 posOffset = new (0, 0.95f, 0);
    public bool isActive = false;
    private LayerMask defaultMask;
    private LayerMask platformMask;

    private void Awake()
    {
        targetPos = transform.position;
        defaultMask = LayerMask.GetMask("Default");
        platformMask = LayerMask.GetMask("Platform");
    }

    private void OnEnable()
    {
        InputManager.playerInput.Player.Release.performed += ReleaseChildMatryoshka;
    }

    private void OnDisable()
    {
        InputManager.playerInput.Player.Release.performed -= ReleaseChildMatryoshka;
    }

    private void ReleaseChildMatryoshka(InputAction.CallbackContext context)
    {
        if (childMatryoshka != null)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward * size * 2, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
            if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in front
            {
                GetReleasePos(hits);
            }
            else
            {
                hits = Physics.SphereCastAll(transform.position + transform.right * size * 2, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in right side
                {
                    GetReleasePos(hits);
                }
                else
                {
                    hits = Physics.SphereCastAll(transform.position + transform.right * -size * 2, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                    if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in left side
                    {
                        GetReleasePos(hits);
                    }
                    else
                    {
                        hits = Physics.SphereCastAll(transform.position + transform.forward * -size * 2, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                        if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in back side
                        {
                            GetReleasePos(hits);
                        } else
                        {
                            Debug.Log("Can't release");
                        }
                    }
                }
            }
        }
    }

    private void GetReleasePos(RaycastHit[] hits)
    {
        childMatryoshka.gameObject.SetActive(true);
        childMatryoshka.transform.position = transform.position;
        isActive = false;
        if (childMatryoshka.size % 2 == 0)
        {
            Vector3 averagePos = Vector3.zero;
            foreach (RaycastHit hit in hits)
            {
                averagePos += hit.collider.transform.position;
            }
            averagePos /= hits.Length;
            if (Mathf.RoundToInt(averagePos.x) % 2 == 0)
            {
                RaycastHit[] hits2 = Physics.SphereCastAll(averagePos + Vector3.right, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                if (hits2.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in left side
                {
                    averagePos.x = Mathf.RoundToInt(averagePos.x) + 1;
                } else
                {
                    averagePos.x = Mathf.RoundToInt(averagePos.x) - 1;
                }
            }
            if (Mathf.RoundToInt(averagePos.z) % 2 == 0)
            {
                RaycastHit[] hits2 = Physics.SphereCastAll(averagePos + Vector3.back, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                if (hits2.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in left side
                {
                    averagePos.z = Mathf.RoundToInt(averagePos.z) + 1;
                }
                else
                {
                    averagePos.z = Mathf.RoundToInt(averagePos.z) - 1;
                }
            }
            childMatryoshka.targetPos = averagePos + childMatryoshka.posOffset;
        }
        else
        {
            childMatryoshka.targetPos = hits[0].collider.transform.position + childMatryoshka.posOffset;
        }
        childMatryoshka.isActive = true;
        childMatryoshka = null;
    }

    void Update()
    {
        if (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPos);
            return;
        }
        if (!isActive)
        {
            return;
        }
        Vector2 moveInput = InputManager.playerInput.Player.Move.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            Vector3 moveDir = Vector3.zero;
            if (moveInput.x != 0)
            {
                moveDir.x = moveInput.x;
            } else if (moveInput.y != 0)
            {
                moveDir.z = moveInput.y;
            }
            bool hitPlayer = false;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + moveDir * 2, Mathf.Max(size - 1, 0.5f), Vector3.down, size, defaultMask);
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Player") && hit.collider.transform != transform)
                    {
                        hitPlayer = true;
                        Matryoshka matryoshka = hit.collider.transform.GetComponent<Matryoshka>();
                        if (matryoshka.size - size == 1)
                        {
                            matryoshka.childMatryoshka = this;
                            matryoshka.isActive = true;
                            targetPos = matryoshka.transform.position;
                            isActive = false;
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            } 
            if (!hitPlayer)
            {
                hits = Physics.SphereCastAll(transform.position + moveDir * 2, Mathf.Max(size - 1, 0.5f), Vector3.down, size, platformMask);
                if (hits.Length >= Mathf.Pow(size, 2))
                {
                    targetPos = transform.position + moveDir * 2;
                }
            }
        }
    }
}
