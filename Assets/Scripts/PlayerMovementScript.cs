using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour {

    public Rigidbody rb;
    public GameObject pauseUI;
    public GameObject controlPanel;

    public AudioSource audioSource;
    public AudioClip jumpSound;

    public float forwardForce = 2000f;
    public float sidewaysForce = 50f;
    public float jumpForce = 5f;
    private bool jumpPressed = false;
    private bool isOnStage = false;
    private bool isPaused = false;
    private bool controlPanelShowed = false;
    

    public GameObject particleObject;
    private ParticleSystem sparkleParticles;
    private float ballRadius = 0.5f;
    
    private CountdownManager countdownManager;

    void Start() 
    {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        if (particleObject != null) {
            sparkleParticles = particleObject.GetComponent<ParticleSystem>();
        }
        if (pauseUI != null) {
            pauseUI.SetActive(false);
        }
        if (controlPanel != null) {
            controlPanel.SetActive(false);
        }
        countdownManager = FindFirstObjectByType<CountdownManager>();
    }

    void Update() {
        if (countdownManager != null && !countdownManager.IsCountdownFinished()) {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            ShowControlPanel();
        }

        if (isPaused) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            jumpPressed = true;
            if (audioSource != null && jumpSound != null) {
                audioSource.PlayOneShot(jumpSound);
            }
        }
        UpdateParticlePosition();
    }
    
    void UpdateParticlePosition() {
        if (particleObject != null) {
            RaycastHit hit;
            Vector3 rayStart = transform.position;
            Vector3 rayDirection = Vector3.down;
            
            if (Physics.Raycast(rayStart, rayDirection, out hit, ballRadius + 0.1f)) {
                Vector3 groundPoint = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                particleObject.transform.position = groundPoint;
            } else {
                particleObject.transform.position = transform.position + Vector3.down * ballRadius;
            }
        }
    }
    
    void FixedUpdate() {
        if (countdownManager != null && !countdownManager.IsCountdownFinished()) {
            return;
        }
        
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.Force);
        }

        if (jumpPressed && isOnStage) {
            rb.AddForce(0, jumpForce, 0, ForceMode.VelocityChange);
            jumpPressed = false;
            isOnStage = false;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Stage") {
            isOnStage = true;
        }
        
    }

    void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Stage") {
            isOnStage = false;
        }
        
    }

    private void ShowControlPanel() {
        controlPanelShowed = !controlPanelShowed;
        if (controlPanelShowed) {
            Time.timeScale = 0f;
            if (controlPanel != null) {
                controlPanel.SetActive(true);
            }
        } else {
            Time.timeScale = 1f;
            if (controlPanel != null) {
                controlPanel.SetActive(false);
            }
        }
    }

    private void PauseGame() {
        isPaused = !isPaused;
        if (isPaused) {
            Time.timeScale = 0f;
            if (pauseUI != null) {
                 pauseUI.SetActive(true);
                }
            } else {
                Time.timeScale = 1f;
                if (pauseUI != null) {
                    pauseUI.SetActive(false);
                }
            }
    }
}
