using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform menu;

    public void HandlePauseToggled(Component sender, object data) {
        if (data is bool)
        {
            SetPauseMenu((bool)data);
        }
    }

    void SetPauseMenu(bool active) {
        if (active)
        {
            menu.gameObject.SetActive(true);
        }
        else
        {
            menu.gameObject.SetActive(false);
        }
    }
}
