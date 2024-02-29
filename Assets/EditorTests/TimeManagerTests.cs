using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimeManagerTests
{
    [UnityTest]
    public IEnumerator ElapsedTimeTest()
    {
        TimeManager.Setup();

        yield return null;

        TimeManager.OnPausedTime(0);
        
        yield return null;

        TimeManager.OnResumedTime(0);

        yield return null;

        Assert.AreEqual(0.02f, TimeManager.GetElapsedTime().Seconds, 0.001f);

    }
}