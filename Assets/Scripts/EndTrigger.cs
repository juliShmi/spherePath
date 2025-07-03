using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;

    float loadNextSceneDelay = 2f;

    void OnTriggerEnter() {
        gameManager.CompleteLevel();

        Invoke("LoadNextLevel", loadNextSceneDelay);
    }

    void LoadNextLevel() {
        if (GameManager.selectedMode == GameMode.NORMAL) {
            LifeManager.instance.ResetLivesNormalMode();
        }          
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
}
