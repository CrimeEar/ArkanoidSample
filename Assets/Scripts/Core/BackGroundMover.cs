using DG.Tweening;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    private const float MOVE_DURATION = 3f;

    private Vector3[] _positions;
    private Transform _backgroundTransform;

    private Tween _moveTween;

    public void Init(Vector3[] positions, Transform _background)
    {
        _positions = positions;
        _backgroundTransform = _background;
    }
    private void OnDestroy()
    {
        TryKillTween();
    }
    public void OnChangeState(int stateIndex)
    {
        TryKillTween();

        _moveTween = _backgroundTransform.transform.DOMove(_positions[stateIndex], MOVE_DURATION);
    }

    private void TryKillTween()
    {
        if (_moveTween.IsActive())
        {
            _moveTween.Kill();
        }
    }
}
