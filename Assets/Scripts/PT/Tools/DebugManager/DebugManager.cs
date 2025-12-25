using UnityEngine;
using System;
using UnityEditor;

#region enums

[Flags]
public enum DebugCategory
{
    None = 0,
    Points = 1 << 0,
    Errors = 1 << 1,
    UI = 1 << 2,
    Gameplay = 1 << 3,
    WebView = 1 << 4,
    Config = 1 << 5,
    Observer = 1 << 6,
    Misc = 1 << 7,
    Input = 1 << 8,
    
    All = ~0
}
#endregion

public static class DebugManager
{
    private static DebugCategory _activeCategories;

    private const string _debugPrefsKey = "DebugManagerCategories";

    static DebugManager()
    {
#if UNITY_EDITOR
        LoadDebugCategories();
#else 
        _activeCategories = DebugCategory.All; 
        // Debug.unityLogger.logEnabled = false;
#endif
    }

    public static void EnableDebug(DebugCategory category)
    {
        _activeCategories |= category;
    }

    public static void DisableDebug(DebugCategory category)
    {
        _activeCategories &= ~category;
    }

    public static bool IsDebugEnabled(DebugCategory category) => _activeCategories.HasFlag(category);

    public static void Log(DebugCategory category, string message, LogType logType = LogType.Log)
    {
        if (category == DebugCategory.None) return;

        if (IsDebugEnabled(category))
        {
            var log = $"[{category}] {message}";

            switch (logType)
            {
                case LogType.Log: Debug.Log("<color=white:>log</color> " + log); break;
                case LogType.Assert: Debug.Log("<color=pink>assert:</color> " + log + "-->"); break;
                case LogType.Warning: Debug.LogWarning("<color=yellow>WARNING:</color> " + log); break;
                case LogType.Error: Debug.LogError("<color=red>ERROR:</color> " + log); break;
            }
        }
    }

    #region save, load, editor
#if UNITY_EDITOR
    public static void SaveDebugCategories()
    {
        EditorPrefs.SetInt(_debugPrefsKey, (int)_activeCategories);
    }
    private static void LoadDebugCategories()
    {
        _activeCategories = EditorPrefs.HasKey(_debugPrefsKey) ?
            (DebugCategory)EditorPrefs.GetInt(_debugPrefsKey) :
             DebugCategory.None;
    }
#endif
    #endregion
}
