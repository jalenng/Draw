using System.Collections;
using System.Collections.Generic;

// NOTE aboout Enums: 
// - Do not remove achievements or reorder them
// - Only add them to the bottom of each enum group
// - Otherwise, you may mess up existing references to the enums

public class Global
{
    // Enums
    public enum Level
    {
        NONE,
        IRL_DRAWING,
        TUTORIAL,
        CLAIRE_1,
        IRL_CLAIRE_RUNS_INTO_NATHAN,
        CLAIRE_2,
        IRL_CLAIRE_CUTOFF,
        MIKE_1,
        MIKE_2,
        IRL_MIKE_CUTOFF,
        MIKE_3,
        CHASE,
        NATHAN_1,
        NATHAN_2,
        NATHAN_3,
        CREDITS,
        THE_END
    }
    public enum Achievement
    {
        NONE,
        MEET_MIKE,
        MEET_CLAIRE,
        FINISH_GAME,
        DRAW_IN_CANVAS,
        TOUCH_ORANGE_OBJ,
        DRAW_CHAIR
    }

    // Mappings
    public static Dictionary<Level, int> LevelToBuildIndexMap = new Dictionary<Level, int>()
    {
        { Level.IRL_DRAWING, 1},
        { Level.TUTORIAL, 2},
        { Level.CLAIRE_1, 3},
        { Level.IRL_CLAIRE_RUNS_INTO_NATHAN, 4},
        { Level.CLAIRE_2, 5},
        { Level.IRL_CLAIRE_CUTOFF, 6},
        { Level.MIKE_1, 7},
        { Level.MIKE_2, 8},
        { Level.IRL_MIKE_CUTOFF, 9},
        { Level.MIKE_3, 10},
        { Level.CHASE, 11},
        { Level.NATHAN_1, 12},
        { Level.NATHAN_2, 13},
        { Level.NATHAN_3, 14},
        { Level.CREDITS, 15},
        { Level.THE_END, 16},
    };

    public static Dictionary<Achievement, string> AchievementToIdMap = new Dictionary<Achievement, string>()
    {
        { Achievement.MEET_MIKE, "MEET_MIKE"},
        { Achievement.MEET_CLAIRE, "MEET_CLAIRE"},
        { Achievement.FINISH_GAME, "FINISH_GAME"},
        { Achievement.DRAW_IN_CANVAS, "DRAW_IN_CANVAS"},
        { Achievement.TOUCH_ORANGE_OBJ, "TOUCH_ORANGE_OBJ"},
        { Achievement.DRAW_CHAIR, "DRAW_CHAIR"},
    };

    // Methods
    public static Global.Level GetLevelFromBuildIndex(int buildIndex)
    {
        // Try to see which level this scene is
        foreach (KeyValuePair<Global.Level, int> pair in Global.LevelToBuildIndexMap)
        {
            // If found, set it as a reached level
            if (pair.Value == buildIndex)
            {
                return pair.Key;
            }
        }
        return Global.Level.NONE;
    }
}