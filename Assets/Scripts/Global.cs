using System.Collections;
using System.Collections.Generic;


public class Global
{
    // Enums
    public enum Level
    {
        TUTORIAL,
        CLAIRE_1,
        CLAIRE_2,
        MIKE_1,
        MIKE_2,
        MIKE_3,
        CHASE,
        NATHAN_1,
        NATHAN_2,
        NATHAN_3
    }
    public enum Achievement
    {
        NONE,
        MEET_MIKE,
        MEET_CLAIRE,
        FINISH_GAME,
        DRAW_IN_CANVAS,
        TOUCH_ORANGE_OBJ
    }

    // Mappings
    public static Dictionary<Level, int> levelToBuildIndexMap = new Dictionary<Level, int>()
    {
        { Level.TUTORIAL, 2},
        { Level.CLAIRE_1, 3},
        { Level.CLAIRE_2, 5},
        { Level.MIKE_1, 7},
        { Level.MIKE_2, 8},
        // { Level.MIKE_3, 10},
        { Level.CHASE, 10},
        { Level.NATHAN_1, 11},
        { Level.NATHAN_2, 12},
        { Level.NATHAN_3, 13},
    };
    
    public static Dictionary<Achievement, string> AchievementToIdMap = new Dictionary<Achievement, string>()
    {
        { Achievement.MEET_MIKE, "MEET_MIKE"},
        { Achievement.MEET_CLAIRE, "MEET_CLAIRE"},
        { Achievement.FINISH_GAME, "FINISH_GAME"},
        { Achievement.DRAW_IN_CANVAS, "DRAW_IN_CANVAS"},
        { Achievement.TOUCH_ORANGE_OBJ, "TOUCH_ORANGE_OBJ"},
    };
}