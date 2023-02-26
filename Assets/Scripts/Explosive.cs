using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Destructible
{
    private bool isExploded = false;

    protected override void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        if (hp <= 0 && !isExploded)
        {
            Explode();
        }
    }

    protected void Explode() {
        if (isExploded)
        {
            return;
        }

        Rigidbody parentRb = gameObject.GetComponent<Rigidbody>();
        parentRb.Sleep();
        foreach (Transform child in transform) {
            child.gameObject.AddComponent<Rigidbody>();

            foreach (Transform subchild in child.gameObject.transform) {
                subchild.gameObject.AddComponent<Rigidbody>();
            }
        }

        isExploded = true;
    }
}
