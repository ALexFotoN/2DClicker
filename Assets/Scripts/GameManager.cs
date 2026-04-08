using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    public double pointsPerSecond = 10.0;
    public StatStorage storage;

    [Header("UI")]
    public Text pointsText;
    public Text timerText;
    public Button clickButton;

    private double totalPoints;
    private double sessionStartTime;
    private double accumulatedPlayTimeOnLoad;
    private bool isHolding = false;
    private float holdStartTime = 0f;

    private float autoSaveInterval = 5f;
    private float autoSaveTimer = 0f;

    void Awake()
    {
        if (storage == null) Debug.LogWarning("Storage not assigned in inspector.");
    }

    void Start()
    {
        StatData loaded = storage != null ? storage.Load() : new StatData();
        totalPoints = loaded.points;
        accumulatedPlayTimeOnLoad = loaded.totalPlayTime;

        sessionStartTime = Time.realtimeSinceStartupAsDouble;
        UpdateUI();

        var handler = clickButton.gameObject.GetComponent<ClickButtonHandler>();
        if (handler == null) handler = clickButton.gameObject.AddComponent<ClickButtonHandler>();
        handler.OnHoldStart += OnHoldStart;
        handler.OnHoldEnd += OnHoldEnd;
    }

    void Update()
    {
        double currentPlayTime = accumulatedPlayTimeOnLoad + (Time.realtimeSinceStartupAsDouble - sessionStartTime);
        UpdateTimerText(currentPlayTime);

        autoSaveTimer += Time.unscaledDeltaTime;
        if (autoSaveTimer >= autoSaveInterval)
        {
            autoSaveTimer = 0f;
            SaveStats(currentPlayTime);
        }
    }

    private void UpdateUI()
    {
        pointsText.text = $"Points: {totalPoints:F2}";
    }

    private void UpdateTimerText(double seconds)
    {
        int s = (int)seconds;
        int mins = s / 60;
        int secs = s % 60;
        timerText.text = $"Time: {mins:00}:{secs:00}";
    }

    private void OnHoldStart()
    {
        isHolding = true;
        holdStartTime = Time.unscaledTime;
    }

    private void OnHoldEnd()
    {
        if (!isHolding) return;
        float holdDuration = Time.unscaledTime - holdStartTime;
        double gained = pointsPerSecond * holdDuration;
        totalPoints += gained;
        isHolding = false;
        UpdateUI();
    }

    private void SaveStats(double currentPlayTime)
    {
        if (storage == null) return;
        StatData data = new StatData
        {
            points = totalPoints,
            totalPlayTime = currentPlayTime
        };
        storage.Save(data);
    }

    void OnApplicationQuit()
    {
        double currentPlayTime = accumulatedPlayTimeOnLoad + (Time.realtimeSinceStartupAsDouble - sessionStartTime);
        SaveStats(currentPlayTime);
    }

    void OnDisable()
    {
        double currentPlayTime = accumulatedPlayTimeOnLoad + (Time.realtimeSinceStartupAsDouble - sessionStartTime);
        SaveStats(currentPlayTime);
    }
}
