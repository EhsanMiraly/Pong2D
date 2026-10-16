using System;
using System.Data;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class WinLoseDialog
{
    bool winOrLose;

    GameObject parent;

    VisualTreeAsset winLoseWindow_Template;

    PanelRenderer panelRenderer;

    Label winLose_Label;
    Label oK_Label;

    Menu menu;


    public WinLoseDialog(GameObject parent, bool winOrLose)
    {
        this.parent = parent;
        this.winOrLose = winOrLose;

        parent.name = "WinLoseWindow_UI";
        parent.layer = LayerMask.NameToLayer("UI");

        winLoseWindow_Template = Resources.Load<VisualTreeAsset>("UI/WinLosePopUp_Template");

        panelRenderer = parent.AddComponent<PanelRenderer>();
        panelRenderer.panelSettings = Resources.Load<PanelSettings>("UI/PopUpWindows_UI_PanelSettings");
        panelRenderer.visualTreeAsset = winLoseWindow_Template;
        panelRenderer.sortingOrder = 100;

        panelRenderer.RegisterUIReloadCallback(OnUIReloadCallback);

        //ScreenSafeArea.RemoveUnSafeAreaFromUI(root);
    }

    private void OnUIReloadCallback(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        winLose_Label = root.Q<Label>("WinLose_Label");
        oK_Label = root.Q<Label>("OK_Label");

        if (winOrLose)
        {
            winLose_Label.text = "Win";
        }
        else
        {
            winLose_Label.text = "Lose";
        }

        oK_Label.text = "OK";



        oK_Label.RegisterCallback<ClickEvent>(evt =>
        {
            EventsManager.InvokeFinishGame();
            Dispose();
        });
    }

    private void Dispose()
    {
        if (parent != null)
        {
            UnityEngine.Object.Destroy(parent);
            parent = null;
            winLoseWindow_Template = null;
            panelRenderer = null;
            winLose_Label = null;
            menu = null;
        }
    }
}
