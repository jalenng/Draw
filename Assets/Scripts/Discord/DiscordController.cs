#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLEDISCORD
#endif

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
#if !DISABLEDISCORD
using Discord;
#endif

public class DiscordController : MonoBehaviour
{

#if !DISABLEDISCORD
    public Discord.Discord discord;
    private bool alreadyLoggedInitError = false;

    void Start()
    {
        TryToInitDiscord();
    }

    void TryToInitDiscord()
    {
        try
        {
            discord = new Discord.Discord(1142718120876834889, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
            var activityManager = discord.GetActivityManager();

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int startTime = (int)t.TotalSeconds;

            var activity = new Discord.Activity
            {
                State = "",
                Details = "",
                Assets =
            {
                LargeImage = "logo",
            },
                Timestamps = {
                Start = startTime,
            }
            };
            activityManager.RegisterSteam(2473980);
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Debug.Log("[DiscordController] Update activity callback registered");
                }
                else
                {
                    Debug.LogError("[DiscordController] Failed to update register activity callback");
                }
            });

            alreadyLoggedInitError = false;
            Debug.Log("[DiscordController] Initialized successfully");
        }
        catch (Discord.ResultException e)
        {
            if (!alreadyLoggedInitError)
            {
                Debug.LogError($"[DiscordController] Failed to initialize Discord instance: {e}");
            }
            alreadyLoggedInitError = true;
            DestroyDiscord();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (discord == null)
        {
            TryToInitDiscord();
        }

        try
        {
            discord?.RunCallbacks();
        }
        catch (Discord.ResultException e)
        {
            Debug.Log($"[DiscordController] Encountered error running callbacks: {e}");
            DestroyDiscord();
        }
    }

    void OnApplicationQuit()
    {
        if (discord == null) return;

        DestroyDiscord();
    }

    void DestroyDiscord()
    {
        if (discord == null) return;

        discord.Dispose();
        discord = null;
        Debug.Log("[DiscordController] Discord instance destroyed");
    }
#endif
}