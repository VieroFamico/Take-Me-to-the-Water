using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnHomeManager : MonoBehaviour
{
    public Button returnHomeButton;
    private int originalSceneIndex;

    void Start()
    {
        originalSceneIndex = 1;
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene(originalSceneIndex);
    }

}
