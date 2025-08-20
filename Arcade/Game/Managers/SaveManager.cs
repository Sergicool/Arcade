using Godot;
using System;

public static class SaveManager
{
    private static string savePath = "user://savegame.cfg";
    
    // Stats
    public static string breakoutHighScore = "breakout_highscore";

    public static void SaveStat(string statName, int value)
    {
        var config = new ConfigFile();
        config.Load(savePath);

        config.SetValue("stats", statName, value);
        config.Save(savePath);
    }

    public static int LoadStat(string statName)
    {
        var config = new ConfigFile();
        var err = config.Load(savePath);

        if (err == Error.Ok)
            return (int)config.GetValue("stats", statName, 0);
        else
            return 0;
    }

    public static void SaveSetting(string key, Variant value)
    {
        var config = new ConfigFile();
        config.Load(savePath);

        config.SetValue("settings", key, value);
        config.Save(savePath);
    }

    public static Variant LoadSetting(string key, Variant defaultValue)
    {
        var config = new ConfigFile();
        var err = config.Load(savePath);

        if (err == Error.Ok)
            return config.GetValue("settings", key, defaultValue);
        else
            return defaultValue;
    }
}
