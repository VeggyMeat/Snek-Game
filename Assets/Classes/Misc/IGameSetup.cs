// COMPLETE

/// <summary>
/// The interface for the game setup
/// </summary>
public interface IGameSetup
{
    public IShopManager ShopManager { get; }
    public IHeadController HeadController { get; }
    public IDeathScreen DeathScreenController { get; }
    public ICameraController CameraController { get; }
    public IUIController UIController { get; }
    public IEnemySummonerController EnemySummonerController { get; }
    public IReorganiser Reorganiser { get; }
    public IBackgroundManager BackgroundManager { get; }
    public ISelectionManager SelectionManager { get; }
    public IChoiceManager ChoiceManager { get; }
}
