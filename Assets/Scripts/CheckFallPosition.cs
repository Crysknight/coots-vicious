using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFallPosition : MonoBehaviour
{
    int count = 0;
    bool positionChecked = false;

    void FixedUpdate() {
        if (positionChecked)
        {
            return;
        }

        count++;
        Debug.Log(count);
        if (count >= 50)
        {
            CheckPosition();
        }
    }

    private void CheckPosition() {
        Debug.Log(gameObject.name);
        Debug.Log("X");
        Debug.Log(gameObject.transform.position.x);
        Debug.Log("Y");
        Debug.Log(gameObject.transform.position.y);
        Debug.Log("Z");
        Debug.Log(gameObject.transform.position.z);

        positionChecked = true;
    }
}
