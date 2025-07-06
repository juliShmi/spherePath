using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovementScript movement;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    private bool isCollided = false;
    private Vector3 startPosition;
    private CountdownManager countdownManager;

    void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        startPosition = transform.position;
    }

    void Update() {
        if (transform.position.y < -1f && !isCollided) {
            HandleFall();
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.collider.tag == "Obstacle" && !isCollided && IsOnGround()) {
            HandleObstacleCollision();
        }
    }

    private bool IsOnGround() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f)) {
            return hit.collider.tag == "Stage";
        }
        return false;
    }

    private void HandleObstacleCollision() {
        isCollided = true;
        
        if (audioSource != null && collisionSound != null) {
            audioSource.PlayOneShot(collisionSound);
        }
        
        LifeManager.instance.TakeLife();
        
        if (LifeManager.instance.GetLives() < 0) {
            GameOver();
        } else {
            isCollided = false;
        }
    }

    private void HandleFall() {
        isCollided = true;
        
        if (audioSource != null && collisionSound != null) {
            audioSource.PlayOneShot(collisionSound);
        }
        
        LifeManager.instance.TakeLife();
        
        if (LifeManager.instance.GetLives() < 0) {
            GameOver();
        } else {
            ReturnToStart();
        }
    }

    private void GameOver() {
        movement.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        FindAnyObjectByType<GameManager>().ShowFailPanel();
        Time.timeScale = 0;
    }

    private void ReturnToStart() {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        transform.position = startPosition;
        
        isCollided = false;
        
        countdownManager = FindFirstObjectByType<CountdownManager>();
        if (countdownManager != null) {
            countdownManager.RestartCountdown();
        }
    }
}
