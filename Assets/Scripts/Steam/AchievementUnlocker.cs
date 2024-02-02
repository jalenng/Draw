#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !DISABLESTEAMWORKS
using Steamworks;
#endif 

public class AchievementUnlocker : MonoBehaviour
{
    [SerializeField] private Global.Achievement achievement;

    [ContextMenu("Set Achievement")]
    public void SetAchievement()
    {
        if (achievement == Global.Achievement.NONE)
        {
            Debug.LogError("[AchievementUnlocker] Tried to set achievement but none was specified");
            return;
        }

        if (!SteamManager.Initialized)
        {
            Debug.LogWarning($"[AchievementUnlocker] Tried to set achievement but SteamManager is not initialized");
            return;
        }

        // Look up achievement ID
        if (!Global.AchievementToIdMap.TryGetValue(achievement, out string achievementId))
        {
            Debug.LogError("[AchievementUnlocker] Tried to set achievement but could not lookup achievement ID");
            return;
        }

#if !DISABLESTEAMWORKS
        // See if achievement is already unlocked
        Steamworks.SteamUserStats.GetAchievement(achievementId, out bool achievementUnlocked);

        // If not, unlock it
        if (achievementUnlocked)
        {
            Debug.Log($"[AchievementUnlocker] Tried to set achievement \"{achievementId}\" but it has already been unlocked");
        }
        else
        {
            Debug.Log($"[AchievementUnlocker] Unlocking achievement \"{achievementId}\"");
            SteamUserStats.SetAchievement(achievementId);
            SteamUserStats.StoreStats();
        }
#endif // !DISABLESTEAMWORKS
    }
}