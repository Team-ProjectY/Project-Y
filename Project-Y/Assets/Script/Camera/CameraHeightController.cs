using UnityEngine;

public class CameraHeightController : MonoBehaviour
{
    [SerializeField] private float _standY = 0.5f;
    [SerializeField] private float _crouchY = 0f;
    [SerializeField] private float _speed = 10f;

    [SerializeField] MonoBehaviour postureProviderComponent;
    IPostureProvider _postureProvider;

    void Awake()
    {
        _postureProvider = postureProviderComponent as IPostureProvider;
    }

    void Update()
    {
        float targetY;
        if (_postureProvider.Posture == PostureState.Crouching)
            targetY = _crouchY;
        else
            targetY = _standY;

        Vector3 pos = transform.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * _speed);
        transform.localPosition = pos;
    }
}