using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TimeManager
{
    private static List<(DateTime, DateTime)> pausedPairs = new List<(DateTime, DateTime)>();
    private static List<TimeSpan> pausedTimes = new List<TimeSpan>();

    private static DateTime? lastPausedTime = null;

    private readonly static DateTime startTime = DateTime.Now;

    public static DateTime StartTime => startTime;

    public static void Setup()
    {
        TriggerManager.PauseTimeTrigger.AddTrigger(OnPausedTime);
        TriggerManager.ResumeTimeTrigger.AddTrigger(OnResumedTime);
    }

    private static int OnPausedTime(int _)
    {
        lastPausedTime = DateTime.Now;

        return _;
    }

    private static int OnResumedTime(int _)
    {
        pausedPairs.Add(((DateTime)lastPausedTime, DateTime.Now));
        pausedTimes.Add((DateTime.Now - (DateTime)lastPausedTime));

        lastPausedTime = null;

        return _;
    }

    public static TimeSpan GetElapsedTime()
    {
        TimeSpan elapsedTime = GetElapsedTimeSince(startTime);

        // need to subtract off the ending time as the game may be currently paused
        if (lastPausedTime is not null)
        {
            elapsedTime -= (DateTime.Now - (DateTime)lastPausedTime);
        }

        return elapsedTime;
    }

    public static TimeSpan GetElapsedTimeSince(DateTime time)
    {
        // time should not be inbetween any of the pairs

        TimeSpan elapsedTimeSpan = DateTime.Now - time;

        // if the game time has not been paused
        if (pausedPairs.Count == 0 || time >= pausedPairs[pausedPairs.Count - 1].Item2)
        {
            return elapsedTimeSpan;
        }

        if (time <= pausedPairs[0].Item1)
        {
            return elapsedTimeSpan - new TimeSpan(pausedTimes.Sum(x => x.Ticks));
        }

        // binary search to find inbetween which pairs this time is
        (int, int) range = (0, pausedPairs.Count - 1);

        while ((range.Item2 - range.Item1) > 1)
        {
            // grabs the middle of the range
            int midPoint = (range.Item1 + range.Item2) / 2;

            (DateTime, DateTime) dateRange = pausedPairs[midPoint];

            if (time <= dateRange.Item1)
            {
                // time is before the range
                range.Item2 = midPoint;
            }
            else if (time >= dateRange.Item2)
            {
                // time is after the range
                range.Item1 = midPoint;
            }
            else
            {
                // time is inbetween the range
                throw new Exception("Time passed was when the game time was paused");
            }
        }

        // grabs all the pausedTimes between the end of the range and the end of the list
        for (int i = range.Item2; i < pausedTimes.Count; i++)
        {
            elapsedTimeSpan -= pausedTimes[i];
        }

        return elapsedTimeSpan;
    }
}
