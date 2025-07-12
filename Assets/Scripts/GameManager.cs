using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameMode { NORMAL, HARD }

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 2f;

    public static GameMode selectedMode = GameMode.NORMAL;

    public GameObject completeLevelUI;
    public GameObject lifePanel;
    public GameObject failPanel;
    public GameObject tabKeyInfoPanel;
    public GameObject restartLevelButton;
    private ButtonSound buttonSound;

    void Awake() {
        buttonSound = FindFirstObjectByType<ButtonSound>();
    }

    public void SelectNormalMode() {
        selectedMode = GameMode.NORMAL;
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        StartCoroutine(LoadSceneWithDelay());
    }

    public void SelectHardMode() {
        selectedMode = GameMode.HARD;
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        StartCoroutine(LoadSceneWithDelay());
    }

    public void Back() {
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        StartCoroutine(LoadStartSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay() {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame() {
        if (gameHasEnded == false) {
            gameHasEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CompleteLevel() {
        completeLevelUI.SetActive(true);
        lifePanel.SetActive(false);
        tabKeyInfoPanel.SetActive(false);
    }

    public void ShowFailPanel() {
        if (lifePanel != null) lifePanel.SetActive(false);
        if (failPanel != null) {
            failPanel.SetActive(true);
        } 
        tabKeyInfoPanel.SetActive(false);

        if (restartLevelButton != null) {
        restartLevelButton.SetActive(GameManager.selectedMode != GameMode.HARD);
    }
    } 

    public void TryAgain() {
        Time.timeScale = 1;
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        Destroy(LifeManager.instance.gameObject);
        StartCoroutine(LoadStartSceneWithDelay());
    }

    public void RestartLevel() {
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        LifeManager.instance.GameOverTryAgain();
        StartCoroutine(LoadCurrentSceneWithDelay());
    }


    private IEnumerator LoadCurrentSceneWithDelay() {
            yield return new WaitForSecondsRealtime(0.2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadStartSceneWithDelay() {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene("WelcomeScreen");
    }
}
