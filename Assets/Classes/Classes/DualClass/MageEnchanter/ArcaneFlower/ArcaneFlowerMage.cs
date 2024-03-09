// COMPLETE

/// <summary>
/// The arcan flower mage class, a subclass of the mage class
/// </summary>
internal class ArcaneFlowerMage : Mage
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/ArcaneFlower/ArcaneFlowerMage.json";

        // indicates that this is not a primary class
        primary = false;

        base.ClassSetup();
    }
}
