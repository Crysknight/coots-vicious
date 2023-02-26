using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField] Transform menu;

    public void HandleTimerFinished(Component sender, object data) {
        ActivateMenu();
    }

    void ActivateMenu() {
        menu.gameObject.SetActive(true);
    }
}
