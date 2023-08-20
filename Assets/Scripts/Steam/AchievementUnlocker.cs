using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public enum Achievement
{
    NONE,
    MEET_MIKE,
    MEET_CLAIRE,
    FINISH_GAME
}

public class AchievementUnlocker : MonoBehaviour
{
    [SerializeField] private Achievement achievement;

    public static Dictionary<Achievement, string> AchievementIds = new Dictionary<Achievement, string>()
    {
        { Achievement.MEET_MIKE, "MEET_MIKE"},
        { Achievement.MEET_CLAIRE, "MEET_CLAIRE"},
        { Achievement.FINISH_GAME, "FINISH_GAME"},
    };

    public void SetAchievement()
    {
        if (achievement == Achievement.NONE) {
            Debug.Log("Tried to set achievement but none was specified");
            return;
        }

        if (!SteamManager.Initialized) {
            Debug.Log($"Tried to set achievement but SteamManager is not initialized");
            return;
        }

        // Look up achievement ID
        if (!AchievementIds.TryGetValue(achievement, out string achievementId)) {
            Debug.Log("Tried to set achievement but could not lookup achievement ID");
            return;
        }

        // See if achievement is already unlocked
        Steamworks.SteamUserStats.GetAchievement(achievementId, out bool achievementUnlocked);

        // If not, unlock it
        if (!achievementUnlocked) {
            Debug.Log($"Unlocking achievement \"{achievementId}\"");
            SteamUserStats.SetAchievement(achievementId);
            SteamUserStats.StoreStats();
        }
    }
}
