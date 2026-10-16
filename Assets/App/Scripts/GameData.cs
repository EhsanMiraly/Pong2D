using UnityEngine;

public class GameData : MonoBehaviour
{
    public static bool IsPlaying = false;

    public const int scoreNeedToWin = 2;//change to 10

    private static int aiScore = 0;
    public static int AIScore
    {
        get
        {
            return aiScore;
        }
        set
        {
            aiScore = value;
            EventsManager.InvokeAIScoreChanged();
        }
    }

    private static int youScore = 0;
    public static int YouScore
    {
        get
        {
            return youScore;
        }
        set
        {
            youScore = value;
            EventsManager.InvokeYouScoreChanged();
        }
    }



    public static float PaddleMoveSpeed = 10f;


    public static float MinBallSpeed = 5f;
    public static float MaxBallSpeed = 25f;

    public static float BallSpeedIncrease = 0.1f;

    public static float MaxPaddleYPosition = 3.5f;

    public static float MaxBallXPosition = 9.6f;
    public static float MaxBallYPosition = 4.1f;

    public static float BallXVelocityAdd = 14.1f;
    public static float BallXVelocityDivide = 10.9f;
    public static float MaxBallYVelocity = 19.16f;





}
