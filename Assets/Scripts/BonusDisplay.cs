using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDisplay : MonoBehaviour
{
    [SerializeField] Scheduler scheduler;
    [SerializeField] Transform icon;
    [SerializeField] TMPro.TextMeshProUGUI textMesh;
    [SerializeField] GameEvent BonusDeactivatedEvent;

    bool isBonusActive = false;
    float? bonusActiveUntil;

    public float GetCurrentTime() {
        return scheduler.GetCurrentTime();
    }

    void Start() {
        DeactivateBonus();
    }

    void Update() {
        if (isBonusActive)
        {
            CheckBonusExpiry();
            UpdateTimer();
        }
    }

    public void HandleBonusActivated(Component sender, object data) {
        if (data is float)
        {
            ActivateBonus((float)data);
        }
    }

    void ActivateBonus(float time) {
        isBonusActive = true;
        bonusActiveUntil = GetCurrentTime() + time;
        icon.gameObject.SetActive(true);
    }

    void DeactivateBonus() {
        isBonusActive = false;
        bonusActiveUntil = null;
        icon.gameObject.SetActive(false);
        textMesh.text = "";
        BonusDeactivatedEvent.Raise(this, GetCurrentTime());
    }

    void CheckBonusExpiry() {
        if (GetCurrentTime() >= bonusActiveUntil)
        {
            DeactivateBonus();
        }
    }

    void UpdateTimer() {
        if (bonusActiveUntil == null)
        {
            return;
        }

        float timeLeft = (float)bonusActiveUntil - GetCurrentTime();
        textMesh.text = TimeToMessage(timeLeft);
    }

    private string TimeToMessage(float time) {
        int ms = (int)Mathf.Floor(time % 1 * 1000);
        int sc = (int)Mathf.Floor(time % 60);

        string scString = sc.ToString().PadLeft(2, '0');
        string msString = ms.ToString().PadLeft(3, '0');

        return $"{scString}:{msString}";
    }
}
