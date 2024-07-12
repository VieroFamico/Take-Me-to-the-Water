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
        originalSceneIndex = 0;
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    public void ReturnHome()
    {
        SceneTransitionManager.Instance.TransitionToScene(0);
    }

}
