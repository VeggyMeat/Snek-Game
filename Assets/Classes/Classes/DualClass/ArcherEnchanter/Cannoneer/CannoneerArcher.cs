// COMPLETE

/// <summary>
/// The cannoneer archer class, a subclass of the archer class
/// </summary>
internal class CannoneerArcher : Archer
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/Cannoneer/CannoneerArcher.json";

        // indicates that this is not the primary class of the object
        primary = false;

        base.ClassSetup();
    }
}
