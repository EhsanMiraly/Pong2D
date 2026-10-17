using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 100;
        QualitySettings.vSyncCount = 0;

        Time.fixedDeltaTime = 0.01f;
        Time.maximumDeltaTime = 0.05f;

        ScreenSafeArea.RemoveUnSafeAreaFromGame();
    }

}
