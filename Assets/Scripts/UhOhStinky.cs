using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UhOhStinky : MonoBehaviour
{
    [SerializeField] LayerMask coots;
    [SerializeField] GameEvent UhOhSpotted;

    bool isMessageSpawned = false;

    void Update()
    {
        if (!isMessageSpawned)
        {
            CheckCootsCollision();
        }
    }

    void CheckCootsCollision() {
        if (Physics.CheckSphere(transform.position, 1f, coots))
        {
            isMessageSpawned = true;
            MessageWithTime messageWithTime = new MessageWithTime();
            messageWithTime.message = "Uh Oh... But it wasn't me";
            messageWithTime.time = 4f;
            UhOhSpotted.Raise(this, messageWithTime);
        }
    }
}
