using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// COMPLETE

/// <summary>
/// Controls the main menu screen
/// </summary>
public class PlayScreenController : MonoBehaviour
{
    /// <summary>
    /// The background of the main menu
    /// </summary>
    [SerializeField] private GameObject background;

    /// <summary>
    /// The image component of the background
    /// </summary>
    private Image backgroundSprite;

    /// <summary>
    /// Loads the game scene (called when the play button is clicked)
    /// </summary>
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Loads the leaderboard scene (called when the leaderboard button is clicked)
    /// </summary>
    public void LeaderboardButtonClicked()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    // Called on the first frame of the scene
    public void Start()
    {
        backgroundSprite = background.GetComponent<Image>();
    }

    // Called every frame of the scene
    public void Update()
    {
        // sets the colour of the background to ping pong between different colours over time
        backgroundSprite.color = new Color(Mathf.PingPong(Time.time / 2, 0.5f) + 0.5f, Mathf.PingPong(Time.time / 3, 0.5f) + 0.5f, Mathf.PingPong(Time.time / 5, 0.5f) + 0.5f, 1);
    }
}
