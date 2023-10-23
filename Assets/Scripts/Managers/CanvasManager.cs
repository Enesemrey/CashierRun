using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : MonoSingleton<CanvasManager>
{
    public enum PanelType
    {
        MainMenu, Game, Success, Fail
    }

    [Header("Canvas Groups")]
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup gameCanvasGroup;
    public CanvasGroup successCanvasGroup;
    public CanvasGroup failCanvasGroup;

    [Space]

    [Header("Standard Objects")]
    public Image screenFader;
    public TextMeshProUGUI levelText;

    CanvasGroup[] canvasArray;
    public TextMeshProUGUI holdToSwerve;
    void Start()
    {
        canvasArray = new CanvasGroup[System.Enum.GetNames(typeof(PanelType)).Length];

        canvasArray[(int)PanelType.MainMenu] = mainMenuCanvasGroup;
        canvasArray[(int)PanelType.Game] = gameCanvasGroup;
        canvasArray[(int)PanelType.Success] = successCanvasGroup;
        canvasArray[(int)PanelType.Fail] = failCanvasGroup;

        foreach (CanvasGroup canvas in canvasArray)
        {
            canvas.gameObject.SetActive(true);
            canvas.alpha = 0;
        }

        FadeInScreen(1f);
        ShowPanel(PanelType.MainMenu);

        levelText.text = "Level " + GameManager.instance.GetTotalStagePlayed().ToString();

        GameManager.instance.LevelSuccessEvent += (() => ShowPanel(PanelType.Success));
        GameManager.instance.LevelEndedEvent += (() => ShowPanel(PanelType.Fail));

        // HACK: Workaround for FBSDK
        // FBSDK spawns a persistent EventSystem object. Since Unity 2020.2 there must be only one EventSystem objects at a given time.
        // So we must dispose our own EventSystem object if it exists.
        UnityEngine.EventSystems.EventSystem[] eventSystems = FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystems.Length > 1)
        {
            Destroy(GetComponentInChildren<UnityEngine.EventSystems.EventSystem>().gameObject);
            Logger.Log("There are multiple live EventSystem components. Destroying ours.", Logger.LogLevel.Warning);
        }
        ShowText(false);
    }
    public void ShowText(bool activate)
    {
        holdToSwerve.gameObject.SetActive(activate);
        if (activate)
        {
            holdToSwerve.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.7f).SetLoops(-1, LoopType.Yoyo);

        }
    }
    public void ShowPanel(PanelType panelId)
    {
        int panelIndex = (int)panelId;

        for (int i = 0; i < canvasArray.Length; i++)
        {
            if (i == panelIndex)
            {
                FadePanelIn(canvasArray[i]);
            }

            else
            {
                FadePanelOut(canvasArray[i]);
            }
        }
    }

    #region ButtonEvents
    public void OnTapStart()
    {
        GameManager.instance.StartGame();
        ShowPanel(PanelType.Game);
    }

    public void OnTapRestart()
    {
        FadeOutScreen(GameManager.instance.RestartStage, 1);
    }

    public void OnTapContinue()
    {
        FadeOutScreen(GameManager.instance.NextStage, 1);
    }
    #endregion

    #region FadeInOut
    private void FadePanelOut(CanvasGroup panel)
    {
        panel.DOFade(0, 0.75f);
        panel.blocksRaycasts = false;
    }

    private void FadePanelIn(CanvasGroup panel)
    {
        panel.DOFade(1, 0.75f);
        panel.blocksRaycasts = true;
    }

    public void FadeOutScreen(TweenCallback callback, float duration)
    {
        screenFader.DOFade(1, duration).From(0).OnComplete(callback);
    }

    public void FadeOutScreen(float duration)
    {
        screenFader.DOFade(1, duration).From(0);
    }

    public void FadeInScreen(TweenCallback callback, float duration)
    {
        screenFader.DOFade(0, duration).From(1).OnComplete(callback);
    }

    public void FadeInScreen(float duration)
    {
        screenFader.DOFade(0, duration).From(1);
    }
    #endregion
}
