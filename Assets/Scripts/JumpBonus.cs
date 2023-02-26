using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBonus : Bonus
{
    [SerializeField] GameEvent JumpBonusActivated;
    [SerializeField] float time = 20f;

    protected override void BonusTaken() {
        Debug.Log("BonusTaken");
        JumpBonusActivated.Raise(this, time);
    }
}
