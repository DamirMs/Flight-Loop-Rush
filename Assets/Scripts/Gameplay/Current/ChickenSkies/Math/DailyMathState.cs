using System;
using UnityEngine;

namespace Gameplay.Current.ChickenSkies.Math
{
    public static class DailyMathState
    {
        private const string Key = "DailyMathSolvedDate";

        public static bool IsSolvedToday()
        {
            return PlayerPrefs.GetString(Key, "") == TodayKey();
        }

        public static void MarkSolved()
        {
            PlayerPrefs.SetString(Key, TodayKey());
        }

        private static string TodayKey()
        {
            var d = DateTime.UtcNow;
            return $"{d.Year}-{d.Month}-{d.Day}";
        }
    }
}