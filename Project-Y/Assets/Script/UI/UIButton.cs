using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButton : Button
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;

    [SerializeField] private ButtonType buttonType;

    [SerializeField] private GameObject enableObject;
    [SerializeField] private GameObject disableObject;

    [SerializeField] private string nextSceneName;

    private SoundManager soundManager;

    protected override void Start()
    {
        base.Start();
        soundManager = SoundManager.Instance;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (hoverSound != null)
            soundManager.SFXPlay("UIHover", hoverSound);
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
        if (clickSound != null)
            soundManager.SFXPlay("UIClick", clickSound);

        // 타입별 동작
        switch (buttonType)
        {
            case ButtonType.ChangeCanvas:
                ChangeObject();
                break;

            case ButtonType.OpenPopup:
                if (enableObject != null)
                    enableObject.SetActive(true);
                break;

            case ButtonType.ClosePopup:
                if (disableObject != null)
                    disableObject.SetActive(false);
                break;

            case ButtonType.GoScene:
                SceneManager.LoadScene(nextSceneName);
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

    private void ChangeObject()
    {
        if (disableObject != null)
            disableObject.SetActive(false);

        if (enableObject != null)
            enableObject.SetActive(true);
    }
}
public enum ButtonType
{
    None,
    ChangeCanvas,
    OpenPopup,
    ClosePopup,
    GoScene,
    Quit
}