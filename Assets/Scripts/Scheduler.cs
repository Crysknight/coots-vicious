using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SchedulerEvent {
    public float time;
    public GameEvent gameEvent;
}

[System.Serializable]
public struct SchedulerMessageEvent {
    public float time;
    public GameEvent gameEvent;
    public string message;
    public float messageTime;
}

public class Scheduler : MonoBehaviour
{
    [SerializeField] List<SchedulerEvent> events;
    [SerializeField] List<SchedulerMessageEvent> messageEvents;
    [SerializeField] GameEvent PauseToggled;

    private float currentTime = 0f;
    private bool isPaused = false;

    public float GetCurrentTime() {
        return currentTime;
    }

    private void Start() {
        CootsGameManager.Instance.PlayMusic("Game");
    }

    private void Update() {
        HandlePause();

        if (isPaused)
        {
            return;
        }

        currentTime += 1 * Time.deltaTime;

        HandleTime(currentTime);
    }

    private void HandlePause() {
        bool isInputPause = Input.GetButtonDown("Cancel");
        if (isInputPause)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            PauseToggled.Raise(this, isPaused);
        }
    }

    private void HandleTime(float time) {
        List<SchedulerEvent> schedulerEvents = events.FindAll(schedulerEvent => {
            return Mathf.Floor(schedulerEvent.time) == Mathf.Floor(time);
        });
        List<SchedulerMessageEvent> schedulerMessageEvents = messageEvents.FindAll(schedulerMessageEvent => {
            return Mathf.Floor(schedulerMessageEvent.time) == Mathf.Floor(time);
        });

        for (int i = 0; i < schedulerEvents.Count; i++)
        {
            SchedulerEvent schedulerEvent = schedulerEvents[i];
            schedulerEvents.Remove(schedulerEvent);
            schedulerEvent.gameEvent.Raise(this, time);
        }

        for (int i = 0; i < schedulerMessageEvents.Count; i++)
        {
            SchedulerMessageEvent schedulerMessageEvent = schedulerMessageEvents[i];
            schedulerMessageEvents.Remove(schedulerMessageEvent);
            MessageWithTime messageWithTime = new MessageWithTime();
            messageWithTime.message = schedulerMessageEvent.message;
            messageWithTime.time = schedulerMessageEvent.messageTime;
            schedulerMessageEvent.gameEvent.Raise(this, messageWithTime);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void MainMenu() {
        CootsGameManager.Instance.LoadScene("MainMenu");
    }

    public void PlayAgain() {
        CootsGameManager.Instance.LoadScene("LudsApt");
    }
}
