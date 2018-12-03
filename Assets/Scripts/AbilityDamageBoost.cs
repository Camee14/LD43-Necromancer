using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDamageBoost : MonoBehaviour {
    public int ManaCost = 5;
    public float Cooldown = 5f;
    public float Duration = 10f;

    public Image CooldownUI;

    Minions mana;
    PlayerControls player;
    bool on_cooldown = false;

    void Start()
    {
        mana = GetComponent<Minions>();
        player = GetComponent<PlayerControls>();
    }

    void Update()
    {
        if (on_cooldown)
        {
            return;
        }

        if (Input.GetButtonDown("AbilityDamageBoost"))
        {
            if (mana.use(ManaCost))
            {
                activate();
                startCooldown();
            }
        }
    }
    void activate()
    {
        player.DamageBoosted = true;
        Invoke("endAbility", Duration);
    }
    void endAbility() {
        player.DamageBoosted = false;
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
