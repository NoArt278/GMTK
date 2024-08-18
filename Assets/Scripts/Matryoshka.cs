using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Matryoshka : MonoBehaviour
{
    public int size;
    private Vector3 targetPos;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    private Matryoshka childMatryoshka;
    public Vector3 posOffset = new (0, 0.95f, 0);
    public bool isActive = false;
    private LayerMask defaultMask, platformMask, obstacleMask;
    private bool isEnteringBigger = false, justRotated = false;
    private Matryoshka parentMatryoshka;
    private List<Vector3> targetPosBuffer = new List<Vector3>();
    private Animator animator;
    private Coroutine scaleCoroutine;

    private void Awake()
    {
        targetPos = transform.position;
        defaultMask = LayerMask.GetMask("Default");
        platformMask = LayerMask.GetMask("Platform");
        obstacleMask = LayerMask.GetMask("Obstacles");

        animator = GetComponentInChildren<Animator>();
        if (!isActive)
        {
            animator.SetBool("OpenMouth", true);
        }
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
        if (childMatryoshka != null && isActive)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward * size, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
            if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in front
            {
                GetReleasePos(hits);
            }
            else
            {
                hits = Physics.SphereCastAll(transform.position + transform.right * size, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in right side
                {
                    GetReleasePos(hits);
                }
                else
                {
                    hits = Physics.SphereCastAll(transform.position + transform.right * -size, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
                    if (hits.Length >= Mathf.Pow(childMatryoshka.size, 2)) // Search for platform in left side
                    {
                        GetReleasePos(hits);
                    }
                    else
                    {
                        hits = Physics.SphereCastAll(transform.position + transform.forward * -size, Mathf.Max(childMatryoshka.size - 1, 0.5f), Vector3.down, size, platformMask);
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
        animator.SetBool("OpenMouth", true);
        childMatryoshka.gameObject.SetActive(true);
        childMatryoshka.transform.position = transform.position;
        if (childMatryoshka.scaleCoroutine != null)
        {
            StopCoroutine(childMatryoshka.scaleCoroutine);
        }
        childMatryoshka.scaleCoroutine = StartCoroutine(childMatryoshka.ScaleSelf(false));
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

    private IEnumerator ScaleSelf (bool toSmall)
    {
        if (toSmall)
        {
            while (transform.localScale.x > 0)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, rotationSpeed * size * Time.deltaTime);
                yield return null;
            }
        } else
        {
            while (transform.localScale.x < size + 0.38f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(size+0.38f, size + 0.38f, size + 0.38f), rotationSpeed * size * Time.deltaTime);
                yield return null;
            }
        }
        scaleCoroutine = null;
    }

    void Update()
    {
        if (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            return;
        }
        if (isEnteringBigger)
        {
            gameObject.SetActive(false);
            parentMatryoshka.isActive = true;
            isEnteringBigger = false;
            parentMatryoshka = null;
        }
        if (!isActive)
        {
            return;
        }
        if (targetPosBuffer.Count > 0)
        {
            targetPos = targetPosBuffer[0];
            targetPosBuffer.RemoveAt(0);
        }
        Vector2 moveInput = InputManager.playerInput.Player.Move.ReadValue<Vector2>();
        if (moveInput.x != 0 && !justRotated)
        {
            justRotated = true;
            transform.Rotate(new Vector3(0, 90 * Mathf.Sign(moveInput.x), 0));
        } else if (moveInput.x == 0)
        {
            justRotated = false;
        }
        if (moveInput.y != 0)
        {
            moveInput.y = Mathf.Sign(moveInput.y);
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward * 2 * moveInput.y, Mathf.Max(size - 1, 0.5f), Vector3.down, size, defaultMask);
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Player") && hit.collider.transform != transform)
                    {
                        parentMatryoshka = hit.collider.transform.GetComponent<Matryoshka>();
                        if (parentMatryoshka.size - size == 1)
                        {
                            if (scaleCoroutine != null)
                            {
                                StopCoroutine(scaleCoroutine);
                            }
                            scaleCoroutine = StartCoroutine(ScaleSelf(true));
                            parentMatryoshka.animator.SetBool("OpenMouth", false);
                            parentMatryoshka.childMatryoshka = this;
                            targetPos = parentMatryoshka.transform.position;
                            targetPosBuffer.Clear();
                            isActive = false;
                            isEnteringBigger = true;
                        }
                        return;
                    }
                }
            }
            hits = Physics.SphereCastAll(transform.position + transform.forward * 2 * moveInput.y, Mathf.Max(size - 1, 0.5f), Vector3.down, size, platformMask);
            List<RaycastHit> hitsList = hits.ToList<RaycastHit>();
            List<RaycastHit> removeList = new List<RaycastHit>();
            foreach (var hit in hitsList)
            {
                if (hit.collider.CompareTag("Rail"))
                {
                    if (hit.collider.GetComponent<ThinRail>().size == size)
                    {
                        targetPosBuffer.Add(transform.position + transform.forward * 2 * moveInput.y);
                        return;
                    }
                    else
                    {
                        removeList.Add(hit);
                    }
                }
            }
            foreach (var hit in removeList)
            {
                hitsList.Remove(hit);
            }
            if (hitsList.Count >= Mathf.Pow(size, 2))
            {
                targetPos = transform.position + transform.forward * 2 * moveInput.y;
            }
        }
    }
}
