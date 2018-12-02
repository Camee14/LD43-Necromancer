using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {
    public int MaxEnemyCount = 100;

    public float KnightsToSpawn = 10;
    public float KnightSpawnInterval = 1f;
    public float WizardsToSpawn = 0;
    public float WizardSpawnInterval = 1f;

    public Transform KnightPrefab;
    public Transform WizardPrefab;
    public Transform[] SpawnPoints;

    public Text WaveText;

    public delegate void ScoreEvent(int score);
    public delegate void WaveEvent(int wave);

    public event ScoreEvent onScoreChanged;
    public event WaveEvent onWaveChanged;

    List<Transform> knights;
    List<Transform> wizards;


    List<int> inactive_knights;
    List<int> inactive_wizards;

    int num_active_enemies = 0;
    int wave_counter = 0;


    bool wave_ongoing = false;

    public void killKnight(int index, bool raise_undead)
    {
        if (inactive_knights.Contains(index))
        {
            Debug.LogError(index + " is already in inactive wizard array");
        }

        if (raise_undead)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Minions>().addMinion(new Vector3(knights[index].position.x, 0, knights[index].position.z));
        }

        knights[index].gameObject.SetActive(false);

        inactive_knights.Add(index);
        num_active_enemies--;

        if (onScoreChanged != null)
        {
            onScoreChanged(1);
        }
    }
    public void killWizard(int index, bool raise_undead)
    {
        if (inactive_wizards.Contains(index)) {
            Debug.LogError(index + " is already in inactive wizard array");
        }

        if (raise_undead)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Minions>().addMinion(new Vector3(wizards[index].position.x, 0, wizards[index].position.z));
        }

        wizards[index].gameObject.SetActive(false);

        inactive_wizards.Add(index);
        num_active_enemies--;

        if (onScoreChanged != null)
        {
            onScoreChanged(5);
        }
    }

    void Start() {
        Random.InitState(System.Environment.TickCount);
        knights = new List<Transform>();
        wizards = new List<Transform>();

        inactive_knights = new List<int>();
        inactive_wizards = new List<int>();

        Invoke("startNextWave", 3f);
        InvokeRepeating("waveStartCountdown", 0, 1f);

    }
    void Update() {
        if (wave_ongoing && num_active_enemies == 0) {
            wave_ongoing = false;

            Invoke("startNextWave", 3f);
            InvokeRepeating("waveStartCountdown", 0, 1f);
        }
    }
    void waveStartCountdown() {
        Debug.Log("waiting for wave "+(wave_counter + 1)+" to start...");
    }
    void startNextWave() {
        CancelInvoke("waveStartCountdown");

        wave_ongoing = true;
        wave_counter++;
        WaveText.text = wave_counter.ToString();

        if (onWaveChanged != null) {
            onWaveChanged(wave_counter);
        }

        adjustSpawnNumbers();

        StartCoroutine(spawnKnights((int)KnightsToSpawn, KnightSpawnInterval));
        StartCoroutine(spawnWizards((int)WizardsToSpawn, WizardSpawnInterval));
    }
    void adjustSpawnNumbers() {
        KnightsToSpawn += wave_counter + KnightsToSpawn * 0.25f;
        WizardsToSpawn += (wave_counter / 3) + WizardsToSpawn * 0.1f;
    }
    IEnumerator spawnKnights(int num, float delay) {
        while (num > 0)
        {
            if (num_active_enemies < MaxEnemyCount)
            {
                if (inactive_knights.Count > 0)
                {
                    knights[inactive_knights[0]].GetComponent<Knight>().respawn(getSpawnPoint());
                    knights[inactive_knights[0]].gameObject.SetActive(true);

                    inactive_knights.RemoveAt(0);
                }
                else
                {
                    Knight k = Instantiate(KnightPrefab, getSpawnPoint(), Quaternion.identity).GetComponent<Knight>();
                    k.setEntityIndex(knights.Count);

                    knights.Add(k.transform);
                }
                num--;
                num_active_enemies++;
            }
            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator spawnWizards(int num, float delay) {
        while (num > 0)
        {
            if (num_active_enemies < MaxEnemyCount)
            {
                if (inactive_wizards.Count > 0)
                {
                    wizards[inactive_wizards[0]].GetComponent<Wizard>().respawn(getSpawnPoint());
                    wizards[inactive_wizards[0]].gameObject.SetActive(true);

                    inactive_wizards.RemoveAt(0);
                }
                else
                {
                    Wizard w = Instantiate(WizardPrefab, getSpawnPoint(), Quaternion.identity).GetComponent<Wizard>();
                    w.setEntityIndex(wizards.Count);

                    wizards.Add(w.transform);
                }
                num--;
                num_active_enemies++;
            }
            yield return new WaitForSeconds(delay);
        }
    }
    Vector3 getSpawnPoint() {
        Vector3 player = GameObject.FindGameObjectWithTag("Player").transform.position;
        List<Vector3> points = new List<Vector3>();
        foreach (Transform point in SpawnPoints) {
            if (Vector3.Distance(player, point.position) >= 50f) {
                points.Add(point.position);
            }
        }
        if (points.Count > 0) {
            return points[Random.Range(0, points.Count)];
        }
        Debug.LogError("failed to find suitable point for spawning an enemy");
        return Vector3.zero;
    }


}
