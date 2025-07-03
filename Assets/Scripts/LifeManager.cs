using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    public static LifeManager instance;
    public RawImage[] lifeIcons;

    private int lives = 3;
    public static int hardModeLives = 3;
    
    
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        lifeIcons = null;
        var panel = GameObject.Find("LifePanel");

        if (panel != null) {
            panel.SetActive(IsGameScene(scene.name));
            lifeIcons = panel.GetComponentsInChildren<RawImage>();
            UpdateLifeIcons();
        }   
    }

    public int GetLives() {
        return lives;
    }

    public void TakeLife() {
        lives--;
        if (lives >= 0) {
            if (GameManager.selectedMode == GameMode.HARD) hardModeLives = lives;
            UpdateLifeIcons();
        }
    }

    public void GameOverTryAgain() {
        lives = 3;
        hardModeLives = 3;
        UpdateLifeIcons();
        Debug.Log("LifeIcons: " + lifeIcons.Length);
    }

    private void UpdateLifeIcons() {
        for (int i = 0; i < lifeIcons.Length; i++) {
            lifeIcons[i].enabled = i < lives;
        }
    }

    public void ResetLivesNormalMode() {
        lives = 3;
        UpdateLifeIcons();
    }

    private bool IsGameScene(string sceneName) {
        return sceneName.Contains("Level");
    }
}

