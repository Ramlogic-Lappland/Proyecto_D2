using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        ScenesManager.Instance.LoadNewGame();
    }
}
