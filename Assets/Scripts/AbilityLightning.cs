using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLightning : MonoBehaviour {

    public int ManaCost = 5;
    public float Cooldown = 5f;

    public Transform LightningAnim;

    public Image CooldownUI;

    Minions mana;
    bool on_cooldown = false;

    void Start () {
        mana = GetComponent<Minions>();
    }
	
	// Update is called once per frame
	void Update () {
        if (on_cooldown)
        {
            return;
        }

        if (Input.GetButtonDown("AbilityLightning"))
        {
            if (mana.use(ManaCost))
            {
                activate();
                startCooldown();
            }
        }
    }
    void activate() {
        Collider[] cols = Physics.OverlapSphere(transform.position, 150f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
        foreach (Collider c in cols) {
            Health h = c.GetComponent<Health>();
            if (h != null) {
                h.apply(-1000);
            }
            Instantiate(LightningAnim, c.transform.position, Quaternion.identity);
        }
    }
    void startCooldown()
    {
        on_cooldown = true;
        StartCoroutine(doCooldownTimer());
    }
    IEnumerator doCooldownTimer()
    {
        float timer = Cooldown;
        while (timer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
            CooldownUI.fillAmount = timer / Cooldown;
        }
        endCooldown();
    }
    void endCooldown()
    {
        on_cooldown = false;

    }
}
