using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {
    public float MoveSpeed = 5f;
    public float BasicAttackSpeed = 0.25f;

    public Transform FireBall;
    public Transform FireBallBoosted;

    public Text health;

    bool damage_boosted = false;
    public bool DamageBoosted {
        set { damage_boosted = value; }
    }

    Rigidbody RB;
    Health hp;

    bool can_shoot = true;

    void Start() {
        RB = GetComponent<Rigidbody>();
        hp = GetComponent<Health>();

        hp.OnHealthDamaged += onHealthDamaged;
        health.text = hp.Hp + " / " + hp.MaxHP;
    }
	void Update () {

        Movement();
        basicAttack();
	}
    void Movement() {
        Vector3 input = new Vector3(Input.GetAxisRaw("MoveH"), 0, Input.GetAxisRaw("MoveV"));
        input = Vector3.ClampMagnitude(input, 1f);

        RB.velocity = input * MoveSpeed;
    }
    void basicAttack() {
        if (!can_shoot) {
            return;
        }

        Vector3 aim = new Vector3(Input.GetAxis("AimH"), 0, Input.GetAxis("AimV")).normalized;
        if (aim != Vector3.zero) {

            Transform projectile = Instantiate(damage_boosted ? FireBallBoosted : FireBall, transform.position, Camera.main.transform.rotation);
            projectile.GetComponent<Projectile>().setDirection(aim);

            can_shoot = false;

            Invoke("basicAttackCooldown", BasicAttackSpeed);
        }
    }
    void basicAttackCooldown()
    {
        can_shoot = true;
    }
    void onHealthDamaged(int hp, int max)
    {
        health.text = hp + " / " + max;
    }
}
