using System.Threading.Tasks;
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


    private void OnStartGame()
    {
        Time.timeScale = 1f;

        GameData.AIScore = 0;
        GameData.YouScore = 0;

        leftPaddle.GetComponent<LeftPaddle>().ResetAgent();
        rightPaddle.GetComponent<RightPaddle>().ResetPaddle();

        new TimerDialog(new GameObject(), ball.GetComponent<Ball>());
    }

    private void ResetGameEnvironment()
    {
        leftPaddle.GetComponent<LeftPaddle>().ResetAgent();
        rightPaddle.GetComponent<RightPaddle>().ResetPaddle();
        ball.GetComponent<Ball>().StopMoving();

        GameData.IsPlaying = false;
        new TimerDialog(new GameObject(), ball.GetComponent<Ball>());
    }


    private void OnAIScoreChanged()
    {
        if (GameData.AIScore >= GameData.scoreNeedToWin)
        {
            GameData.IsPlaying = false;
            ball.GetComponent<Ball>().StopMoving();

            WinLoseDialog winLoseDialog = new WinLoseDialog(new GameObject(), false);
        }
        else
        {
            ResetGameEnvironment();
        }
    }

    private void OnYouScoreChanged()
    {
        if (GameData.YouScore >= GameData.scoreNeedToWin)
        {
            GameData.IsPlaying = false;
            ball.GetComponent<Ball>().StopMoving();

            WinLoseDialog winLoseDialog = new WinLoseDialog(new GameObject(), true);
        }
        else
        {
            ResetGameEnvironment();
        }
    }

    #endregion

}
