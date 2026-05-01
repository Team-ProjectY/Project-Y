using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    // 패널 표시와 입력 차단을 함께 제어한다.
    [SerializeField] private CanvasGroup _canvasGroup;

    // 시작하자마자 열어 둘지 결정하는 기본 상태값.
    [SerializeField] private bool _baseEnabled = false;

    [Header("애니메이션 설정")]
    // 페이드 인/아웃 재생 시간(초).
    [SerializeField] private float _duration = 0.2f;
    // 첫 Open 호출 시 페이드 사용 여부.
    [SerializeField] private bool _useFadeByDefault = true;

    // 현재 재생 중인 전환 Tween.
    private Tween _activeTween;

    void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (_baseEnabled)
            Open(_useFadeByDefault);
        else
            // 첫 닫기는 무조건 애니메이션 없이 동작한다
            Close(false);
    }

    public void Open(bool useFade)
    {
        _activeTween?.Kill();

        // 닫혀 있던 패널은 먼저 켠 뒤에 Fade를 재생한다.
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        SetInteraction(true);
        _activeTween = CreateFadeInOutTween(true, useFade);
    }

    public void Close(bool useFade)
    {
        _activeTween?.Kill();

        // 닫히는 동안 중복 입력이 들어오지 않게 먼저 막는다.
        SetInteraction(false);

        _activeTween = CreateFadeInOutTween(true, useFade);

        if (_activeTween == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _activeTween.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 캔버스의 상호작용을 수정합니다
    /// </summary>
    /// <param name="isInteractable">상호작용 여부</param>
    private void SetInteraction(bool isInteractable)
    {
        _canvasGroup.interactable = isInteractable;
        _canvasGroup.blocksRaycasts = isInteractable;
    }

    /// <summary>
    /// FadeInOut의 Tween을 제작합니다
    /// </summary>
    /// <param name="isOpen">캔버스를 여는거면 true 아니면 false를 씁니다</param>
    /// <param name="useAnimation">애니메이션 사용 여부를 결정합니다</param>
    /// <returns>애니메이션을 사용하지 않으면 null을 반환합니다</returns>
    private Tween CreateFadeInOutTween(bool isOpen, bool useAnimation)
    {
        if (!useAnimation || _duration <= 0f)
        {
            SetVisibleState(isOpen);
            return null;
        }

        SetVisibleState(!isOpen);

        float targetAlpha = isOpen ? 1f : 0f;
        return _canvasGroup.DOFade(targetAlpha, _duration).SetUpdate(true);
    }

    private void OnDestroy()
    {
        // 오브젝트가 제거될 때 남아 있는 Tween도 같이 정리한다.
        _activeTween?.Kill();
    }

    /// <summary>
    /// CanvasGroup의 알파값을 0 또는 1로 정합니다.
    /// </summary>
    /// <param name="isVisible">캔버스가 보이는지 여부를 정합니다</param>
    private void SetVisibleState(bool isVisible)
    {
        _canvasGroup.alpha = isVisible ? 1f : 0f;
    }
}
