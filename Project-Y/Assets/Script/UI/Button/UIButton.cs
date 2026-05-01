using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButton : Button
{
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;

    [SerializeField] private ButtonType _buttonType;

    [SerializeField] private UIPanel _enablePanel;
    [SerializeField] private UIPanel _disablePanel;

    [SerializeField] private string _nextSceneName;

    private SoundManager _soundManager;

    protected override void Start()
    {
        base.Start();
        _soundManager = SoundManager.Instance;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (_hoverSound != null)
            _soundManager.SFXPlay(_hoverSound);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        HandleClick();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        HandleClick();
    }

    private void HandleClick()
    {
        // 사운드
        if (_clickSound != null)
            _soundManager.SFXPlay(_clickSound);

        // 타입별 동작
        switch (_buttonType)
        {
            case ButtonType.ChangeCanvas:
                ChangePanel();
                break;

            case ButtonType.OpenPopup:
                if (_enablePanel != null)
                    _enablePanel.Open(true);
                break;

            case ButtonType.ClosePopup:
                if (_disablePanel != null)
                    _disablePanel.Close(true);
                break;

            case ButtonType.GoScene:
                SceneManager.LoadScene(_nextSceneName);
                break;

            case ButtonType.Quit:
                QuitGame();
                break;
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ChangePanel()
    {
        if (_disablePanel != null)
            _disablePanel.Close(true);

        if (_enablePanel != null)
            _enablePanel.Open(true);
    }
}
