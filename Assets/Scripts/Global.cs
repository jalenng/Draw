using System.Collections;
using System.Collections.Generic;

// NOTE aboout Enums: 
// - Do not remove achievements or reorder them
// - Only add them to the bottom of each enum group
// - Otherwise, you may mess up existing references to the enums

public class Global
{
    // Enums
    public enum UIScene
    {
        NONE,
        MAIN_MENU,
        CONTENT_WARNING
    }
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
    public static Dictionary<UIScene, int> UISceneToBuildIndexMap = new Dictionary<UIScene, int>()
    {
        { UIScene.CONTENT_WARNING, 0},
        { UIScene.MAIN_MENU, 1},
    };

    public static Dictionary<Level, int> LevelToBuildIndexMap = new Dictionary<Level, int>()
    {
        { Level.IRL_DRAWING, 2},
        { Level.TUTORIAL, 3},
        { Level.CLAIRE_1, 4},
        { Level.IRL_CLAIRE_RUNS_INTO_NATHAN, 5},
        { Level.CLAIRE_2, 6},
        { Level.IRL_CLAIRE_CUTOFF, 7},
        { Level.MIKE_1, 8},
        { Level.MIKE_2, 9},
        { Level.IRL_MIKE_CUTOFF, 10},
        { Level.MIKE_3, 11},
        { Level.CHASE, 12},
        { Level.NATHAN_1, 13},
        { Level.NATHAN_2, 14},
        { Level.NATHAN_3, 15},
        { Level.CREDITS, 16},
        { Level.THE_END, 17},
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
    public static Global.UIScene GetUISceneFromBuildIndex(int buildIndex)
    {
        return GetKeyFromValue<Global.UIScene, int>(
            Global.UISceneToBuildIndexMap,
            buildIndex,
            Global.UIScene.NONE
        );
    }
    public static Global.Level GetLevelFromBuildIndex(int buildIndex)
    {
        return GetKeyFromValue<Global.Level, int>(
            Global.LevelToBuildIndexMap,
            buildIndex,
            Global.Level.NONE
        );
    }

    // Helper functions
    private static K GetKeyFromValue<K, V>(Dictionary<K, V> map, V value, K fallback)
    {
        foreach (KeyValuePair<K, V> pair in map)
        {
            if (pair.Value.Equals(value))
            {
                return pair.Key;
            }
        }
        return fallback;
    }
}