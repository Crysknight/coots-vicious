using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class ScoresMenu : MonoBehaviour
{
    [SerializeField] Transform table;

    void Start()
    {
        CreateScoresTable();
    }

    void CreateScoresTable() {
        string prevScore = PlayerPrefs.GetString("HighScore");
        Debug.Log($"prevScore: {prevScore}");
        if (prevScore == null)
        {
            return;
        }

        string[] entries = prevScore.Split(';');
        Array.Resize(ref entries, entries.Length - 1);
        if (entries.Length == 0)
        {
            return;
        }

        Array.Sort(entries, (a, b) => {
            int aScore = int.Parse(a.Split(':')[1]);
            int bScore = int.Parse(b.Split(':')[1]);
            return bScore.CompareTo(aScore);
        });

        foreach (string entry in entries)
        {
            if (entry == "")
            {
                return;
            }

            string[] parts = entry.Split(':');
            if (parts.Length == 0)
            {
                return;
            }

            string name = parts[0];
            string score = parts[1];
            CreateTextObject(name, table);
            CreateTextObject(score, table);
        }
    }

    void CreateTextObject(string text, Transform parent) {
        GameObject textObject = new GameObject("TextMeshPro");
        TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

        textObject.transform.SetParent(parent);

        textMeshPro.transform.localPosition = Vector3.zero;
        textMeshPro.transform.localRotation = Quaternion.identity;
        textMeshPro.transform.localScale = Vector3.one;
        textMeshPro.text = text;
        textMeshPro.fontSize = 20;
    }
}
