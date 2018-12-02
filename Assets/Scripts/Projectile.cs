using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float Speed = 3f;
    public int Damage = 30;
    public bool RaiseUndead = false;

    Vector3 MoveDir = Vector3.forward;

    public void setDirection(Vector3 dir) {
        MoveDir = dir;
    }
    void Start() {
        Destroy(gameObject, 5f);
    }
	void FixedUpdate () {
        transform.position = transform.position + MoveDir * Speed * Time.deltaTime;
	}
    void OnTriggerEnter(Collider col){
        if (col.isTrigger) {
            return;
        }
        Health h = col.GetComponent<Health>();
        if (h != null) {
            h.apply(-Damage, RaiseUndead);
        }
        Destroy(gameObject);
    }
}
