using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minions : MonoBehaviour {
    public int MaxMinionCount = 10;
    public Transform MinionPrefab;

    public Text MinionUI;

    List<Transform> minions;
    List<Transform> inactive;

    void Start() {
        minions = new List<Transform>();
        inactive = new List<Transform>();
    }
    void Update() {
        int num = GameObject.FindGameObjectsWithTag("Minion").Length;
        if (minions.Count != num) {
            foreach (Transform t in minions) {
                Debug.Log(t.gameObject.activeInHierarchy);
            }
            Debug.LogError("minion count discrepancy, list = " + minions.Count + " gameobjects = " + num);
        }
        changeMinionUI(minions.Count);
    }
    public void addMinion(Vector3 pos) {
        if (minions.Count >= MaxMinionCount) {
            return;
        }
        if (inactive.Count > 0)
        {
            //reactivate a dead minion
            Transform m = inactive[0];
            m.GetComponent<Spectre>().respawn(pos);
            m.gameObject.SetActive(true);

            minions.Add(m);
            inactive.RemoveAt(0);
        }
        else
        {
            Spectre s = Instantiate(MinionPrefab, pos, Quaternion.identity).GetComponent<Spectre>();
            minions.Add(s.transform);
        }

        //changeMinionUI(minions.Count);

    }
    public void killSpectre(Transform t) {
        if (inactive.Contains(t)) {
            Debug.LogError("minion already dead");
        }
        minions.Remove(t);
        inactive.Add(t);
        t.gameObject.SetActive(false);
        //changeMinionUI(minions.Count);
    }
    public bool use(int num) {
        if (minions.Count >= num) {
            while (num > 0) {
                minions[0].GetComponent<Health>().apply(-1000, true);
                num--;
            }
            //changeMinionUI(minions.Count);
            return true;
        }
        return false;
    }
    void changeMinionUI(int num) {
        MinionUI.text = num + " / " + MaxMinionCount; 
    }
}
