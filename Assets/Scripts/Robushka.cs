using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Robushka : MonoBehaviour
{
    public int size;
    // private Vector3 targetPos;
    public float moveSpeed = 5f, rotationSpeed = 5f;
    private Robushka childMatryoshka;
    public Vector3 posOffset = new (0, 0.95f, 0);
    public bool isActive = false;
    private LayerMask defaultMask, platformMask, obstacleMask;
    private bool justRotated = false;
    private float fallSpeed = 25f;
    private Robushka parentMatryoshka;
    private Animator animator;
    private LevelManager levelManager;

    private float selfNeededGridSize;
    private float childNeededGridSize;
    private bool onAction = false;

    private void Awake()
    {
        // targetPos = transform.position;
        defaultMask = LayerMask.GetMask("Default");
        platformMask = LayerMask.GetMask("Platform");
        obstacleMask = LayerMask.GetMask("Obstacles");

        animator = GetComponentInChildren<Animator>();
        if (!isActive)
        {
            animator.SetBool("OpenMouth", true);
        }

        selfNeededGridSize = Mathf.Pow(size, 2);
        childNeededGridSize = Mathf.Pow(size - 1, 2);
    }

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
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
        if (childMatryoshka == null || !isActive || onAction) return;

        float neededSize = childNeededGridSize;

        // Check for the left available platforms
        List<RaycastHit> hits = Physics.SphereCastAll(
            transform.position + transform.forward * ((size - 1) * 2 + 1) + transform.right * 1,
            Mathf.Max(size - 1.5f, 0.5f),
            Vector3.down, 
            size, platformMask
        ).ToList();

        // Check if rails is found
        for (int i = 0; i< hits.Count; i++)
        {
            if (hits[i].collider.CompareTag("Rail"))
            {
                if (hits[i].collider.GetComponent<ThinRail>().size == childMatryoshka.size)
                {
                    ReleaseToRail(hits[i].collider.transform.position, transform.forward);
                    return;
                } else {
                    hits.RemoveAt(i);
                }
            }
        }

        if (hits.Count >= neededSize)
        {
            ReleaseToPlatforms(hits, transform.forward);
            return;
        }

        // Check for the right available platforms
        hits = Physics.SphereCastAll(
            transform.position + transform.forward * ((size - 1) * 2 + 1) + transform.right * -1,
            Mathf.Max(size - 1.5f, 0.5f),
            Vector3.down, 
            size, platformMask
        ).ToList();

        // Check if rails is found
        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].collider.CompareTag("Rail"))
            {
                if (hits[i].collider.GetComponent<ThinRail>().size == childMatryoshka.size)
                {
                    ReleaseToRail(hits[i].collider.transform.position, transform.forward);
                    return;
                } else {
                    hits.RemoveAt(i);
                }
            }
        }

        if (hits.Count >= neededSize)
        {
            ReleaseToPlatforms(hits, transform.forward);
            return;
        }

        Debug.Log("Can't release");
    }

    private void ReleaseToPlatforms(List<RaycastHit> hits, Vector3 lookDir)
    {
        bool canRelease = false;
        if (childMatryoshka.size % 2 == 0)
        {
            Vector3 averagePos = Vector3.zero;
            foreach (RaycastHit hit in hits)
            {
                averagePos += hit.collider.transform.position;
            }
            averagePos /= hits.Count;

            childMatryoshka.MoveTo(averagePos + childMatryoshka.posOffset);
            canRelease = true;
        }
        else
        {
            foreach(var hit in hits)
            {
                Vector3 dest = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                if (Vector3.Distance(dest, transform.position) > size/2 && Vector3.Distance(dest, transform.position) < size * 2)
                {
                    childMatryoshka.MoveTo(hit.collider.transform.position + childMatryoshka.posOffset);
                    canRelease = true;
                    break;
                }
            }
        }

        if (canRelease) {
            DetachChild(lookDir);
        } else {
            Debug.Log("Can't release");
        }
    }

    private void ReleaseToRail(Vector3 pos, Vector3 lookDir) {
        Vector3 targetPos = pos + childMatryoshka.posOffset;
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        childMatryoshka.MoveTo(targetPos);
        DetachChild(lookDir);
    }

    private void DetachChild(Vector3 lookDir) {
        animator.SetBool("OpenMouth", true);

        childMatryoshka.transform.DOScale(Vector3.one * (size - 1), 0.5f);
        isActive = false;
        childMatryoshka.gameObject.SetActive(true);
        childMatryoshka.transform.position = transform.position;
        childMatryoshka.transform.LookAt(lookDir * 3 + transform.position);
        childMatryoshka.isActive = true;
        childMatryoshka = null;
    }

    private void MoveToParent() {
        onAction = true;
        isActive = false;
        parentMatryoshka.animator.SetBool("OpenMouth", false);
        Sequence sequence = DOTween.Sequence();

        transform.DOMove(parentMatryoshka.transform.position, 0.5f).OnComplete(() => {
            parentMatryoshka.childMatryoshka = this;
            parentMatryoshka.isActive = true;
            onAction = false;
            gameObject.SetActive(false);
        });

        sequence.Join(
            transform.DOScale(Vector3.zero, 0.5f)
        );

        sequence.Play();
    }

    private void Die() {
        onAction = true;
        float duration = 100 / fallSpeed;
        transform.DOMoveY(-100, duration).SetEase(Ease.Linear);
    }

    public void MoveTo(Vector3 targetPos) {
        onAction = true;
        float duration = 2 / moveSpeed;
        transform.DOMove(targetPos, duration).OnComplete(() => {
            onAction = false;
        }).SetEase(Ease.OutSine);
    }

    private void OnMeetAnotherRobushka() {
        if (parentMatryoshka.size - size == 1)
        {
            if (parentMatryoshka.size == levelManager.maxSize)
            {
                levelManager.LevelComplete();
            }

            MoveToParent();
        }
    }

    void Update()
    {
        if (onAction) return;

        // Check if the robushka is on a platform
        RaycastHit[] platforms = Physics.SphereCastAll(
            transform.position + Vector3.up * size / 2, Mathf.Max(size - 1, 0.5f), 
            Vector3.down, 
            5, platformMask
        );

        if (platforms.Length == 0)
        {
            isActive = false;
            Die();
            return;
        }

        if (!isActive) { return; }

        Vector2 moveInput = InputManager.playerInput.Player.Move.ReadValue<Vector2>();
        if (moveInput.x != 0 && !justRotated)
        {
            moveInput.x = Mathf.Sign(moveInput.x); // Prevent values that are not one
            justRotated = true;
            onAction = true;
            transform.DORotate(
                transform.rotation.eulerAngles + new Vector3(0, 90 * moveInput.x, 0),
                0.2f
            ).OnComplete(() => {
                onAction = false;
            });
            return;
        } 
        else if (moveInput.x == 0)
        {
            justRotated = false;
        }

        if (moveInput.y != 0)
        {
            moveInput.y = Mathf.Sign(moveInput.y);
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position + transform.forward * 2 * moveInput.y, 
                Mathf.Max(size - 1, 0.5f), Vector3.down, size, defaultMask
            );

            // Check for other robushka
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.CompareTag("Player") && hit.collider.transform != transform)
                    {
                        parentMatryoshka = hit.collider.transform.GetComponent<Robushka>();
                        OnMeetAnotherRobushka();
                        return;
                    }
                }
            }

            hits = Physics.SphereCastAll(
                transform.position + transform.forward * 2 * moveInput.y, 
                Mathf.Max(size - 1, 0.5f), 
                Vector3.down, 
                size, platformMask
            );

            List<RaycastHit> hitList = hits.ToList();

            // Check for railings
            for (int i = 0; i < hitList.Count; i++)
            {
                if (hitList[i].collider.CompareTag("Rail"))
                {
                    if (hitList[i].collider.GetComponent<ThinRail>().size == size)
                    {
                        MoveTo(transform.position + 2 * moveInput.y * transform.forward);
                        return;
                    }
                    else
                    {
                        hitList.RemoveAt(i);
                    }
                }
            }

            if (hitList.Count >= selfNeededGridSize)
            {
                // targetPos = transform.position + 2 * moveInput.y * transform.forward;
                MoveTo(transform.position + 2 * moveInput.y * transform.forward);
                return;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + transform.forward * ((size - 1) * 2 + 1) + transform.right * 1,
            Mathf.Max(size - 1.5f, 0.5f)
        );
        Gizmos.DrawWireSphere(
            transform.position + transform.forward * ((size - 1) * 2 + 1) + transform.right * -1,
            Mathf.Max(size - 1.5f, 0.5f)
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            transform.position + transform.forward * 2,
            Mathf.Max(size - 1, 0.5f)
        );
    }
}
