// COMPLETE

/// <summary>
/// The vampire frontline class, a subclass of the frontline class
/// </summary>
internal class VampireFrontline : Frontline
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/Vampire/VampireFrontline.json";

        // notes that this is not the primary class
        primary = false;

        base.ClassSetup();
    }
}
