using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenUI : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private int panelRendererVersion = -1;
    //private bool isUIReady = false;

    private AudioSource audioSource;


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
    VisualElement soundEffectsCheckMark_VisualElement;
    VisualElement backgroundMusicCheckMark_VisualElement;


    VisualElement menuButtonPage_VisualElement;
    VisualElement menuButton_VisualElement;



    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloadCallback);

        audioSource = GetComponent<AudioSource>();
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
        soundEffectsCheckMark_VisualElement = settingsPage_VisualElement.Q<VisualElement>("SoundEffectsCheckMark_VisualElement");
        backgroundMusicCheckMark_VisualElement = settingsPage_VisualElement.Q<VisualElement>("BackgroundMusicCheckMark_VisualElement");


        menuButtonPage_VisualElement = root.Q<VisualElement>("MenuButtonPage_VisualElement");
        menuButton_VisualElement = menuButtonPage_VisualElement.Q<VisualElement>("MenuButton_VisualElement");


        //isUIReady = true;
        ConnectEvents();
        AddFunctionality();
        ScreenSafeArea.RemoveUnSafeAreaFromUI(root);
        SetPageActive(mainPage_VisualElement);

        Settings_SaveSystem.Load_Settings();

        if (SettingsData.isSoundEffectsOn)
        {
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.Flex;
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.None;
        }
        else
        {
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.None;
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.Flex;
        }

        if (SettingsData.isBackgroundMusicOn)
        {
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.Flex;
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.None;

            audioSource.Play();
        }
        else
        {
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.None;
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.Flex;

            audioSource.Stop();
        }

    }


    #region Functionality

    private void AddFunctionality()
    {
        newGame_Label.RegisterCallback<ClickEvent>(OnNewGameSelected);
        settings_Label.RegisterCallback<ClickEvent>(OnSettingsSelected);
        exit_Label.RegisterCallback<ClickEvent>(OnExitSelected);

        difficultyPageBack_VisualElement.RegisterCallback<ClickEvent>(OnDifficultyPageBackSelected);
        easy_Label.RegisterCallback<ClickEvent>(OnEasySelected);
        medium_Label.RegisterCallback<ClickEvent>(OnMediumSelected);
        hard_Label.RegisterCallback<ClickEvent>(OnHardSelected);

        settingsPageBack_VisualElement.RegisterCallback<ClickEvent>(OnSettingsPageBackSelected);
        soundEffectsCheckMark_VisualElement.RegisterCallback<ClickEvent>(OnSoundEffectsCheckMarkSelected);
        backgroundMusicCheckMark_VisualElement.RegisterCallback<ClickEvent>(OnBackgroundMusicCheckMarkSelected);

        menuButton_VisualElement.RegisterCallback<ClickEvent>(OnMenuButtonSelected);
    }


    private void RemoveFunctionality()
    {
        newGame_Label.UnregisterCallback<ClickEvent>(OnNewGameSelected);
        settings_Label.UnregisterCallback<ClickEvent>(OnSettingsSelected);
        exit_Label.UnregisterCallback<ClickEvent>(OnExitSelected);

        difficultyPageBack_VisualElement.UnregisterCallback<ClickEvent>(OnDifficultyPageBackSelected);
        easy_Label.UnregisterCallback<ClickEvent>(OnEasySelected);
        medium_Label.UnregisterCallback<ClickEvent>(OnMediumSelected);
        hard_Label.UnregisterCallback<ClickEvent>(OnHardSelected);

        settingsPageBack_VisualElement.UnregisterCallback<ClickEvent>(OnSettingsPageBackSelected);
        soundEffectsCheckMark_VisualElement.UnregisterCallback<ClickEvent>(OnSoundEffectsCheckMarkSelected);
        backgroundMusicCheckMark_VisualElement.UnregisterCallback<ClickEvent>(OnBackgroundMusicCheckMarkSelected);

        menuButton_VisualElement.UnregisterCallback<ClickEvent>(OnMenuButtonSelected);
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

    private void OnSoundEffectsCheckMarkSelected(ClickEvent clickEvent)
    {
        SettingsData.isSoundEffectsOn = !SettingsData.isSoundEffectsOn;

        if (SettingsData.isSoundEffectsOn)
        {
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.Flex;
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.None;
        }
        else
        {
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.None;
            soundEffectsCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.Flex;
        }

        Settings_SaveSystem.Save_Settings();
    }

    private void OnBackgroundMusicCheckMarkSelected(ClickEvent clickEvent)
    {
        SettingsData.isBackgroundMusicOn = !SettingsData.isBackgroundMusicOn;

        if (SettingsData.isBackgroundMusicOn)
        {
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.Flex;
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.None;

            audioSource.Play();
        }
        else
        {
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("On_VisualElement").style.display =
                DisplayStyle.None;
            backgroundMusicCheckMark_VisualElement.Q<VisualElement>("Off_VisualElement").style.display =
                DisplayStyle.Flex;

            audioSource.Stop();
        }

        Settings_SaveSystem.Save_Settings();
    }

    private void OnMenuButtonSelected(ClickEvent clickEvent)
    {
        GameData.IsPlaying = false;
        FindAnyObjectByType<Ball>().GetComponent<Ball>().StopMoving();
        SetPageActive(mainPage_VisualElement);

        Time.timeScale = 0f;
    }

    #endregion



    #region EventsManager

    public void ConnectEvents()
    {
        EventsManager.FinishGame_Event += OnFinishGame;
    }

    public void DisconnectEvents()
    {
        EventsManager.FinishGame_Event -= OnFinishGame;
    }


    private void OnFinishGame()
    {
        Time.timeScale = 0f;
        SetPageActive(mainPage_VisualElement);
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
