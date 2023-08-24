using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

// NOTE: 
// - Do not remove achievements or reorder them
// - Only add them to the bottom of the enum
// - Otherwise, you will mess up the other references to the enums
public enum Achievement
{
    NONE,
    MEET_MIKE,
    MEET_CLAIRE,
    FINISH_GAME,
    DRAW_IN_CANVAS,
    TOUCH_ORANGE_OBJ
}

public class AchievementUnlocker : MonoBehaviour
{
    [SerializeField] private Achievement achievement;

    public static Dictionary<Achievement, string> AchievementIds = new Dictionary<Achievement, string>()
    {
        { Achievement.MEET_MIKE, "MEET_MIKE"},
        { Achievement.MEET_CLAIRE, "MEET_CLAIRE"},
        { Achievement.FINISH_GAME, "FINISH_GAME"},
        { Achievement.DRAW_IN_CANVAS, "DRAW_IN_CANVAS"},
        { Achievement.TOUCH_ORANGE_OBJ, "TOUCH_ORANGE_OBJ"},
    };

    [ContextMenu("Set Achievement")]
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
