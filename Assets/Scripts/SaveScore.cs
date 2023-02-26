using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveScore : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Score referenceScore;

    private bool isScoreSaved = false;

    public void Save() {
        if (isScoreSaved)
        {
            return;
        }

        isScoreSaved = true;

        string name = inputField.text;
        if (name == "")
        {
            name = "DefinitelyNotLudwig";
        }

        int score = referenceScore.GetScore();

        string prevScore = PlayerPrefs.GetString("HighScore");
        prevScore += $"{name}:{score.ToString()};";

        PlayerPrefs.SetString("HighScore", prevScore);
    }
}
