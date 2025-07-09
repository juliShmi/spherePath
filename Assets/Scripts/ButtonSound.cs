using UnityEngine;
using System.Collections;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = FindFirstObjectByType<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
        StartCoroutine(LoadSceneWithDelay());
    }

        private IEnumerator LoadSceneWithDelay() {
        yield return new WaitForSecondsRealtime(0.2f);
    }
}
