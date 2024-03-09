// COMPLETE

/// <summary>
/// The death ward mage class, a subclass of the mage class
/// </summary>
internal class DeathWardMage : Mage
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/DeathWard/DeathWardMage.json";

        // indicates that this is not a primary class
        primary = false;

        base.ClassSetup();
    }
}
