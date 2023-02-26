using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MenuTab {
    public string name;
    public Transform component;
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] List<MenuTab> menuTabs;
    [SerializeField] MenuTab activeTab;

    void Start() {
        CootsGameManager.Instance.PlayMusic("Menu");
        ActivateTab("main");
    }

    public void PlayGame() {
        CootsGameManager.Instance.LoadScene("Intro");
    }

    public void Main() {
        ActivateTab("main");
    }

    public void Options() {
        ActivateTab("options");
    }

    public void Levels() {
        ActivateTab("levels");
    }

    public void Scores() {
        ActivateTab("scores");
    }

    public void ActivateTab(string name) {
        if (activeTab != null && activeTab.name != "")
        {
            activeTab.component.gameObject.SetActive(false);
        }

        MenuTab menuTab = menuTabs.Find(tab => tab.name == name);
        if (menuTab != null)
        {
            activeTab = menuTab;
            menuTab.component.gameObject.SetActive(true);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
