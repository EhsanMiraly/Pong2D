using UnityEngine;
using UnityEngine.UIElements;

public class WorldUI : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private int panelRendererVersion = -1;
    private bool isUIReady = false;


    Label aiScore_Label;
    Label youScore_Label;


    VisualElement upButton_VisualElement;
    VisualElement downButton_VisualElement;





    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloadCallback);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReloadCallback);

        RemoveFunctionality();
        DisconnectEvents();
    }

    private void OnUIReloadCallback(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        if (panelRendererVersion == version)
        {
            return;
        }
        panelRendererVersion = version;

        aiScore_Label = root.Q<Label>("AIScore_Label");
        youScore_Label = root.Q<Label>("YouScore_Label");

        upButton_VisualElement = root.Q<VisualElement>("UpButton_VisualElement");
        downButton_VisualElement = root.Q<VisualElement>("DownButton_VisualElement");

        isUIReady = true;

        ConnectEvents();
        AddFunctionality();
    }




    #region Functionality

    private void AddFunctionality()
    {
        upButton_VisualElement.RegisterCallback<PointerDownEvent>(OnUpButtonPointerDown);
        upButton_VisualElement.RegisterCallback<PointerUpEvent>(OnUpButtonPointerUp);
        upButton_VisualElement.RegisterCallback<PointerLeaveEvent>(OnUpButtonPointerLeave);
        upButton_VisualElement.RegisterCallback<PointerCancelEvent>(OnUpButtonPointerCancel);

        downButton_VisualElement.RegisterCallback<PointerDownEvent>(OnDownButtonPointerDown);
        downButton_VisualElement.RegisterCallback<PointerUpEvent>(OnDownButtonPointerUp);
        downButton_VisualElement.RegisterCallback<PointerLeaveEvent>(OnDownButtonPointerLeave);
        downButton_VisualElement.RegisterCallback<PointerCancelEvent>(OnDownButtonPointerCancel);
    }

    private void RemoveFunctionality()
    {
        upButton_VisualElement.UnregisterCallback<PointerDownEvent>(OnUpButtonPointerDown);
        upButton_VisualElement.UnregisterCallback<PointerUpEvent>(OnUpButtonPointerUp);
        upButton_VisualElement.UnregisterCallback<PointerLeaveEvent>(OnUpButtonPointerLeave);
        upButton_VisualElement.UnregisterCallback<PointerCancelEvent>(OnUpButtonPointerCancel);

        downButton_VisualElement.UnregisterCallback<PointerDownEvent>(OnDownButtonPointerDown);
        downButton_VisualElement.UnregisterCallback<PointerUpEvent>(OnDownButtonPointerUp);
        downButton_VisualElement.UnregisterCallback<PointerLeaveEvent>(OnDownButtonPointerLeave);
        downButton_VisualElement.UnregisterCallback<PointerCancelEvent>(OnDownButtonPointerCancel);
    }


    //Up Button
    private void OnUpButtonPointerDown(PointerDownEvent pointerDownEvent)
    {
        EventsManager.InvokeMoveUpButtonSelected();
    }

    private void OnUpButtonPointerUp(PointerUpEvent pointerUpEvent)
    {
        EventsManager.InvokeMoveButtonReleased();
    }

    private void OnUpButtonPointerLeave(PointerLeaveEvent pointerLeaveEvent)
    {
        EventsManager.InvokeMoveButtonReleased();
    }

    private void OnUpButtonPointerCancel(PointerCancelEvent pointerCancelEvent)
    {
        EventsManager.InvokeMoveButtonReleased();
    }

    //Down Button
    private void OnDownButtonPointerDown(PointerDownEvent pointerDownEvent)
    {
        EventsManager.InvokeMoveDownButtonSelected();
    }

    private void OnDownButtonPointerUp(PointerUpEvent pointerUpEvent)
    {
        EventsManager.InvokeMoveButtonReleased();
    }

    private void OnDownButtonPointerLeave(PointerLeaveEvent pointerLeaveEvent)
    {
        EventsManager.InvokeMoveButtonReleased();
    }

    private void OnDownButtonPointerCancel(PointerCancelEvent pointerCancelEvent)
    {
        EventsManager.InvokeMoveButtonReleased();
    }

    #endregion




    #region Events Manager

    private void ConnectEvents()
    {
        EventsManager.AIScoreChanged_Event += OnAIScoreChanged;
        EventsManager.YouScoreChanged_Event += OnYouScoreChanged;
    }

    private void DisconnectEvents()
    {
        EventsManager.AIScoreChanged_Event -= OnAIScoreChanged;
        EventsManager.YouScoreChanged_Event -= OnYouScoreChanged;
    }


    private void OnAIScoreChanged()
    {
        if (!isUIReady)
            return;

        aiScore_Label.text = "" + GameData.AIScore;
    }

    private void OnYouScoreChanged()
    {
        if (!isUIReady)
            return;

        youScore_Label.text = "" + GameData.YouScore;
    }

    #endregion

}
