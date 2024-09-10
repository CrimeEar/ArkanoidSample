using DG.Tweening;
using UnityEngine;

public class ScaleOnEnable : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.OutBack;
    [SerializeField] private float delay = 0f;

    private Tween scaleTween;
    private void OnDestroy()
    {
        if (scaleTween.IsActive())
        {
            scaleTween.Kill();
        }
    }

    private void OnEnable()
    {
        if (scaleTween.IsActive())
        {
            scaleTween.Kill();
        }

        transform.localScale = Vector3.zero;

        transform.DOScale(Vector3.one, duration).SetEase(ease).SetDelay(delay);
    }
}
