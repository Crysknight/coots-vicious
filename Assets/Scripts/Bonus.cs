using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] Transform icon;

    protected void OnCollisionEnter(Collision collision) {
        GameObject picker = collision.gameObject;

        if (picker.name != "Coots")
        {
            return;
        }

        BonusTaken();

        gameObject.SetActive(false);
    }

    protected virtual void BonusTaken() {}
}
