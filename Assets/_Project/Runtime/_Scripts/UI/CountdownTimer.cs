#region
using System;
using Lumina.Essentials.Attributes;
using TMPro;
using UnityEngine;
#endregion

public class CountdownTimer : MonoBehaviour
{
    public delegate void TimerEnded();
    [Header("Reference")] [Space(10)] [ReadOnly]
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] [ReadOnly] float currentTime;
    [SerializeField] [ReadOnly] bool finished;
    [Tooltip("If true, the timer will count down instead of up.")]
    [SerializeField] bool countDown;

    [Header("Limit Settings")] [Tooltip("If true, the timer will have a time limit.")]
    [SerializeField] bool hasTimeLimit;
    [SerializeField] float timeLimit;

    [Header("Format Settings")]
    [SerializeField] [Tooltip("If true, the timer will use a custom format.")]
    bool customFormat;
    [SerializeField] [Tooltip("The value at which the timer will switch to a red color.")]
    float colorSwitchValue;
    [SerializeField] [Tooltip("The value at which the timer will switch to a whole number format.")]
    float tenthSwitchValue;
    [SerializeField] [Tooltip("The value at which the timer will switch to a tenth decimal format.")]
    float hundredthsSwitchValue;

    // -- Properties --
    public static CountdownTimer Instance { get; private set; }
    public TimeSpan Time
    {
        get => TimeSpan.FromSeconds(currentTime);
        set => currentTime = (float) value.TotalSeconds;
    }

    public bool Finished
    {
        get => finished;
        set => finished = value;
    }

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() => Finished = false;

    void Update()
    {
        if (timerText == null || Finished) return;

        if (countDown) DecreaseTime(UnityEngine.Time.deltaTime);
        else IncreaseTime(UnityEngine.Time.deltaTime);

        UpdateTimerText();
    }

    void OnEnable() => OnTimerEnded += TimerFinishedEvent;

    void OnDisable() => OnTimerEnded -= TimerFinishedEvent;

    public static event TimerEnded OnTimerEnded;

    public void AddTime(float time) => currentTime += 60;

    void TimerFinishedEvent()
    {
        Finished = true;
        Debug.Log("Timer has ended!");

        TimerFinished();

        return;

        void TimerFinished()
        {
            // do timer finished stuff
            // e.g. show game over screen
        }
    }

    void UpdateTimerText()
    {
        // Format the time as MM:SS
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime % 60F);
        timerText.text = $"{minutes:00}:{seconds:00}";

        // Set the timer color
        Color defaultColor = timerText.color;

        if (customFormat) // If the custom format is enabled, use the color switch values.
        {
            if (countDown) timerText.color = currentTime <= colorSwitchValue ? Color.red : defaultColor;
            else timerText.color = currentTime >= colorSwitchValue ? Color.red : defaultColor;
        }

        switch (countDown)
        {
            // If the timer has finished, invoke the OnTimerEnded event
            case true: {
                if (currentTime <= 0.0f && !Finished) OnTimerEnded?.Invoke();
                break;
            }

            default: {
                if (currentTime >= timeLimit && !Finished) OnTimerEnded?.Invoke();
                break;
            }
        }
    }

    public void StopTimer()
    {
        Finished = true;
        OnTimerEnded?.Invoke();
        Debug.Log("Timer has been stopped.");
    }

    #region Timer Methods
    void IncreaseTime(float delta)
    {
        currentTime += delta;
        if (hasTimeLimit && currentTime > timeLimit) currentTime = timeLimit;
    }

    void DecreaseTime(float delta)
    {
        currentTime -= delta;
        if (currentTime < 0.0f) currentTime = 0.0f;
        if (hasTimeLimit && currentTime < timeLimit) currentTime = timeLimit;
    }

    public void SetTimer(float newTime) => currentTime = newTime;

    public void ResetTimer() => currentTime = timeLimit;
    #endregion
}
