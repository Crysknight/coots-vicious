using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Buntable
{
    [SerializeField] protected int hp = 100;

    protected override void OnCollisionEnter(Collision collision) {
        if (!isReady || hp <= 0)
        {
            return;
        }

        int damage = CollisionToDamage(collision);
        Debug.Log($"damage: {damage}");

        int score = damage * scoreFromDamageAmplifier;
        score = score + CollisionToStylePoints(collision);

        ScoreIncremented.Raise(this, score);

        hp = hp - damage;
    }
}
