using UnityEngine;
using DG.Tweening;

/// <summary>
/// 각 UI요소에 부착되는 애니메이터다.
/// 독립적으로 실행 될 수는 없고,
/// 위에 패널이나 캔버스에 있는 UIPanel스크립트의 참조되어서 Open과 Close가 호출된다. 
/// 
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UIAnimator : MonoBehaviour
{
    // UI가 어떤 방식으로 열리고 닫힐지 정의하는 설정값.
    [SerializeField] private UIAnimaType _uiAnimaType;

    // FadeInOut용 필드
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField] private float _fadeOutDuration = 0.3f;

    // ScaleInOut용 필드
    [SerializeField] private float _scaleInDuration = 0.3f;
    [SerializeField] private float _scaleOutDuration = 0.3f;

    // Moving용 필드
    [SerializeField] private Vector3 _moveOffset = Vector3.zero;
    [SerializeField] private float _moveDuration = 0.3f;

    private RectTransform _rectTransform;
    private Vector3 _baseLocalPosition;
    private Vector3 _baseLocalScale;
    private Tween _activeTween;
    private bool _isOpen;

    private void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        _rectTransform = transform as RectTransform;
        _baseLocalPosition = transform.localPosition;
        _baseLocalScale = transform.localScale;
        _isOpen = DetermineOpenStateFromCurrentValues();
    }

    /// <summary>
    /// UI를 열 때 설정된 애니메이션 타입을 실행합니다.
    /// </summary>
    public void Open()
    {
        // 이미 열린 상태면 중복 Open 애니메이션을 실행하지 않음
        if (_isOpen)
            return;

        PlayTransition(true);
    }

    /// <summary>
    /// UI를 닫을 때 설정된 애니메이션 타입을 실행합니다.
    /// </summary>
    public void Close()
    {
        // 이미 닫힌 상태면 중복 Close 애니메이션을 실행하지 않음
        if (!_isOpen)
            return;

        PlayTransition(false);
    }

    /// <summary>
    /// 오브젝트 제거 시 실행 중인 Tween을 정리합니다.
    /// </summary>
    private void OnDestroy()
    {
        _activeTween?.Kill();
    }

    /// <summary>
    /// Open/Close 전환의 공통 흐름을 처리합니다.
    /// </summary>
    /// <param name="isOpen">열기면 true, 닫기면 false</param>
    private void PlayTransition(bool isOpen)
    {
        // 상태를 먼저 갱신해서 같은 요청의 중복 호출을 방지
        _isOpen = isOpen;

        // 이전 전환이 남아 있으면 먼저 종료
        _activeTween?.Kill();

        // 타입이 None이면 애니메이션 없이 즉시 상태 반영
        if (_uiAnimaType == UIAnimaType.None)
        {
            ApplyImmediateState(isOpen);
            return;
        }

        // 타입별 Tween을 하나의 시퀀스로 합성
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        AppendFadeTween(sequence, isOpen);
        AppendScaleTween(sequence, isOpen);
        AppendMoveTween(sequence, isOpen);

        // 실제로 붙은 Tween이 있을 때만 activeTween으로 보관
        if (sequence.active && sequence.Duration(false) > 0f)
        {
            _activeTween = sequence;
            return;
        }

        // 유효 Tween이 없으면 시퀀스 정리 후 즉시 상태 반영
        sequence.Kill();
        ApplyImmediateState(isOpen);
    }

    /// <summary>
    /// FadeInOut 타입에 해당하면 알파 전환 Tween을 추가합니다.
    /// </summary>
    /// <param name="sequence">결합 대상 시퀀스</param>
    /// <param name="isOpen">열기 여부</param>
    private void AppendFadeTween(Sequence sequence, bool isOpen)
    {
        if (!HasType(UIAnimaType.FadeInOut) || _canvasGroup == null)
            return;

        float duration = isOpen ? _fadeInDuration : _fadeOutDuration;
        float targetAlpha = isOpen ? 1f : 0f;

        if (duration <= 0f)
        {
            _canvasGroup.alpha = targetAlpha;
            return;
        }

        // 열기 시 시작 알파를 0으로 고정해 페이드 인 시작점을 보장
        if (isOpen)
            _canvasGroup.alpha = 0f;

        sequence.Join(_canvasGroup.DOFade(targetAlpha, duration));
    }

    /// <summary>
    /// ScaleInOut 타입에 해당하면 스케일 전환 Tween을 추가합니다.
    /// </summary>
    /// <param name="sequence">결합 대상 시퀀스</param>
    /// <param name="isOpen">열기 여부</param>
    private void AppendScaleTween(Sequence sequence, bool isOpen)
    {
        if (!HasType(UIAnimaType.ScaleInOut))
            return;

        float duration = isOpen ? _scaleInDuration : _scaleOutDuration;
        Vector3 targetScale = isOpen ? _baseLocalScale : Vector3.zero;

        if (duration <= 0f)
        {
            transform.localScale = targetScale;
            return;
        }

        // 열기 시 0 스케일에서 기준 스케일로 확대
        if (isOpen)
            transform.localScale = Vector3.zero;

        sequence.Join(transform.DOScale(targetScale, duration));
    }

    /// <summary>
    /// Moving 타입에 해당하면 위치 전환 Tween을 추가합니다.
    /// </summary>
    /// <param name="sequence">결합 대상 시퀀스</param>
    /// <param name="isOpen">열기 여부</param>
    private void AppendMoveTween(Sequence sequence, bool isOpen)
    {
        if (!HasType(UIAnimaType.Moving))
            return;

        float duration = _moveDuration;
        Vector3 targetPosition = isOpen ? _baseLocalPosition : _baseLocalPosition + _moveOffset;

        if (duration <= 0f)
        {
            transform.localPosition = targetPosition;
            return;
        }

        // 열기 시 오프셋 위치에서 기준 위치로 이동
        if (isOpen)
            transform.localPosition = _baseLocalPosition + _moveOffset;

        // UI 계층은 RectTransform 이동을 우선 적용
        if (_rectTransform != null)
            sequence.Join(_rectTransform.DOLocalMove(targetPosition, duration));
        else
            sequence.Join(transform.DOLocalMove(targetPosition, duration));
    }

    /// <summary>
    /// 애니메이션 없이 즉시 목표 상태를 적용합니다.
    /// </summary>
    /// <param name="isOpen">열기 여부</param>
    private void ApplyImmediateState(bool isOpen)
    {
        if (HasType(UIAnimaType.FadeInOut) && _canvasGroup != null)
            _canvasGroup.alpha = isOpen ? 1f : 0f;

        if (HasType(UIAnimaType.ScaleInOut))
            transform.localScale = isOpen ? _baseLocalScale : Vector3.zero;

        if (HasType(UIAnimaType.Moving))
            transform.localPosition = isOpen ? _baseLocalPosition : _baseLocalPosition + _moveOffset;
    }

    /// <summary>
    /// 현재 애니메이션 플래그에 특정 타입이 포함되어 있는지 검사합니다.
    /// </summary>
    /// <param name="type">검사할 애니메이션 타입</param>
    /// <returns>포함되어 있으면 true</returns>
    private bool HasType(UIAnimaType type)
    {
        return (_uiAnimaType & type) == type;
    }

    /// <summary>
    /// 현재 시각 상태를 기준으로 열린 상태인지 판정합니다.
    /// </summary>
    /// <returns>열린 상태로 보이면 true</returns>
    private bool DetermineOpenStateFromCurrentValues()
    {
        bool visibleOpen = !HasType(UIAnimaType.FadeInOut) || _canvasGroup == null || _canvasGroup.alpha >= 0.99f;
        bool scaleOpen = !HasType(UIAnimaType.ScaleInOut) || Vector3.Distance(transform.localScale, _baseLocalScale) <= 0.0001f;
        bool moveOpen = !HasType(UIAnimaType.Moving) || Vector3.Distance(transform.localPosition, _baseLocalPosition) <= 0.0001f;
        return visibleOpen && scaleOpen && moveOpen;
    }
}
