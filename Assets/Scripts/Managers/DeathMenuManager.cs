using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject deathMenuUI;
    [SerializeField] private TMP_Text deathScoreText;
    
    [Header("Dependencies")]
    [SerializeField] private ScoreManager scoreManager;
    
    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private bool _isPaused;

    private void Start()
    {
        // Initialize with menu hidden
        deathMenuUI.SetActive(false);
        
        // Setup button listeners
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowDeathMenu()
    {
        // Update score display
        if (scoreManager != null && deathScoreText != null)
        {
            deathScoreText.text = $"Score: {scoreManager.playerScore}";
        }
        
        // Show menu
        deathMenuUI.SetActive(true);
        
        // Handle cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        // Pause game
        Time.timeScale = 0f;
        _isPaused = true;
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        ScenesManager.Instance.LoadMainMenu();
    }

    private void QuitGame()
    {
        ScenesManager.Instance.QuitGame();
    }
}
