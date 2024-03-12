using System.Collections.Generic;

// COMPLETE

/// <summary>
/// The class that manages all the items
/// </summary>
public static class ItemManager
{
    /// <summary>
    /// The list of current items
    /// </summary>
    internal static List<Item> items = new List<Item>();

    /// <summary>
    /// The game setup
    /// </summary>
    internal static IGameSetup gameSetup;

    /// <summary>
    /// Sets up the item manager
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal static void Setup(IGameSetup gameSetup)
    {
        ItemManager.gameSetup = gameSetup;
    }

    /// <summary>
    /// Adds a new item
    /// </summary>
    /// <param name="itemName">The name of the item to add</param>
    /// <exception cref="System.Exception">An exception is called if the item name is not one of the items</exception>
    internal static void AddItem(string itemName)
    {
        Item item;

        // grabs the item that matches the name
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

        // sets p the item and adds it to the list
        item.Setup(gameSetup);
        items.Add(item);
    }
}
