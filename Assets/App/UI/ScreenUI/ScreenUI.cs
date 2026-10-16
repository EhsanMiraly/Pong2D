using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenUI : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private int panelRendererVersion = -1;
    //private bool isUIReady = false;


    VisualElement mainPage_VisualElement;
    Label newGame_Label;
    Label settings_Label;
    Label exit_Label;

    VisualElement difficultyPage_VisualElement;
    VisualElement difficultyPageBack_VisualElement;
    Label easy_Label;
    Label medium_Label;
    Label hard_Label;

    VisualElement settingsPage_VisualElement;
    VisualElement settingsPageBack_VisualElement;


    VisualElement menuButtonPage_VisualElement;
    VisualElement menuButton_VisualElement;



    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloadCallback);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReloadCallback);

        RemoveFunctionality();
    }


    private void OnUIReloadCallback(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        if (panelRendererVersion == version)
        {
            return;
        }
        panelRendererVersion = version;

        mainPage_VisualElement = root.Q<VisualElement>("MainPage_VisualElement");
        newGame_Label = mainPage_VisualElement.Q<Label>("NewGame_Label");
        settings_Label = mainPage_VisualElement.Q<Label>("Settings_Label");
        exit_Label = mainPage_VisualElement.Q<Label>("Exit_Label");

        difficultyPage_VisualElement = root.Q<VisualElement>("DifficultyPage_VisualElement");
        difficultyPageBack_VisualElement = difficultyPage_VisualElement.Q<VisualElement>("Back_VisualElement");
        easy_Label = difficultyPage_VisualElement.Q<Label>("Easy_Label");
        medium_Label = difficultyPage_VisualElement.Q<Label>("Medium_Label");
        hard_Label = difficultyPage_VisualElement.Q<Label>("Hard_Label");

        settingsPage_VisualElement = root.Q<VisualElement>("SettingsPage_VisualElement");
        settingsPageBack_VisualElement = settingsPage_VisualElement.Q<VisualElement>("Back_VisualElement");


        menuButtonPage_VisualElement = root.Q<VisualElement>("MenuButtonPage_VisualElement");
        menuButton_VisualElement = menuButtonPage_VisualElement.Q<VisualElement>("MenuButton_VisualElement");


        //isUIReady = true;

        AddFunctionality();

        SetPageActive(mainPage_VisualElement);
    }


    #region Functionality

    private void AddFunctionality()
    {
        newGame_Label.RegisterCallback<ClickEvent>(OnNewGameSelected);
        settings_Label.RegisterCallback<ClickEvent>(OnSettingsSelected);
        exit_Label.RegisterCallback<ClickEvent>(OnExitSelected);

        difficultyPageBack_VisualElement.RegisterCallback<ClickEvent>(OnDifficultyPageBackSelected);
        settingsPageBack_VisualElement.RegisterCallback<ClickEvent>(OnSettingsPageBackSelected);

        easy_Label.RegisterCallback<ClickEvent>(OnEasySelected);
        medium_Label.RegisterCallback<ClickEvent>(OnMediumSelected);
        hard_Label.RegisterCallback<ClickEvent>(OnHardSelected);
    }


    private void RemoveFunctionality()
    {
        newGame_Label.UnregisterCallback<ClickEvent>(OnNewGameSelected);
        settings_Label.UnregisterCallback<ClickEvent>(OnSettingsSelected);
        exit_Label.UnregisterCallback<ClickEvent>(OnExitSelected);

        easy_Label.UnregisterCallback<ClickEvent>(OnEasySelected);
        medium_Label.UnregisterCallback<ClickEvent>(OnMediumSelected);
        hard_Label.UnregisterCallback<ClickEvent>(OnHardSelected);
    }


    private void OnNewGameSelected(ClickEvent clickEvent)
    {
        SetPageActive(difficultyPage_VisualElement);
    }

    private void OnSettingsSelected(ClickEvent clickEvent)
    {
        SetPageActive(settingsPage_VisualElement);
    }

    private void OnExitSelected(ClickEvent clickEvent)
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDifficultyPageBackSelected(ClickEvent clickEvent)
    {
        SetPageActive(mainPage_VisualElement);
    }

    private void OnSettingsPageBackSelected(ClickEvent clickEvent)
    {
        SetPageActive(mainPage_VisualElement);
    }

    private void OnEasySelected(ClickEvent clickEvent)
    {
        EventsManager.InvokeEasyGame();
        EventsManager.InvokeStartGame();

        SetPageActive(menuButtonPage_VisualElement);
    }

    private void OnMediumSelected(ClickEvent clickEvent)
    {
        EventsManager.InvokeMediumGame();
        EventsManager.InvokeStartGame();

        SetPageActive(menuButtonPage_VisualElement);
    }

    private void OnHardSelected(ClickEvent clickEvent)
    {
        EventsManager.InvokeHardGame();
        EventsManager.InvokeStartGame();

        SetPageActive(menuButtonPage_VisualElement);
    }

    #endregion


    private void SetPageActive(VisualElement pageVisualElement)
    {
        mainPage_VisualElement.style.display = DisplayStyle.None;
        difficultyPage_VisualElement.style.display = DisplayStyle.None;
        settingsPage_VisualElement.style.display = DisplayStyle.None;
        menuButtonPage_VisualElement.style.display = DisplayStyle.None;

        pageVisualElement.style.display = DisplayStyle.Flex;
    }

}
