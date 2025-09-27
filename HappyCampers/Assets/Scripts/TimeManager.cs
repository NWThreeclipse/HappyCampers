using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Day Settings")]
    public float realSecondsPerDay = 900f;
    public int hoursPerDay = 15;
    public int currentDay = 1;

    [Header("Phases")]
    public DayPhase currentPhase;
    public enum DayPhase { Afternoon, Evening, Night }

    [Header("UI Displays")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private TextMeshProUGUI dayText;

    private float totalTime; // total time in seconds
    private int currentHour;
    private int currentMinute;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        totalTime += Time.deltaTime;

        // Reset if we passed one whole day
        if (totalTime >= realSecondsPerDay)
        {
            totalTime = 0f;
            currentDay++;
        }

        // Calculate current hour and minute from total time
        float dayProgress = totalTime / realSecondsPerDay;
        float totalGameHours = dayProgress * hoursPerDay;

        currentHour = Mathf.FloorToInt(totalGameHours);
        currentMinute = Mathf.FloorToInt((totalGameHours - currentHour) * 60f);

        UpdateDayPhase();
        UpdateUI();
    }

    private void UpdateUI()
    {
        int displayHour = (currentHour % 12 == 0) ? 12 : currentHour % 12;
        string clockTime = displayHour + ":" + currentMinute.ToString("00");

        timeText.text = clockTime;
        phaseText.text = currentPhase.ToString();
        dayText.text = "Day: " + currentDay;
    }

    private void UpdateDayPhase()
    {
        if (currentHour < 5)
            currentPhase = DayPhase.Afternoon;
        else if (currentHour < 10)
            currentPhase = DayPhase.Evening;
        else
            currentPhase = DayPhase.Night;
    }

    public void SkipToNextDay()
    {
        totalTime = 0f;
        currentDay++;
    }
    public void SkipHour()
    {
        // Calculate how many real seconds equal one in game hour
        float secondsPerHour = realSecondsPerDay / hoursPerDay;

        // Snap to next whole hour
        float hoursPassed = Mathf.Floor(totalTime / secondsPerHour) + 1;
        totalTime = hoursPassed * secondsPerHour;

        // ends day if past cutoff
        if (totalTime >= realSecondsPerDay)
        {
            totalTime -= realSecondsPerDay;
            currentDay++;
        }
    }
}
