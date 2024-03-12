using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DatabaseTests
{
    [Test]
    public void DatabaseTestsSimplePasses()
    {
        DatabaseHandler.Setup();
        Run run = new Run
        {
            PlayerName = "Test",
            Score = 100,
            Time = 100,
            Date = System.DateTime.Now
        };

        List<ItemInfo> items = new List<ItemInfo>
        {
            new ItemInfo
            {
                Name = "TestItem",
                Level = 1
            },
            new ItemInfo
            {
                Name = "TestItem2",
                Level = 2
            },
        };

        List<BodyInfo> bodies = new List<BodyInfo>
        {
            new BodyInfo
            {
                Name = "TestBody",
                Level = 1
            },
            new BodyInfo
            {
                Name = "TestBody2",
                Level = 2
            },
            new BodyInfo
            {
                Name = "TestBody3",
                Level = 3
            }
        };

        DatabaseHandler.AddRun(run, items, bodies);

        List<ItemInfo> itemInfos = DatabaseHandler.GetItemInfo(1);

        Debug.Log(itemInfos.Count);
    }
}
