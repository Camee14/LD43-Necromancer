using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spectre : MonoBehaviour {

    public Transform SacrificeEffect;
    NavMeshAgent NMA;
    Transform target;
    Health hp;

    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        hp = GetComponent<Health>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("aquireTarget", 0, 1);

        //hp.OnHealthDamaged += onHealthDamaged;
        hp.OnCharacterDeath += onSpectreDeath;
    }

    public void respawn(Vector3 pos)
    {
        transform.position = pos;
        InvokeRepeating("aquireTarget", 0, 1);
        hp.reset();
    }
    public void die(bool state)
    {
        if (state)
        {
            Instantiate(SacrificeEffect, transform.position, Quaternion.identity);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Minions>().killSpectre(transform);
        CancelInvoke();
        if (NMA.hasPath)
        {
            NMA.ResetPath();
        }
    }
    void onHealthDamaged(int hp, int max) {
        //Debug.Log("taking damage!");
    }
    void onSpectreDeath(bool state) {
        die(state);
    }
    void aquireTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
        Transform closest = GameObject.FindGameObjectWithTag("Player").transform;
        foreach (Collider c in cols)
        {
            if ((transform.position - closest.position).sqrMagnitude > (transform.position - c.transform.position).sqrMagnitude)
            {
                //target = c.transform;
            }
        }

        NMA.SetDestination(target.position);
    }
}
