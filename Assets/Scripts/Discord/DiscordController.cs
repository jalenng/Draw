using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{

    public Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(1142718120876834889, (System.UInt64)Discord.CreateFlags.Default);
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
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Discord activity updated successfully");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }
}