using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    internal static List<Item> items = new List<Item>();

    internal static IGameSetup gameSetup;

    internal static void Setup(IGameSetup gameSetup)
    {
        ItemManager.gameSetup = gameSetup;
    }

    internal static void AddItem(string itemName)
    {
        Item item;

        switch (itemName)
        {
            case nameof(Quiver):
                item = new Quiver();
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
            case nameof(CactusHelmet):
                item = new CactusHelmet();
                break;
            default:
                // crashes if its been called with an item that doesn't exist
                throw new System.Exception();
        }

        // removes that body from the list of available bodies
        if (gameSetup.ShopManager.Remove)
        {
            gameSetup.ShopManager.RemoveItem(itemName);
        }

        item.Setup(gameSetup);
        items.Add(item);
    }
}
