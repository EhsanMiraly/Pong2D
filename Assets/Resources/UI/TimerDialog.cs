using UnityEngine;
using UnityEngine.UIElements;

public class TimerDialog
{
    Ball ball;

    GameObject parent;

    VisualTreeAsset timerWindow_Template;

    PanelRenderer panelRenderer;

    Label timer_Label;


    public TimerDialog(GameObject parent, Ball ball)
    {
        this.parent = parent;
        this.ball = ball;

        parent.name = "TimerWindow_UI";
        parent.layer = LayerMask.NameToLayer("UI");

        timerWindow_Template = Resources.Load<VisualTreeAsset>("UI/TimerPopUp_Template");

        panelRenderer = parent.AddComponent<PanelRenderer>();
        panelRenderer.panelSettings = Resources.Load<PanelSettings>("UI/PopUpWindows_UI_PanelSettings");
        panelRenderer.visualTreeAsset = timerWindow_Template;
        panelRenderer.sortingOrder = 100;

        panelRenderer.RegisterUIReloadCallback(OnUIReloadCallback);
    }

    private async void OnUIReloadCallback(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        ScreenSafeArea.RemoveUnSafeAreaFromUI(root);

        timer_Label = root.Q<Label>("Timer_Label");

        timer_Label.text = "3";
        await Awaitable.WaitForSecondsAsync(1f);
        timer_Label.text = "2";
        await Awaitable.WaitForSecondsAsync(1f);
        timer_Label.text = "1";
        await Awaitable.WaitForSecondsAsync(1f);

        GameData.IsPlaying = true;
        ball.StartMoving();

        Dispose();
    }




    private void Dispose()
    {
        if (parent != null)
        {
            UnityEngine.Object.Destroy(parent);
            parent = null;
            timerWindow_Template = null;
            panelRenderer = null;
            timer_Label = null;
        }
    }
}
