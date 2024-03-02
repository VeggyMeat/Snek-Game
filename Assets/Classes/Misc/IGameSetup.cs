using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
