using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;

    void Update() {
        scoreText.text = SceneManager.GetActiveScene().name + "\n" + player.position.z.ToString("0"); //дистанция с целыми числами
    }
}
