using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScore : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI textMesh;
    [SerializeField] Score referenceScore;

    private int score;

    void Update() {
        if (score != referenceScore.GetScore())
        {
            score = referenceScore.GetScore();

            string scoreString = score.ToString();

            textMesh.text = scoreString;
        }
    }
}
