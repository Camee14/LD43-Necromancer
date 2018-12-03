using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    public EnemyManager EM;
    public Transform Player;

    public Transform GameOver;
    public Text GameOverText;

    public Text ScoreText;
    
    Health PlayerHealth;

    int wave = 0;
    int score = 0;

	void Start () {
        Health PlayerHealth = Player.GetComponent<Health>();

        PlayerHealth.OnCharacterDeath += EndGame;

        EM.onScoreChanged += onScoreChanged;
        EM.onWaveChanged += onWaveChanged;
    }
    void EndGame(bool state) {
        Time.timeScale = 0;
        GameOverText.text = "You Survived Until Wave " + wave;
        GameOver.gameObject.SetActive(true);
}
    void onScoreChanged(int ammount) {
        score = Mathf.Max(0, score += ammount);

        ScoreText.text = score.ToString();
    }
    void onWaveChanged(int current_wave) {
        wave = current_wave;
    }
    public void restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
