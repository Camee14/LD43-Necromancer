using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
    public Transform KnightPrefab;
	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnWave", 0, 30);
	}

    void SpawnWave()
    {
        for (int i = 0; i < 5; i++) {
            Instantiate(KnightPrefab, Vector3.zero, Quaternion.identity);
        }
    }

}
