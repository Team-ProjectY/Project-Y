using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _baseEnabled = false;

    [Header("Fade")]
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private bool _useFadeByDefault = true;

    [Header("Events")]
    [SerializeField] private UnityEvent _onOpened;
    [SerializeField] private UnityEvent _onClosed;
    [SerializeField] private float _stepDelay = 0.1f;
    [SerializeField] private UnityEvent[] _onOpenedSteps;
    [SerializeField] private UnityEvent[] _onClosedSteps;

    private Tween _activeTween;
    private Tween _stepTween;

    private void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (_baseEnabled)
            Open();
        else
            Close(false);
    }

    public void Open()
    {
        Open(_useFadeByDefault);
    }

    public void Close()
    {
        Close(_useFadeByDefault);
    }

    private void Open(bool useFade)
    {
        _activeTween?.Kill();
        _stepTween?.Kill();

        // Open 순차 실행 전에 대상 애니메이터를 닫힌 상태로 맞춰 플래시를 방지
        PrimeOpenedStepAnimators();

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        SetInteraction(true);
        _activeTween = CreateFadeTween(true, useFade);

        if (_activeTween == null)
        {
            _onOpened?.Invoke();
            PlayStepsSequentially(_onOpenedSteps);
            return;
        }

        _activeTween.OnComplete(() =>
        {
            _onOpened?.Invoke();
            PlayStepsSequentially(_onOpenedSteps);
        });
    }

    private void Close(bool useFade)
    {
        _activeTween?.Kill();
        SetInteraction(false);

        _activeTween = CreateFadeTween(false, useFade);
        if (_activeTween == null)
        {
            _onClosed?.Invoke();
            PlayStepsSequentially(_onClosedSteps);
            gameObject.SetActive(false);
            return;
        }

        _activeTween.OnComplete(() =>
        {
            _onClosed?.Invoke();
            PlayStepsSequentially(_onClosedSteps);
            gameObject.SetActive(false);
        });
    }

    private Tween CreateFadeTween(bool isOpen, bool useFade)
    {
        if (!useFade || _duration <= 0f)
        {
            SetVisibleState(isOpen);
            return null;
        }

        SetVisibleState(!isOpen);
        float targetAlpha = isOpen ? 1f : 0f;
        return _canvasGroup.DOFade(targetAlpha, _duration).SetUpdate(true);
    }

    private void SetInteraction(bool isInteractable)
    {
        _canvasGroup.interactable = isInteractable;
        _canvasGroup.blocksRaycasts = isInteractable;
    }

    private void SetVisibleState(bool isVisible)
    {
        _canvasGroup.alpha = isVisible ? 1f : 0f;
    }

    private void OnDestroy()
    {
        _activeTween?.Kill();
        _stepTween?.Kill();
    }

    private void PlayStepsSequentially(UnityEvent[] steps)
    {
        if (steps == null || steps.Length == 0)
            return;

        _stepTween?.Kill();
        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < steps.Length; i++)
        {
            int index = i;
            sequence.AppendCallback(() => steps[index]?.Invoke());

            if (i < steps.Length - 1)
                sequence.AppendInterval(_stepDelay);
        }

        _stepTween = sequence;
    }

    private void PrimeOpenedStepAnimators()
    {
        if (_onOpenedSteps == null || _onOpenedSteps.Length == 0)
            return;

        for (int i = 0; i < _onOpenedSteps.Length; i++)
        {
            UnityEvent evt = _onOpenedSteps[i];
            if (evt == null)
                continue;

            int count = evt.GetPersistentEventCount();
            for (int j = 0; j < count; j++)
            {
                Object target = evt.GetPersistentTarget(j);
                string method = evt.GetPersistentMethodName(j);
                if (target is UIAnimator animator && method == nameof(UIAnimator.Open))
                    animator.SetClosedStateImmediate();
            }
        }
    }
}
