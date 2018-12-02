using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int MaxHP = 100;

    public delegate void HealthDamagedEvent(int hp, int max);
    public delegate void DeathEvent(bool raise_undead);

    public event HealthDamagedEvent OnHealthDamaged;
    public event DeathEvent OnCharacterDeath;

    int hp;
    public int Hp{
        get { return hp; }
    }

    public void apply(int ammount, bool raise_undead = false) {
        if (hp <= 0) {
            return;
        }
        hp = Mathf.Clamp(hp + ammount, 0, MaxHP);
        if (OnHealthDamaged != null)
        {
            OnHealthDamaged(hp, MaxHP);
        }

        if (hp == 0 && OnCharacterDeath != null) {
            OnCharacterDeath(raise_undead);
        }
    }

    public void reset() {
        hp = MaxHP;
    }

    void Start() {
        hp = MaxHP;
    }
}
