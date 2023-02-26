using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MessageWithTime {
    public string message;
    public float? time;
}

public class Message : MonoBehaviour
{
    [SerializeField] Scheduler scheduler;
    [SerializeField] TMPro.TextMeshProUGUI textMesh;

    private string message;
    private float? removeTime;

    public float GetCurrentTime() {
        return scheduler.GetCurrentTime();
    }

    public void HandleMessageUpdated(Component sender, object data) {
        if (data is string)
        {
            SetMessage((string)data);
        }
        else if (data is MessageWithTime)
        {
            MessageWithTime messageWithTime = (MessageWithTime)data;
            SetMessage(messageWithTime.message, messageWithTime.time);
        }
    }

    private void SetMessage(string value, float? time = 1f) {
        message = value;

        textMesh.text = message;

        if (time != null && time != 0)
        {
            removeTime = GetCurrentTime() + time;
        }
    }

    private void Update() {
        HandleTime();
    }

    private void HandleTime() {
        if (removeTime != null && GetCurrentTime() > removeTime)
        {
            SetMessage("", 0);
            removeTime = null;
        }
    }
}
