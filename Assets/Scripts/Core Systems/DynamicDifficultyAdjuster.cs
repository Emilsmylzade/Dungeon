using UnityEngine;

public class DynamicDifficultyAdjuster : MonoBehaviour
{
    void Update()
    {
        // Example usage of GetCurrentProgressionLevel
        int progressionLevel = GameManagerScript.Instance.GetCurrentProgressionLevel();
        AdjustDifficulty(progressionLevel);
    }

    private void AdjustDifficulty(int progressionLevel)
    {
        // Implement your dynamic difficulty logic based on progressionLevel
        // For example:
        if (progressionLevel > 10)
        {
            // Increase difficulty
        }
        else
        {
            // Maintain or decrease difficulty
        }
    }
}
