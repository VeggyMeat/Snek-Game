using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayScreenController : MonoBehaviour
{
    [SerializeField] private GameObject background;

    private Image backgroundSprite;

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void Start()
    {
        backgroundSprite = background.GetComponent<Image>();
    }

    public void Update()
    {

    }
}
