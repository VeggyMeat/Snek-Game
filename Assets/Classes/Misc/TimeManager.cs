using System;
using System.Collections.Generic;
using System.Linq;

// COMPLETE

/// <summary>
/// The time manager is used to keep track of the time in the game
/// </summary>
public static class TimeManager
{
    /// <summary>
    /// The pairs of time between which the game was paused
    /// </summary>
    private static List<(DateTime, DateTime)> pausedPairs = new List<(DateTime, DateTime)>();
    
    /// <summary>
    /// The list of times that the game was paused, matching the paused pairs
    /// </summary>
    private static List<TimeSpan> pausedTimes = new List<TimeSpan>();

    /// <summary>
    /// The last time the game was paused
    /// </summary>
    private static DateTime? lastPausedTime = DateTime.Now;

    /// <summary>
    /// The time the game started
    /// </summary>
    private readonly static DateTime startTime = DateTime.Now;

    /// <summary>
    /// The time the game started
    /// </summary>
    public static DateTime StartTime => startTime;

    /// <summary>
    /// Called to setup the time manager
    /// </summary>
    public static void Setup()
    {
        TriggerManager.PauseTimeTrigger.AddTrigger(OnPausedTime);
        TriggerManager.ResumeTimeTrigger.AddTrigger(OnResumedTime);
    }

    /// <summary>
    /// Called when time is paused
    /// </summary>
    /// <param name="_">Redundant value</param>
    /// <returns>Redundant value</returns>
    internal static int OnPausedTime(int _)
    {
        lastPausedTime = DateTime.Now;

        return _;
    }

    /// <summary>
    /// Called when time is resumed
    /// </summary>
    /// <param name="_">Redundant value</param>
    /// <returns>Redundant value</returns>
    internal static int OnResumedTime(int _)
    {
        // adds the time the game was paused to the list
        pausedPairs.Add(((DateTime)lastPausedTime, DateTime.Now));
        pausedTimes.Add(DateTime.Now - (DateTime)lastPausedTime);

        lastPausedTime = null;

        return _;
    }

    /// <summary>
    /// Gets the elapsed (game) time since the game started
    /// </summary>
    /// <returns>The elapsed (game) time since the game started</returns>
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

    /// <summary>
    /// Gets the elapsed (game) time since a given time
    /// </summary>
    /// <param name="time">The given time since</param>
    /// <returns>The elapsed time</returns>
    /// <exception cref="Exception">Thrown if the 'time' is a paused time</exception>
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
