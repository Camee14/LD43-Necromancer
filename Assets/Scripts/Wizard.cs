using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : MonoBehaviour {

    public float AttackCooldown = 8f;
    public Transform FireBall;
    public Transform DeathEffectPrefab;
    int EntityIndex;

    NavMeshAgent NMA;
    Transform target;
    Health hp;

    bool cooldown = false;

    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        hp = GetComponent<Health>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("aquireTarget", 0, 1);

        hp.OnHealthDamaged += onHealthDamaged;
        hp.OnCharacterDeath += onDeath;
    }
    void Update()
    {
        NavMeshHit hit;
        if (Vector3.Distance(transform.position, target.position) < 50f && !NMA.Raycast(target.position, out hit))
        {
            if (NMA.hasPath)
            {
                NMA.ResetPath();
            }
            if (!cooldown)
            {
                Transform projectile = Instantiate(FireBall, transform.position, Camera.main.transform.rotation);
                projectile.GetComponent<Projectile>().setDirection((target.position - transform.position).normalized);

                cooldown = true;
                Invoke("attackCooldown", AttackCooldown);
            }
        }
    }
    public void setEntityIndex(int i)
    {
        EntityIndex = i;
    }
    public void respawn(Vector3 pos)
    {
        transform.position = pos;
        InvokeRepeating("aquireTarget", 0, 1);
        hp.reset();
    }
    public void die(bool raise_undead)
    {
        Instantiate(DeathEffectPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().killWizard(EntityIndex, raise_undead);
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
    void aquireTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f, LayerMask.GetMask("Friendly"), QueryTriggerInteraction.Ignore);
        Transform closest = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Collider c in cols)
        {
            if ((transform.position - closest.position).sqrMagnitude > (transform.position - c.transform.position).sqrMagnitude)
            {
                target = c.transform;
            }
        }
        NMA.SetDestination(target.position);
    }
    void attackCooldown()
    {
        cooldown = false;
    }
}
