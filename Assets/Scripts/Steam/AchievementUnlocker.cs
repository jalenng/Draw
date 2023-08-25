using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

// NOTE: 
// - Do not remove achievements or reorder them
// - Only add them to the bottom of the enum
// - Otherwise, you will mess up the other references to the enums

public class AchievementUnlocker : MonoBehaviour
{
    [SerializeField] private Global.Achievement achievement;

    [ContextMenu("Set Achievement")]
    public void SetAchievement()
    {
        if (achievement == Global.Achievement.NONE) {
            Debug.Log("Tried to set achievement but none was specified");
            return;
        }

        if (!SteamManager.Initialized) {
            Debug.Log($"Tried to set achievement but SteamManager is not initialized");
            return;
        }

        // Look up achievement ID
        if (!Global.AchievementToIdMap.TryGetValue(achievement, out string achievementId)) {
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
