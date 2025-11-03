using UnityEngine;

public static class DebugUtil
{
    
#if UNITY_EDITOR
    public static bool EnableLogs = true;
#else
    public static bool EnableLogs = false;
#endif

    #region >--------------------------------------------------- CORE

    
    private static string FormatMessage(string emoji, string tag, string message)
    {
        return $"[{emoji} {tag}] {message}";
    }
    
    
    #endregion
    #region >--------------------------------------------------- LOG

    
    public static void Log(string message, string tag = "")
    {
        if (!EnableLogs) return;
        Debug.Log(FormatMessage("ℹ️", tag, message));
    }
    
    public static void Log(string message)
    {
        if (!EnableLogs) return;
        Debug.Log(message);
    }

    
    public static void LogWarning(string message, string tag = "")
    {
        if (!EnableLogs) return;
        Debug.LogWarning(FormatMessage("⚠️", tag, message));
    }
    
    public static void LogWarning(string message)
    {
        if (!EnableLogs) return;
        Debug.LogWarning(message);
    }

    
    public static void LogError(string message, string tag = "")
    {
        if (!EnableLogs) return;
        Debug.LogError(FormatMessage("❌", tag, message));
    }
    
    public static void LogError(string message)
    {
        if (!EnableLogs) return;
        Debug.LogError(message);
    }
    
    #endregion


}