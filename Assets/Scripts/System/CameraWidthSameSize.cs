using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraWidthSameSize : MonoBehaviour
{
    private const float SIZE_DEVIATION = 0.01f;

    public Vector2 _referenceScreenResolution = new Vector2(1920f, 1080f);
    public bool _isNeddChangeOnUpdate = true;

    private float _referenceAspectRatio;
    private float _startOrthographicSize;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _startOrthographicSize = _camera.orthographicSize;

        _referenceAspectRatio = _referenceScreenResolution.x / _referenceScreenResolution.y;

        AdjustCameraSizeAndPosition();
    }

    void AdjustCameraSizeAndPosition()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        if (Mathf.Abs(currentAspectRatio - _referenceAspectRatio) > SIZE_DEVIATION)
        {
            _camera.orthographicSize = _startOrthographicSize * (_referenceAspectRatio / currentAspectRatio);
        }

        if (Mathf.Abs(_startOrthographicSize - _camera.orthographicSize) > SIZE_DEVIATION)
        {
            _camera.orthographicSize = _startOrthographicSize * (_referenceAspectRatio / currentAspectRatio);
        }

        float newYPosition = _camera.orthographicSize - _startOrthographicSize;
        _camera.transform.position = new Vector3(_camera.transform.position.x, -newYPosition, _camera.transform.position.z);
    }

    void Update()
    {
        if (!_isNeddChangeOnUpdate)
        {
            return;
        }

        AdjustCameraSizeAndPosition();
    }
}
