using UnityEngine;

public class CameraHeightController : MonoBehaviour
{
    [SerializeField] private float _standY = 0.5f;
    [SerializeField] private float _crouchY = 0f;
    [SerializeField] private float _proneY = 0.5f;
    [SerializeField] private float _speed = 10f;

    [SerializeField] private MonoBehaviour _postureProviderComponent;
    private IPostureProvider _postureProvider;

    void Awake()
    {
        _postureProvider = _postureProviderComponent as IPostureProvider;
    }

    void Update()
    {
        float targetY;
        if (_postureProvider.Posture == PostureState.Crouching)
            targetY = _crouchY;
        else if (_postureProvider.Posture == PostureState.Proning)
            targetY = _proneY;
        else
            targetY = _standY;

        Vector3 pos = transform.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * _speed);
        transform.localPosition = pos;
    }
}
