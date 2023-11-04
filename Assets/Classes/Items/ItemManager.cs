using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    internal static List<Item> items = new List<Item>();
    internal static HeadController headController;

    internal static void Setup(HeadController headController)
    {
        ItemManager.headController = headController;
    }

    internal static void AddItem(string itemName)
    {
        Item item;

        switch (itemName)
        {
            case nameof(Quiver):
                item = new Quiver();
                break;
            case nameof(LuckyFlask):
                item = new LuckyFlask();
                break;
            case nameof(SteeringWheel):
                item = new SteeringWheel();
                break;
            case nameof(BigBullet):
                item = new BigBullet();
                break;
            case nameof(HeadstoneHelmet):
                item = new HeadstoneHelmet();
                break;
            case nameof(KineticLauncher):
                item = new KineticLauncher();
                break;
            case nameof(MagicCap):
                item = new MagicCap();
                break;
            case nameof(ExplosiveTippedArrows):
                item = new ExplosiveTippedArrows();
                break;
            case nameof(EnchantersTome):
                item = new EnchantersTome();
                break;
            case nameof(BerserkerBlood):
                item = new BerserkerBlood();
                break;
            default:
                // crashes if its been called with an item that doesn't exist
                throw new System.Exception();
        }

        item.Setup();
        items.Add(item);
    }
}
