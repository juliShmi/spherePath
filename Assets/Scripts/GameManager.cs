using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //change or reload the scene

public enum GameMode { NORMAL, HARD }

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 2f;

    public static GameMode selectedMode = GameMode.NORMAL;

    public GameObject completeLevelUI;
    public GameObject lifePanel;
    public GameObject failPanel; 

    public void SelectNormalMode() {
        selectedMode = GameMode.NORMAL;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectHardMode() {
        selectedMode = GameMode.HARD;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EndGame() { //public is used to find the script in the PlayerCollision script
        if (gameHasEnded == false) {
            gameHasEnded = true;
            Invoke("Restart", restartDelay); //invoke is used to call the function after 2 seconds
        }
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CompleteLevel() {
        completeLevelUI.SetActive(true);
        lifePanel.SetActive(false);
    }

    public void ShowFailPanel() {
        if (lifePanel != null) lifePanel.SetActive(false);
        if (failPanel != null) {
            failPanel.SetActive(true);
        } 
    } 

    public void TryAgain() {
        Time.timeScale = 1;
        Destroy(LifeManager.instance.gameObject);
        SceneManager.LoadScene("WelcomeScreen");
    }
}
