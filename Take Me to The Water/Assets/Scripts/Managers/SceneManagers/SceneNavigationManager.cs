using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigationManager : MonoBehaviour
{
    public Button[] navigationButtons;

    private int choosenIndex;
    void Start()
    {
        for (int i = 0; i < navigationButtons.Length; i++)
        {
            int index = i + 1;
            navigationButtons[i].onClick.AddListener(() => SelectLocation(index));
        }
    }
    
    public void SelectLocation(int index)
    {
        choosenIndex = index;
    }
    public void ChangeScene()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneTransitionManager.Instance.TransitionToScene(currentBuildIndex + choosenIndex);
    }
}
