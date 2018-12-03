using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHealth : MonoBehaviour {

    public int HealingAmmount = 25;
    public int ManaCost = 5;
    public float Cooldown = 5f;

    public Image CooldownUI;

    Minions mana;
    Health hp;

    bool on_cooldown = false;

    void Start() {
        mana = GetComponent<Minions>();
        hp = GetComponent<Health>();
    }
    void Update() {
        if (on_cooldown) {
            return;
        }

        if (Input.GetButtonDown("AbilityHealth"))
        {
            if (mana.use(ManaCost))
            {
                hp.apply(HealingAmmount);
                startCooldown();
            }
        }
    }
    void startCooldown() {
        on_cooldown = true;
        StartCoroutine(doCooldownTimer());
    }
    IEnumerator doCooldownTimer() {
        float timer = Cooldown;
        while (timer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;
            CooldownUI.fillAmount = timer / Cooldown;
        }
        endCooldown();
    }
    void endCooldown() {
        on_cooldown = false;

    }
}
