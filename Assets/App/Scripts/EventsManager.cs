
public delegate void OnNotify();

public class EventsManager
{
    public static event OnNotify StartGame_Event;
    public static void InvokeStartGame()
    {
        StartGame_Event?.Invoke();
    }


    public static event OnNotify FinishGame_Event;
    public static void InvokeFinishGame()
    {
        FinishGame_Event?.Invoke();
    }


    #region Score

    public static event OnNotify AIScoreChanged_Event;
    public static void InvokeAIScoreChanged()
    {
        AIScoreChanged_Event?.Invoke();
    }

    public static event OnNotify YouScoreChanged_Event;
    public static void InvokeYouScoreChanged()
    {
        YouScoreChanged_Event?.Invoke();
    }

    #endregion


    #region Move Buttons

    public static event OnNotify MoveUpButtonSelected_Event;
    public static void InvokeMoveUpButtonSelected()
    {
        MoveUpButtonSelected_Event?.Invoke();
    }

    public static event OnNotify MoveDownButtonSelected_Event;
    public static void InvokeMoveDownButtonSelected()
    {
        MoveDownButtonSelected_Event?.Invoke();
    }

    public static event OnNotify MoveButtonReleased_Event;
    public static void InvokeMoveButtonReleased()
    {
        MoveButtonReleased_Event?.Invoke();
    }

    #endregion


    #region Difficulity

    public static event OnNotify EasyGame_Event;
    public static void InvokeEasyGame()
    {
        EasyGame_Event?.Invoke();
    }

    public static event OnNotify MediumGame_Event;
    public static void InvokeMediumGame()
    {
        MediumGame_Event?.Invoke();
    }

    public static event OnNotify HardGame_Event;
    public static void InvokeHardGame()
    {
        HardGame_Event?.Invoke();
    }

    #endregion

}
