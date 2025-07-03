using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovementScript movement;
    public AudioSource audioSource;
    public AudioClip collisionSound;
    private bool isCollided = false;

    void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update() {
        if (transform.position.y < -1f && !isCollided) {
            StopMotion();
    }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.collider.tag == "Obstacle" && !isCollided) {
            StopMotion();
        }
    }

    private void StopMotion() {
            isCollided = true;
            if (audioSource != null && collisionSound != null) {
                audioSource.PlayOneShot(collisionSound);
            }
            movement.enabled = false; // disable the movement script
            GetComponent<Rigidbody>().isKinematic = true;
            LifeManager.instance.TakeLife();
            Debug.Log("Lives: " + LifeManager.instance.GetLives());
            if (LifeManager.instance.GetLives() >= 0) {
                FindAnyObjectByType<GameManager>().EndGame();
            } else {
                FindAnyObjectByType<GameManager>().ShowFailPanel();
                Time.timeScale = 0; 
            }
    }
}
