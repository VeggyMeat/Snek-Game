// COMPLETE

/// <summary>
/// Handles the player's selection of what they want next level
/// </summary>
public class SelectionManager : CanvasManager, ISelectionManager
{
    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    /// <summary>
    /// Called when the user clicks on a button (only called by unity button)
    /// </summary>
    /// <param name="button">The number corresponding to which button was clicked</param>
    public override void ButtonClicked(int button)
    {
        HideButtons();

        switch (button)
        {
            case 0:
                // new body

                // if no more bodies can be added, ignore the button click
                if (gameSetup.HeadController.Bodies == 8)
                {
                    break;
                }

                gameSetup.ShopManager.NextState = ChoiceState.NewBody;
                break;
            case 1:
                // level up body
                gameSetup.ShopManager.NextState = ChoiceState.BodyUpgrade;
                break;
            case 2:
                // add item

                // if no more items can be added, ignore the button click
                if (ItemManager.items.Count == 8)
                {
                    break;
                }

                gameSetup.ShopManager.NextState = ChoiceState.Item;
                break;
        }
    }

    /// <summary>
    /// Called to hide the canvas
    /// </summary>
    public override void HideButtons()
    {
        base.HideButtons();

        gameSetup.Reorganiser.Active = false;

        // resumes time
        gameSetup.ShopManager.ResumeTime();
    }

    /// <summary>
    /// Called to show the canvas
    /// </summary>
    public override void ShowButtons()
    {
        base.ShowButtons();

        gameSetup.Reorganiser.Active = true;

        // pauses time
        gameSetup.ShopManager.PauseTime();
    }
}
