using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchievementUnlocker : MonoBehaviour
{
    [SerializeField] private Global.Achievement achievement;

    [ContextMenu("Set Achievement")]
    public void SetAchievement()
    {
        if (achievement == Global.Achievement.NONE) {
            Debug.LogError("[AchievementUnlocker] Tried to set achievement but none was specified");
            return;
        }

        if (!SteamManager.Initialized) {
            Debug.LogWarning($"[AchievementUnlocker] Tried to set achievement but SteamManager is not initialized");
            return;
        }

        // Look up achievement ID
        if (!Global.AchievementToIdMap.TryGetValue(achievement, out string achievementId)) {
            Debug.LogError("[AchievementUnlocker] Tried to set achievement but could not lookup achievement ID");
            return;
        }

        // See if achievement is already unlocked
        Steamworks.SteamUserStats.GetAchievement(achievementId, out bool achievementUnlocked);

        // If not, unlock it
        if (!achievementUnlocked) {
            Debug.Log($"[AchievementUnlocker] Unlocking achievement \"{achievementId}\"");
            SteamUserStats.SetAchievement(achievementId);
            SteamUserStats.StoreStats();
        }
    }
}
