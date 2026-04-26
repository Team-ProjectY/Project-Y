using UnityEngine;

public class CameraADSController : MonoBehaviour, IADSController
{
    [SerializeField] private Camera _camera;

    private float _defaultFov;
    private float _targetFov;

    void Start()
    {
        _defaultFov = _camera.fieldOfView;
        _targetFov = _defaultFov;
    }

    void Update()
    {
        _camera.fieldOfView = Mathf.Lerp(
            _camera.fieldOfView,
            _targetFov,
            Time.deltaTime * 10f
        );
    }

    public void StartADS(WeaponSOData data)
    {
        _targetFov = data.HasScope ? data.ScopeFov : data.AdsFov;
    }

    public void EndADS()
    {
        _targetFov = _defaultFov;
    }
}