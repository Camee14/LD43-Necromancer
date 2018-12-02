using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

    public float timer = 5f;
	void Start () {
        Destroy(gameObject, timer);
	}

}
