using System.Text.Json;

namespace ServerlessApi.Utils;

public static class Logger
{
    private static readonly Dictionary<string, int> LogLevels = new()
    {
        { "debug", 0 },
        { "info", 1 },
        { "warn", 2 },
        { "error", 3 }
    };

    private static int CurrentLevel
    {
        get
        {
            var level = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "info";
            return LogLevels.GetValueOrDefault(level, 1);
        }
    }

    private static void Log(string level, string message, Dictionary<string, object?>? data = null)
    {
        if (LogLevels.GetValueOrDefault(level, 1) < CurrentLevel) return;

        var entry = new Dictionary<string, object?>
        {
            { "timestamp", DateTime.UtcNow.ToString("o") },
            { "level", level },
            { "message", message },
            { "stage", Environment.GetEnvironmentVariable("STAGE") ?? "unknown" }
        };

        if (data != null)
        {
            foreach (var kvp in data)
                entry[kvp.Key] = kvp.Value;
        }

        var output = JsonSerializer.Serialize(entry);

        if (level == "error")
            Console.Error.WriteLine(output);
        else
            Console.WriteLine(output);
    }

    public static void Debug(string msg, Dictionary<string, object?>? data = null) => Log("debug", msg, data);
    public static void Info(string msg, Dictionary<string, object?>? data = null) => Log("info", msg, data);
    public static void Warn(string msg, Dictionary<string, object?>? data = null) => Log("warn", msg, data);
    public static void Error(string msg, Dictionary<string, object?>? data = null) => Log("error", msg, data);
}
