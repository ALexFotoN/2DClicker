using System.IO;
using UnityEngine;

public class JsonStatStorage : StatStorage
{
    private string FilePath => Path.Combine(Application.persistentDataPath, "clicker_stats.json");

    public override void Save(StatData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(FilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save stats to file: " + e);
        }
    }

    public override StatData Load()
    {
        try
        {
            if (!File.Exists(FilePath)) return new StatData { totalPlayTime = 0, points = 0 };
            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<StatData>(json) ?? new StatData();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load stats from file: " + e);
            return new StatData { totalPlayTime = 0, points = 0 };
        }
    }
}
