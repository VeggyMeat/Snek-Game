using UnityEngine;

// COMPLETE

/// <summary>
/// Sets up the game as well as providing access to the game's main controllers on initial objects
/// </summary>
public class GameSetup : MonoBehaviour, IGameSetup
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemySummonerObject;
    [SerializeField] private GameObject deathScreenObject;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject uIObject;
    [SerializeField] private GameObject optionChoicesObject;
    [SerializeField] private GameObject shopManagerObject;
    [SerializeField] private GameObject selectOptionsObject;
    [SerializeField] private GameObject reorganiserObject;
    [SerializeField] private GameObject backgroundObject;

    public IShopManager ShopManager { get; private set; }
    public IHeadController HeadController { get; private set; }
    public IDeathScreen DeathScreenController { get; private set; }
    public ICameraController CameraController { get; private set; }
    public IUIController UIController { get; private set; }
    public IEnemySummonerController EnemySummonerController { get; private set; }
    public IReorganiser Reorganiser { get; private set; }
    public IBackgroundManager BackgroundManager { get; private set; }
    public ISelectionManager SelectionManager { get; private set; }
    public IChoiceManager ChoiceManager { get; private set; }

    // Called by unity as soon as the game is started (the Game scene is loaded)
    public void Awake()
    {
        // grabs all the different controllers from the objects
        ShopManager = shopManagerObject.GetComponent<ShopManager>();
        HeadController = playerObject.GetComponent<HeadController>();
        DeathScreenController = deathScreenObject.GetComponent<DeathScreenController>();
        CameraController = cameraObject.GetComponent<CameraController>();
        UIController = uIObject.GetComponent<UIController>();
        EnemySummonerController = enemySummonerObject.GetComponent<EnemySummonerController>();
        Reorganiser = reorganiserObject.GetComponent<Reorganiser>();
        BackgroundManager = backgroundObject.GetComponent<BackgroundManager>();
        SelectionManager = selectOptionsObject.GetComponent<SelectionManager>();
        ChoiceManager = optionChoicesObject.GetComponent<ChoiceManager>();

        // gives all the controllers reference to this object
        HeadController.SetGameSetup(this);
        UIController.SetGameSetup(this);
        Reorganiser.SetGameSetup(this);
        ShopManager.SetGameSetup(this);
        SelectionManager.SetGameSetup(this);
        ChoiceManager.SetGameSetup(this);
        CameraController.SetGameSetup(this);
        BackgroundManager.SetGameSetup(this);
        DeathScreenController.SetGameSetup(this);
        EnemySummonerController.SetGameSetup(this);

        // sets up various static classes
        ItemManager.Setup(this);
        DatabaseHandler.Setup();
        AOEEffect.Setup();
        TimeManager.Setup();
    }
}
