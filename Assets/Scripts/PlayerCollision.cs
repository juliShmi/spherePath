using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovementScript movement;
    public AudioSource audioSource;
    public AudioClip collisionSound;

    private bool isCollided = false;
    private Vector3 startPosition;
    private Renderer playerRenderer;

    [SerializeField] private float flashDuration = 1.0f;
    [SerializeField] private int flashCount = 8;
    [SerializeField] private float respawnOffsetY = 0.5f;

    private Collider stageCollider;
    private float ballRadius;


    void Start() 
    {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        startPosition = transform.position;

        playerRenderer = GetComponent<Renderer>();
        ballRadius = GetComponent<SphereCollider>().radius;

        GameObject stage = GameObject.FindWithTag("Stage");
        if (stage != null)
            stageCollider = stage.GetComponent<Collider>();
    }

    void Update() 
    {
        if (transform.position.y < -1f && !isCollided) {
            HandleFall();
        }
    }

    void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.collider.tag == "Obstacle" && !isCollided) {
            HandleObstacleCollision();
        }
    }

    private bool IsOnGround() 
    {
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

    private void HandleFall()
    {
        isCollided = true;

        if (audioSource && collisionSound)
            audioSource.PlayOneShot(collisionSound);

        LifeManager.instance.TakeLife();

        if (LifeManager.instance.GetLives() < 0)
        {
            GameOver();
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity        = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Bounds b = stageCollider.bounds;

        float zPos = transform.position.z;
        float yPos = b.max.y + respawnOffsetY + ballRadius;

        Vector3 respawnPos = new Vector3(b.center.x, yPos, zPos);
        transform.position = respawnPos;

        isCollided = false;
        StartCoroutine(FlashPlayer());
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
        Time.timeScale = 0;
        for (int i = 0; i < flashCount; i++)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
            playerRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
        }
        Time.timeScale = 1;
    }
}
