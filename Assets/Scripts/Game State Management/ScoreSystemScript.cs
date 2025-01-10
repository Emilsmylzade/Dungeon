
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystemScript : MonoBehaviour
{
    // Variables to track player performance metrics
    public int totalScore;
    private float damageTaken;
    private float timeSpentInRoom;
    private float roomStartTime;

    // UI elements to display score
    public Text scoreText;
    public Text damageText;
    public Text timeText;

    public static ScoreSystemScript Instance { get; private set; }
    void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        ResetMetrics();
        roomStartTime = Time.time; // Start timing when the room is entered
    }

    // Update is called once per frame
    void Update()
    {
        // Update time spent in the current room
        timeSpentInRoom = Time.time - roomStartTime;
    }

    // Call this method to record damage taken by the player
    public void RecordDamage(float damage)
    {
        damageTaken += damage;
        UpdateScore(); // Update score based on damage taken
    }

    // Call this method when the player clears the room
    public void RoomCleared()
    {
        CalculateScore();
        DisplayScore();
        ResetMetrics(); // Reset metrics for the next room
    }

    // Calculate score based on performance metrics
    private void CalculateScore()
    {
        // Example scoring logic: higher score for less damage taken and faster completion
        int scoreFromDamage = Mathf.Max(0, 100 - (int)(damageTaken * 10)); // Penalty for damage taken
        int scoreFromTime = Mathf.Max(0, 100 - (int)(timeSpentInRoom)); // Penalty for time spent
        totalScore += scoreFromDamage + scoreFromTime; // Total score calculation
    }

    // Update the score display
    private void DisplayScore()
    {
        scoreText.text = "Score: " + totalScore;
        damageText.text = "Damage Taken: " + damageTaken;
        timeText.text = "Time Spent: " + timeSpentInRoom.ToString("F2") + "s";
    }

    // Reset metrics for the next room
    private void ResetMetrics()
    {
        damageTaken = 0;
        timeSpentInRoom = 0;
        roomStartTime = Time.time; // Reset the timer for the new room
    }

    // Update score based on current metrics (optional)
    private void UpdateScore()
    {
        // Update the score display in real-time
        DisplayScore();
    }
}
