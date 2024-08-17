using System.Collections;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public bool goDown;
    private float moveSpeed = 50f;
    Coroutine moveCoroutine;
    
    public void TriggerMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MovePlatform());
        goDown = !goDown;
    }

    IEnumerator MovePlatform()
    {
        if (goDown)
        {
            while (transform.position.y > -50)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -50, transform.position.z), moveSpeed * Time.deltaTime);
                yield return null;
            }
        } else
        {
            while (transform.position.y < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        moveCoroutine = null;
    }
}
