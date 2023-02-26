using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI textMesh;

    private int score = 0;
    private bool isTimerFinished = false;

    public int GetScore() {
        return score;
    }

    public void HandleOnScoreIncremented(Component sender, object data) {
        if (data is int)
        {
            int value = (int)data;
            IncrementScore(value);
        }
    }

    public void HandleTimerFinished(Component sender, object data) {
        isTimerFinished = true;
    }

    private void IncrementScore(int value) {
        SetScore(score + value);
    }

    private void SetScore(int value) {
        if (isTimerFinished)
        {
            return;
        }

        score = value;

        string scoreString = score.ToString().PadLeft(12, '0');

        textMesh.text = scoreString;
    }
}
