using UnityEngine;

public class PlayerPrefsStatStorage : StatStorage
{
    private const string KEY = "clicker_stats_v1";

    public override void Save(StatData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public override StatData Load()
    {
        if (!PlayerPrefs.HasKey(KEY)) return new StatData { totalPlayTime = 0, points = 0 };
        string json = PlayerPrefs.GetString(KEY);
        try
        {
            return JsonUtility.FromJson<StatData>(json) ?? new StatData();
        }
        catch
        {
            return new StatData { totalPlayTime = 0, points = 0 };
        }
    }
}
