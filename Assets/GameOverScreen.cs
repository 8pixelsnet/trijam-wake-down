using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnEnable() {
        // Time.timeScale = 0;
        Invoke("Restart", 2F);
    }

    void Restart() {
        gameManager.Restart();
    }
}
