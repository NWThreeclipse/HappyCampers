using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEditor.Networking.PlayerConnection;
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Day Settings")]
    public int currentDay = 1;
    private const float realSecondsPerDay = 900f;
    private const int hoursPerDay = 15;

    [Header("Phases")]
    public DayPhase currentPhase;
    public enum DayPhase { Afternoon, Evening, Night }

    public PlayerController player;
    [Header("UI Displays")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI pointDisplay;

    [Header("Lighting")]
    [SerializeField] private Light2D sunLight;
    [SerializeField] private Color dayColour;
    [SerializeField] private Color nightColour;

    private float totalTime; // total time in seconds
    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }

    private bool dayEnded = false;

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

    void FixedUpdate()
    {
        // If day ended, freeze time
        if (dayEnded)
            return;

        totalTime += Time.deltaTime;

        // End day when time reaches cutoff
        if (totalTime >= realSecondsPerDay)
        {
            DayEnd();
            return;
        }

        // Calculate current hour and minute from total time
        float dayProgress = totalTime / realSecondsPerDay;
        float totalGameHours = dayProgress * hoursPerDay;

        CurrentHour = Mathf.FloorToInt(totalGameHours);
        CurrentMinute = Mathf.FloorToInt((totalGameHours - CurrentHour) * 60f);

        sunLight.color = Color.Lerp(dayColour, nightColour, dayProgress);

        UpdateDayPhase();
        UpdateUI();
    }

    private void UpdateUI()
    {
        int displayHour = (CurrentHour % 12 == 0) ? 12 : CurrentHour % 12;
        string clockTime = displayHour + ":" + CurrentMinute.ToString("00");

        timeText.text = clockTime;
        phaseText.text = currentPhase.ToString();
        dayText.text = "Day: " + currentDay;
    }

    private void UpdateDayPhase()
    {
        if (CurrentHour < 5)
            currentPhase = DayPhase.Afternoon;
        else if (CurrentHour < 10)
            currentPhase = DayPhase.Evening;
        else
            currentPhase = DayPhase.Night;
    }

    public void SkipToNextDay()
    {
        NewDay();
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
            DayEnd();
        }
    }

    public void DayEnd()
    {
        dayEnded = true;

        // Show the end panel and update the text
        endPanel.SetActive(true);
        if (pointDisplay != null)
        {
            pointDisplay.text = "Score: " + player.CamperPoints;
        }
    }

    public void NewDay()
    {
        dayEnded = false;
        totalTime = 0f;
        currentDay++;

        // Hide the panel and refresh UI
        endPanel.SetActive(false);
        UpdateUI();
    }
}
