using UnityEngine;
using System.IO;

public class Settings_SaveSystem
{
    private static string saveDirectory;
    private static Settings_SaveData settings_SaveData = new Settings_SaveData();


    private static void CreateSaveDirectory()
    {
        saveDirectory = Path.Combine(Application.persistentDataPath, "SaveData");

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    public static string Settings_SaveFileName()
    {
        CreateSaveDirectory();
        return saveDirectory + "/Settings_SaveData" + ".txt";
    }

    public static void Save_Settings()
    {
        settings_SaveData.isBackgroundMusicOn = SettingsData.isBackgroundMusicOn;
        settings_SaveData.isSoundEffectsOn = SettingsData.isSoundEffectsOn;

        File.WriteAllText(Settings_SaveFileName(), JsonUtility.ToJson(settings_SaveData, true));
    }

    public static void Load_Settings()
    {
        if (!File.Exists(Settings_SaveFileName()))
        {
            return;
        }

        string saveContent = File.ReadAllText(Settings_SaveFileName());

        settings_SaveData = JsonUtility.FromJson<Settings_SaveData>(saveContent);

        SettingsData.isBackgroundMusicOn = settings_SaveData.isBackgroundMusicOn;
        SettingsData.isSoundEffectsOn = settings_SaveData.isSoundEffectsOn;
    }

}



[System.Serializable]
public struct Settings_SaveData
{
    public bool isBackgroundMusicOn;
    public bool isSoundEffectsOn;
}
