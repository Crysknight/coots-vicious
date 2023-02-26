using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Buntable : MonoBehaviour
{
    protected bool isReady = false;

    private int buntCount = 0;
    private readonly int defaultStylePointsForSecondaryCollision = 20;

    [SerializeField] protected int scoreFromDamageAmplifier = 20;
    [SerializeField] protected int stylePoints = 0;
    [SerializeField] protected float damageAmplifier = 1f;
    [SerializeField] private int maxBunt = 3;

    [Header("Events")]

    public GameEvent ScoreIncremented;

    protected void Start() {
        SetReadyAfterDelay();
    }

    protected async void SetReadyAfterDelay() {
        await Task.Delay(1000);

        isReady = true;
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        if (!isReady || buntCount > maxBunt)
        {
            return;
        }

        int damage = CollisionToDamage(collision);
        if (Mathf.Floor(damage) == 0)
        {
            return;
        }

        int score = damage * scoreFromDamageAmplifier;
        int collisionStylePoints = CollisionToStylePoints(collision);
        score += collisionStylePoints;

        ScoreIncremented.Raise(this, score);

        buntCount++;
    }

    protected int CollisionToDamage(Collision collision) {
        float impulseStrength = (
            Mathf.Abs(collision.impulse.x) +
            Mathf.Abs(collision.impulse.y) +
            Mathf.Abs(collision.impulse.z)
        );

        GameObject attacker = collision.gameObject;

        Buntable buntable = attacker.GetComponent<Buntable>();

        if (buntable != null && buntable.damageAmplifier > 1f)
        {
            impulseStrength = impulseStrength * buntable.damageAmplifier;
        }

        return (int)Mathf.Ceil(impulseStrength);
    }

    protected int CollisionToStylePoints(Collision collision) {
        GameObject attacker = collision.gameObject;
        int collisionStylePoints = 0;

        if (attacker.name != "Coots" && attacker.name != "Floor")
        {
            collisionStylePoints = defaultStylePointsForSecondaryCollision;
        }

        Buntable buntable = attacker.GetComponent<Buntable>();

        if (buntable != null && buntable.stylePoints > 0)
        {
            collisionStylePoints += buntable.stylePoints;
        }

        return collisionStylePoints;
    }
}
