using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigationManager : MonoBehaviour
{
    public Button[] navigationButtons;

    void Start()
    {
        for (int i = 0; i < navigationButtons.Length; i++)
        {
            int index = i + 1;
            navigationButtons[i].onClick.AddListener(() => ChangeScene(index));
        }
    }

    public void ChangeScene(int buildIndexIncrement)
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentBuildIndex + buildIndexIncrement);
    }

}
