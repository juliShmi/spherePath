using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour

{
    public ButtonSound buttonSound;

    public void StartGameWithSound() {
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay() {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene("ChooseMode");
    }

    public void ControlsWithSound() {
        if (buttonSound != null)
            buttonSound.PlayClickSound();
        StartCoroutine(LoadControlsSceneWithDelay());
    }

    private IEnumerator LoadControlsSceneWithDelay() {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene("Controls");
    }
}
