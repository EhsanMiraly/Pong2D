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

    AudioSource audioSource;
    AudioClip audioClip;


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


        //Sound
        audioSource = parent.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        if (winOrLose)
        {
            audioClip = Resources.Load<AudioClip>("Audio/wingame");
        }
        else
        {
            audioClip = Resources.Load<AudioClip>("Audio/gameover");
        }

        PlaySound();
    }

    private void OnUIReloadCallback(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        ScreenSafeArea.RemoveUnSafeAreaFromUI(root);

        winLose_Label = root.Q<Label>("WinLose_Label");
        oK_Label = root.Q<Label>("OK_Label");

        if (winOrLose)
        {
            winLose_Label.text = "Win";
            winLose_Label.style.color = Color.green;
        }
        else
        {
            winLose_Label.text = "Lose";
            winLose_Label.style.color = Color.red;
        }

        oK_Label.text = "OK";



        oK_Label.RegisterCallback<ClickEvent>(evt =>
        {
            EventsManager.InvokeFinishGame();
            Dispose();
        });
    }


    private void PlaySound()
    {
        if (!SettingsData.isSoundEffectsOn)
            return;

        audioSource.clip = audioClip;
        audioSource.volume = 1f;
        audioSource.Play();
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
        }
    }
}
