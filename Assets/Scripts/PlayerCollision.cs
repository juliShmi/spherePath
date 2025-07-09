using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovementScript movement;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    private bool isCollided = false;
    private Vector3 startPosition;
    private Vector3 lastGroundedPosition;
    private Renderer playerRenderer;
    [SerializeField] private float flashDuration = 1.0f;
    [SerializeField] private int flashCount = 8;

    void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        startPosition = transform.position;
        lastGroundedPosition = startPosition;
        playerRenderer = GetComponent<Renderer>();
    }

    void Update() {
        if (transform.position.y < -1f && !isCollided) {
            HandleFall();
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.collider.tag == "Stage") {
            lastGroundedPosition = transform.position;
        }
        if (collisionInfo.collider.tag == "Obstacle" && !isCollided) {
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
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            GameObject stage = GameObject.FindWithTag("Stage");
            float y = 1f;
            if (stage != null) {
                Collider col = stage.GetComponent<Collider>();
                if (col != null) {
                    y = col.bounds.max.y + 1f;
                } else {
                    y = stage.transform.position.y + 1f;
                }
            }
            transform.position = new Vector3(lastGroundedPosition.x, y, lastGroundedPosition.z);
            isCollided = false;
            StartCoroutine(FlashPlayer());
        }
    }

    private void GameOver() {
        movement.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        FindAnyObjectByType<GameManager>().ShowFailPanel();
        Time.timeScale = 0;
    }

    private IEnumerator FlashPlayer()
    {
        if (playerRenderer == null) yield break;
        for (int i = 0; i < flashCount; i++)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
            playerRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
        }
    }
}
