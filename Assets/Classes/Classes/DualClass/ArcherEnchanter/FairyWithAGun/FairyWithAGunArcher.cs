// COMPLETE

/// <summary>
/// The fairy with a gun archer class, a subclass of the archer class
/// </summary>
internal class FairyWithAGunArcher : Archer
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/FairyWithAGun/FairyWithAGunArcher.json";

        // indicates that this is not the primary class of the object
        primary = false;

        base.ClassSetup();
    }
}
