using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManagers : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public Button settingsButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    private void StartGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneTransitionManager.Instance.TransitionToScene(nextSceneIndex);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void OpenSettings()
    {
        // Placeholder for opening settings
        Debug.Log("Settings button is pressed");
    }

}
