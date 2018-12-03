using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour {
    public int AttackDamage = 10;
    public float AttackCooldown = 1f;

    public Transform DeathEffectPrefab;

    int EntityIndex;

    NavMeshAgent NMA;
    public Transform target;
    Health hp;

    bool cooldown = false;
    
	void Start () {
        NMA = GetComponent<NavMeshAgent>();
        hp = GetComponent<Health>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("aquireTarget", 0, 1);

        hp.OnHealthDamaged += onHealthDamaged;
        hp.OnCharacterDeath += onDeath;
    }
    void Update() {
        if (!target.gameObject.activeInHierarchy)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (Vector3.Distance(transform.position, target.position) < 2f) {
            if (NMA.hasPath)
            {
                NMA.ResetPath();
            }
            if (!cooldown)
            {
                Health targethp = target.GetComponent<Health>();
                if (targethp != null) {

                    targethp.apply(-AttackDamage);

                    if (targethp.Hp <= 0) {
                        target = GameObject.FindGameObjectWithTag("Player").transform;
                    }
                }

                cooldown = true;
                Invoke("attackCooldown", AttackCooldown);
            }
        }
    }
    public void setEntityIndex(int i) {
        EntityIndex = i;
    }
    public void respawn(Vector3 pos) {
        transform.position = pos;
        InvokeRepeating("aquireTarget", 0, 1);
        hp.reset();

        cooldown = false;
    }
    public void die(bool raise_undead) {
        Instantiate(DeathEffectPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().killKnight(EntityIndex, raise_undead);
        CancelInvoke();
        if (NMA.hasPath)
        {
            NMA.ResetPath();
        }
    }
    void onHealthDamaged(int hp, int max)
    {
        //Debug.Log("taking damage!");
    }
    void onDeath(bool state)
    {
        die(state);
    }
    void aquireTarget() {
        Collider[] cols = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Friendly"), QueryTriggerInteraction.Ignore);
        Transform closest = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Collider c in cols) {
            if ((transform.position - closest.position).sqrMagnitude > (transform.position - c.transform.position).sqrMagnitude) {
                target = c.transform;
            }
        }
        NMA.SetDestination(target.position);
    }
    void attackCooldown() {
        cooldown = false;
    }
}
