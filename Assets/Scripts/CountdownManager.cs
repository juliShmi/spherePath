using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private GameObject lifePanel;
    [SerializeField] private GameObject scorePanel;
    
    [Header("Countdown Settings")]
    [SerializeField] private int countdownDuration = 3;
    [SerializeField] private float countdownInterval = 1f;
    [SerializeField] private float goDisplayTime = 0.5f;
    
    [Header("Countdown Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countdownClip; // Весь файл 3-2-1-GO
    [SerializeField] private float[] soundStartTimes; // Временные метки начала каждого звука
    [SerializeField] private float[] soundDurations;  // Длительность каждого звука
    
    [Header("Player Control")]
    [SerializeField] private PlayerMovementScript playerMovement;
    [SerializeField] private Rigidbody playerRigidbody;
    
    private bool countdownFinished = false;
    private Coroutine countdownCoroutine;
    
    #region Unity Lifecycle
    
    void Start()
    {
        InitializeComponents();
        StartInitialCountdown();
    }
    
    #endregion
    
    #region Initialization
    
    private void InitializeComponents()
    {
        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovementScript>();
            
        if (playerRigidbody == null && playerMovement != null)
            playerRigidbody = playerMovement.GetComponent<Rigidbody>();
    }
    
    private void StartInitialCountdown()
    {
        PauseGame();
        ShowCountdownUI();
        StartCountdownSequence();
    }
    
    #endregion
    
    #region Countdown Control
    
    public void RestartCountdown()
    {
        StopCurrentCountdown();
        ResetCountdownState();
        StartCountdownSequence();
    }
    
    public void SkipCountdown()
    {
        if (!countdownFinished)
        {
            StopCurrentCountdown();
            ResumeGame();
        }
    }
    
    public bool IsCountdownFinished() => countdownFinished;
    
    private void StartCountdownSequence()
    {
        countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }
    
    private void StopCurrentCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }
    
    private void ResetCountdownState()
    {
        countdownFinished = false;
        PauseGame();
        ShowCountdownUI();
    }
    
    #endregion
    
    #region Game State Management
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
        DisablePlayerControl();
    }
    
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        EnablePlayerControl();
        HideCountdownUI();
        countdownFinished = true;
    }
    
    private void DisablePlayerControl()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;
            
        if (playerRigidbody != null)
            playerRigidbody.isKinematic = true;
    }
    
    private void EnablePlayerControl()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;
            
        if (playerRigidbody != null)
            playerRigidbody.isKinematic = false;
    }
    
    #endregion
    
    #region UI Management
    
    private void ShowCountdownUI()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(true);
        if (lifePanel != null)
            lifePanel.SetActive(false);
        if (scorePanel != null)
            scorePanel.SetActive(false);
    }
    
    private void HideCountdownUI()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(false);
        if (lifePanel != null)
            lifePanel.SetActive(true);
        if (scorePanel != null)
            scorePanel.SetActive(true);
    }
    
    private void UpdateCountdownText(string text)
    {
        if (countdownText != null)
            countdownText.text = text;
    }
    
    #endregion
    
    #region Coroutines
    
    private IEnumerator CountdownCoroutine()
    {
        for (int i = countdownDuration; i > 0; i--)
        {
            UpdateCountdownText(i.ToString());
            PlayCountdownSound(countdownDuration - i); // 0:3, 1:2, 2:1
            yield return new WaitForSecondsRealtime(countdownInterval);
        }

        UpdateCountdownText("GO!");
        PlayCountdownSound(soundStartTimes.Length - 1); // последний — GO!
        yield return new WaitForSecondsRealtime(goDisplayTime);

        ResumeGame();
    }

    private void PlayCountdownSound(int index)
    {
        if (audioSource != null && countdownClip != null && soundStartTimes != null && soundDurations != null
            && index >= 0 && index < soundStartTimes.Length)
        {
            audioSource.clip = countdownClip;
            audioSource.time = soundStartTimes[index];
            audioSource.Play();
            StartCoroutine(StopSoundAfterDuration(soundDurations[index]));
        }
    }

    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (audioSource != null)
            audioSource.Stop();
    }
    
    #endregion
} 