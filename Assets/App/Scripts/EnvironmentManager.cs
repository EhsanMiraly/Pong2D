using Unity.InferenceEngine;
using Unity.MLAgents;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject leftPaddle;
    [SerializeField] private ModelAsset easyBrain;
    [SerializeField] private ModelAsset mediumBrain;
    [SerializeField] private ModelAsset hardBrain;
    [SerializeField] private GameObject rightPaddle;
    [SerializeField] private GameObject ball;


    private void OnEnable()
    {
        ConnectEvents();
    }

    private void OnDisable()
    {
        DisconnectEvents();
    }




    #region EventsManager

    private void ConnectEvents()
    {
        EventsManager.EasyGame_Event += OnEasyGame;
        EventsManager.MediumGame_Event += OnMediumGame;
        EventsManager.HardGame_Event += OnHardGame;

        EventsManager.StartGame_Event += OnStartGame;

        EventsManager.AIScoreChanged_Event += OnAIScoreChanged;
        EventsManager.YouScoreChanged_Event += OnYouScoreChanged;


        //Hit End Wall
    }

    private void DisconnectEvents()
    {
        EventsManager.EasyGame_Event -= OnEasyGame;
        EventsManager.MediumGame_Event -= OnMediumGame;
        EventsManager.HardGame_Event -= OnHardGame;

        EventsManager.StartGame_Event -= OnStartGame;

        EventsManager.AIScoreChanged_Event -= OnAIScoreChanged;
        EventsManager.YouScoreChanged_Event -= OnYouScoreChanged;
    }

    private void OnEasyGame()
    {
        leftPaddle.GetComponent<Agent>().SetModel("Brain", easyBrain);
    }

    private void OnMediumGame()
    {
        leftPaddle.GetComponent<Agent>().SetModel("Brain", mediumBrain);
    }

    private void OnHardGame()
    {
        leftPaddle.GetComponent<Agent>().SetModel("Brain", hardBrain);
    }


    private async void OnStartGame()
    {
        GameData.AIScore = 0;
        GameData.YouScore = 0;

        leftPaddle.GetComponent<LeftPaddle>().ResetAgent();
        rightPaddle.GetComponent<RightPaddle>().ResetPaddle();

        await Awaitable.WaitForSecondsAsync(3f);//Or Start Timer Dialog

        GameData.IsPlaying = true;

        ball.GetComponent<Ball>().StartMoving();
    }

    private void OnResetGameEnvironment()
    {
        leftPaddle.GetComponent<LeftPaddle>().ResetAgent();
        rightPaddle.GetComponent<RightPaddle>().ResetPaddle();
        ball.GetComponent<Ball>().StopMoving();

        //await 3 second

        ball.GetComponent<Ball>().StartMoving();
    }

    private void OnFinishGame()
    {
        ///Dialog and other stuff

        GameData.IsPlaying = false;

        ball.GetComponent<Ball>().StopMoving();
    }


    private void OnAIScoreChanged()
    {
        if (GameData.AIScore >= GameData.scoreNeedToWin)
        {
            WinLoseDialog winLoseDialog = new WinLoseDialog(new GameObject(), false);
        }
    }

    private void OnYouScoreChanged()
    {
        if (GameData.YouScore >= GameData.scoreNeedToWin)
        {
            WinLoseDialog winLoseDialog = new WinLoseDialog(new GameObject(), true);
        }
    }

    #endregion

}
