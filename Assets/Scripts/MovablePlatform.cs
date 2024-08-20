using DG.Tweening;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public bool goDown;
    public float downHeight, upHeight;
    private float moveSpeed = 50f;
    Tween moveTween;

    private void OnDisable()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }

    public void TriggerMove()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
            moveTween = null;
        }
        if (goDown)
        {
            moveTween = transform.DOMove(new Vector3(transform.position.x, downHeight, transform.position.z), 0.8f).SetEase(Ease.InOutQuart).
                OnComplete(() =>
                {
                    moveTween = null;
                });
        } else
        {
            moveTween = transform.DOMove(new Vector3(transform.position.x, upHeight, transform.position.z), 0.8f).SetEase(Ease.InOutQuart).
                OnComplete(() =>
                {
                    moveTween = null;
                });
        }
        goDown = !goDown;
    }
}
