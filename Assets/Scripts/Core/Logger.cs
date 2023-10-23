using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public enum LogLevel
    {
        Info, Warning, Error
    }

    public static void Log(object msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }

    public static void Log(object msg, LogLevel level)
    {
#if UNITY_EDITOR
        switch (level)
        {
            case LogLevel.Info:
                Debug.Log(msg);
                break;
            case LogLevel.Warning:
                Debug.LogWarning(msg);
                break;
            case LogLevel.Error:
                Debug.LogError(msg);
                break;
        }
#endif
    }

    public static void Log(object msg, LogLevel level, Object ctx)
    {
#if UNITY_EDITOR
        switch (level)
        {
            case LogLevel.Info:
                Debug.Log(msg, ctx);
                break;
            case LogLevel.Warning:
                Debug.LogWarning(msg, ctx);
                break;
            case LogLevel.Error:
                Debug.LogError(msg, ctx);
                break;
        }
#endif
    }
}
