using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public Transform player;
    public Transform finishLine;
    public TextMeshProUGUI scoreText;
    
    private float totalDistance;

    void Start() {
        if (finishLine == null) {
            GameObject finish = GameObject.Find("FinishGate") ?? GameObject.Find("END");
            if (finish != null) {
                finishLine = finish.transform;
            }
        }
        
        if (finishLine != null) {
            totalDistance = finishLine.position.z - player.position.z;
        }
    }

    void Update() {
        if (finishLine != null) {
            float remainingDistance = finishLine.position.z - player.position.z;
            
            if (remainingDistance > 0) {
                scoreText.text = SceneManager.GetActiveScene().name + "\n" + remainingDistance.ToString("0") + " m left";
            } else {
                scoreText.text = SceneManager.GetActiveScene().name + "\nFINISH!";
            }
        } else {
            scoreText.text = SceneManager.GetActiveScene().name + "\n" + player.position.z.ToString("0") + "m";
        }
    }
}
