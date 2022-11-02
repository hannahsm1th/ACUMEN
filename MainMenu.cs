using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public void PlayGame() {
        HideMenu();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Gameplay"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Level 1-1", LoadSceneMode.Additive));
    }
    public void QuitGame() {
        Debug.Log("Quitting game.");
        Application.Quit();
    }
    public void HideMenu() {
        menu.SetActive(false);
    }
}
