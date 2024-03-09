// COMPLETE

/// <summary>
/// The spike launcher frontline class, a subclass of the frontline class
/// </summary>
internal class SpikeLauncherFrontline : Frontline
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/SpikeLauncher/SpikeLauncherFrontline.json";

        // indicates that this is not the primary class on the body
        primary = false;

        base.ClassSetup();
    }

    /// <summary>
    /// Called when the body takes damage, before the damage is applied
    /// </summary>
    /// <param name="amount">The damage taken</param>
    /// <returns>Returns the new damage value</returns>
    internal override int OnDamageTaken(int amount)
    {
        // launches spikes
        ((SpikeLauncherArcher)body.classes[0]).LaunchSpikes();

        return base.OnDamageTaken(amount);
    }
}
