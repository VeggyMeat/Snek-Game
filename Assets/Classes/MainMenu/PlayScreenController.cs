using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScreenController : MonoBehaviour
{
    [SerializeField] private GameObject background;

    private Image backgroundSprite;

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void LeaderboardButtonClicked()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void Start()
    {
        backgroundSprite = background.GetComponent<Image>();
    }

    public void Update()
    {
        backgroundSprite.color = new Color(Mathf.PingPong(Time.time / 2, 0.5f) + 0.5f, Mathf.PingPong(Time.time / 3, 0.5f) + 0.5f, Mathf.PingPong(Time.time / 5, 0.5f) + 0.5f, 1);
    }
}
