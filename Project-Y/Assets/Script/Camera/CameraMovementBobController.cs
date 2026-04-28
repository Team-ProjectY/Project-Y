using UnityEngine;

/// <summary>
/// 플레이어 이동 속도 기반 카메라 bob(흔들림) 오프셋을 적용합니다.
/// 기존 카메라 위치 제어(예: 자세별 높이 변경)와 충돌하지 않도록
/// "현재 위치 - 이전 프레임 오프셋 + 이번 프레임 오프셋" 방식으로 동작합니다.
/// </summary>
public class CameraMovementBobController : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("플레이어 자세 상태(IPostureProvider)를 제공하는 컴포넌트")]
    [SerializeField] private MonoBehaviour _postureProviderComponent;
    [Tooltip("지면 체크(IGroundChecker)를 제공하는 컴포넌트")]
    [SerializeField] private MonoBehaviour _groundCheckerComponent;

    [Header("Movement Condition")]
    [Tooltip("켜면 지면에 있을 때만 bob을 적용합니다")]
    [SerializeField] private bool _requireGrounded = true;
    [Tooltip("이 속도보다 느리면 bob을 멈춥니다")]
    [SerializeField] private float _moveThreshold = 0.1f;
    [Tooltip("걷기 기준 속도 (이 값 근처에서 walk 설정이 적용)")]
    [SerializeField] private float _walkReferenceSpeed = 4f;
    [Tooltip("달리기 기준 속도 (이 값 근처에서 run 설정이 적용)")]
    [SerializeField] private float _runReferenceSpeed = 7f;

    [Header("Bob Shape")]
    [Tooltip("걷기 상태 bob 주파수")]
    [SerializeField] private float _walkFrequency = 9f;
    [Tooltip("달리기 상태 bob 주파수")]
    [SerializeField] private float _runFrequency = 13f;
    [Tooltip("걷기 bob 진폭 (X:좌우, Y:상하, Z:전후)")]
    [SerializeField] private Vector3 _walkAmplitude = new Vector3(0.012f, 0.02f, 0.008f);
    [Tooltip("달리기 bob 진폭 (X:좌우, Y:상하, Z:전후)")]
    [SerializeField] private Vector3 _runAmplitude = new Vector3(0.02f, 0.035f, 0.014f);
    [Tooltip("전체 bob 강도 배율 (1 = 기본)")]
    [SerializeField] private float _intensity = 1f;
    [Tooltip("정지/차단 시 bob 오프셋이 0으로 돌아오는 속도")]
    [SerializeField] private float _recoverSpeed = 10f;
    [Tooltip("켜면 좌우 위치(X) bob을 줄이고 회전(yaw) 위주로 연출합니다")]
    [SerializeField] private bool _disableLateralPositionOffset = true;

    [Header("Rotation Sway")]
    [Tooltip("걷기 상태 최대 yaw 흔들림 각도(도)")]
    [SerializeField] private float _walkYawAngle = 0.8f;
    [Tooltip("달리기 상태 최대 yaw 흔들림 각도(도)")]
    [SerializeField] private float _runYawAngle = 1.6f;
    [Tooltip("걷기 상태 최대 roll 기울기 각도(도)")]
    [SerializeField] private float _walkRollAngle = 0.6f;
    [Tooltip("달리기 상태 최대 roll 기울기 각도(도)")]
    [SerializeField] private float _runRollAngle = 1.2f;
    [Tooltip("이동 중 회전 오프셋으로 수렴하는 속도")]
    [SerializeField] private float _rotationApplySpeed = 12f;
    [Tooltip("정지/차단 시 회전 오프셋 복귀 속도")]
    [SerializeField] private float _rotationRecoverSpeed = 10f;

    [Header("Posture Multiplier")]
    [Tooltip("서기 상태 bob 배율")]
    [SerializeField] private float _standingMultiplier = 1f;
    [Tooltip("웅크리기 상태 bob 배율")]
    [SerializeField] private float _crouchingMultiplier = 0.6f;
    [Tooltip("엎드리기 상태 bob 배율")]
    [SerializeField] private float _proningMultiplier = 0.35f;

    [Header("Debug")]
    [Tooltip("켜면 bob 상태 로그를 콘솔에 출력합니다")]
    [SerializeField] private bool _debugStateLog = false;

    private IPostureProvider _postureProvider;
    private IGroundChecker _groundChecker;

    private Vector3 _currentOffset;
    private Quaternion _currentRotationOffset = Quaternion.identity;
    private float _bobTime;
    private float _nextLogTime;

    /// <summary>
    /// 의존성을 캐싱하고, 미할당 시 부모에서 탐색합니다.
    /// </summary>
    private void Awake()
    {
        ResolveDependencies();
    }

    /// <summary>
    /// 다른 위치 제어 스크립트(Update/LateUpdate) 결과 위에 bob 오프셋을 가산합니다.
    /// </summary>
    private void LateUpdate()
    {
        Vector3 basePosition = transform.localPosition - _currentOffset;
        Quaternion baseRotation = transform.localRotation;
        bool isActive;
        float speedT;
        Vector3 localHorizontalVelocity;
        Vector3 targetOffset = CalculateTargetOffset(out isActive, out speedT, out localHorizontalVelocity);

        if (targetOffset == Vector3.zero)
            _currentOffset = Vector3.Lerp(_currentOffset, Vector3.zero, Time.deltaTime * _recoverSpeed);
        else
            _currentOffset = targetOffset;

        if (_disableLateralPositionOffset)
            _currentOffset.x = 0f;

        Quaternion targetRotationOffset = CalculateTargetRotationOffset(isActive, speedT, localHorizontalVelocity);
        float rotationLerpSpeed = isActive ? _rotationApplySpeed : _rotationRecoverSpeed;
        _currentRotationOffset = Quaternion.Slerp(_currentRotationOffset, targetRotationOffset, Time.deltaTime * rotationLerpSpeed);

        transform.localPosition = basePosition + _currentOffset;
        transform.localRotation = baseRotation * _currentRotationOffset;
    }

    /// <summary>
    /// 현재 이동 상태 기준 bob 목표 오프셋을 계산합니다.
    /// </summary>
    private Vector3 CalculateTargetOffset(out bool isActive, out float speedT, out Vector3 localHorizontalVelocity)
    {
        isActive = false;
        speedT = 0f;
        localHorizontalVelocity = Vector3.zero;

        if (_movementRigidbody == null)
        {
            LogState("No Rigidbody", 0f, false, false);
            return Vector3.zero;
        }

        Vector3 horizontalVelocity = _movementRigidbody.linearVelocity;
        horizontalVelocity.y = 0f;
        localHorizontalVelocity = _movementRigidbody.transform.InverseTransformDirection(horizontalVelocity);

        float speed = horizontalVelocity.magnitude;
        bool isMoving = speed > _moveThreshold;
        bool isGrounded = _groundChecker == null || _groundChecker.IsGrounded();

        if (!isMoving || (_requireGrounded && !isGrounded))
        {
            LogState("Blocked", speed, isMoving, isGrounded);
            return Vector3.zero;
        }

        speedT = Mathf.InverseLerp(_walkReferenceSpeed, _runReferenceSpeed, speed);
        float frequency = Mathf.Lerp(_walkFrequency, _runFrequency, speedT);
        Vector3 amplitude = Vector3.Lerp(_walkAmplitude, _runAmplitude, speedT)
                            * _intensity
                            * GetPostureMultiplier();

        _bobTime += Time.deltaTime * frequency;

        float x = Mathf.Cos(_bobTime * 0.5f) * amplitude.x;
        float y = Mathf.Sin(_bobTime) * amplitude.y;
        float z = Mathf.Abs(Mathf.Sin(_bobTime * 0.5f)) * amplitude.z;

        isActive = true;
        LogState("Active", speed, true, isGrounded);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 현재 이동 상태 기준 카메라 회전 오프셋(yaw/roll)을 계산합니다.
    /// </summary>
    private Quaternion CalculateTargetRotationOffset(bool isActive, float speedT, Vector3 localHorizontalVelocity)
    {
        if (!isActive)
            return Quaternion.identity;

        float postureMultiplier = GetPostureMultiplier();
        float yawAngle = Mathf.Lerp(_walkYawAngle, _runYawAngle, speedT) * _intensity * postureMultiplier;
        float rollAngle = Mathf.Lerp(_walkRollAngle, _runRollAngle, speedT) * _intensity * postureMultiplier;

        float yaw = Mathf.Sin(_bobTime * 0.5f) * yawAngle;
        float lateralRatio = localHorizontalVelocity.x / Mathf.Max(localHorizontalVelocity.magnitude, 0.0001f);
        lateralRatio = Mathf.Clamp(lateralRatio, -1f, 1f);
        float roll = -lateralRatio * rollAngle;

        return Quaternion.Euler(0f, yaw, roll);
    }

    /// <summary>
    /// 자세 상태에 따른 bob 배율을 반환합니다.
    /// </summary>
    private float GetPostureMultiplier()
    {
        if (_postureProvider == null)
            return _standingMultiplier;

        if (_postureProvider.Posture == PostureState.Crouching)
            return _crouchingMultiplier;
        if (_postureProvider.Posture == PostureState.Proning)
            return _proningMultiplier;

        return _standingMultiplier;
    }

    /// <summary>
    /// 인터페이스 의존성(Rigidbody, Posture, GroundChecker)을 해석합니다.
    /// </summary>
    private void ResolveDependencies()
    {
        _postureProvider = _postureProviderComponent as IPostureProvider;
        _groundChecker = _groundCheckerComponent as IGroundChecker;

        if (_postureProvider == null)
        {
            MonoBehaviour[] candidates = GetComponentsInParent<MonoBehaviour>(true);
            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i] is IPostureProvider postureProvider)
                {
                    _postureProvider = postureProvider;
                    if (_postureProviderComponent == null)
                        _postureProviderComponent = candidates[i];
                    break;
                }
            }
        }

        if (_groundChecker == null)
        {
            MonoBehaviour[] candidates = GetComponentsInParent<MonoBehaviour>(true);
            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i] is IGroundChecker groundChecker)
                {
                    _groundChecker = groundChecker;
                    if (_groundCheckerComponent == null)
                        _groundCheckerComponent = candidates[i];
                    break;
                }
            }
        }

        if (_movementRigidbody == null && _postureProviderComponent != null)
            _movementRigidbody = _postureProviderComponent.GetComponent<Rigidbody>();

        if (_movementRigidbody == null)
            _movementRigidbody = GetComponentInParent<Rigidbody>();
    }

    /// <summary>
    /// 디버그 옵션이 켜진 경우 bob 상태를 주기적으로 로그 출력합니다.
    /// </summary>
    private void LogState(string state, float speed, bool isMoving, bool isGrounded)
    {
        if (!_debugStateLog)
            return;

        if (Time.time < _nextLogTime)
            return;

        _nextLogTime = Time.time + 0.5f;
        Debug.Log(
            $"[CameraMovementBobController] {state} | speed:{speed:F2} moving:{isMoving} grounded:{isGrounded} rb:{(_movementRigidbody != null)}",
            this
        );
    }
}
