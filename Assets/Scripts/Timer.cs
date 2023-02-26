using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] Scheduler scheduler;
    [SerializeField] TMPro.TextMeshProUGUI textMesh;
    [SerializeField] int timeInSeconds;
    [SerializeField] GameEvent timerFinishedEvent;

    private bool isReady = false;
    private bool isTimerMessageReported = false;
    private float? startTime;

    public float GetCurrentTime() {
        if (startTime == null)
        {
            return 0f;
        }

        return scheduler.GetCurrentTime() - (float)startTime;
    }

    public void HandleGameStarted(Component sender, object data) {
        isReady = true;
        startTime = scheduler.GetCurrentTime();
    }

    private void Start() {
        textMesh.text = TimeToMessage((float)timeInSeconds);
    }

    private void Update() {
        if (!isReady)
        {
            return;
        }

        HandleTime();
    }

    private void HandleTime() {
        float restTime = (float)timeInSeconds - GetCurrentTime();

        if (restTime > 0)
        {
            textMesh.text = TimeToMessage(restTime);
        }
        else if (!isTimerMessageReported)
        {
            textMesh.text = TimeToMessage(0f);
            isTimerMessageReported = true;
            timerFinishedEvent.Raise(this, restTime);
        }
    }

    private string TimeToMessage(float time) {
        int ms = (int)Mathf.Floor(time % 1 * 1000);
        int sc = (int)Mathf.Floor(time % 60);
        int mn = (int)Mathf.Floor(time / 60);

        string mnString = mn.ToString().PadLeft(2, '0');
        string scString = sc.ToString().PadLeft(2, '0');
        string msString = ms.ToString().PadLeft(3, '0');

        return $"{mnString}:{scString}:{msString}";
    }
}
