using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private int mainMenuFlag = 0;
    private static Flag instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int GetMainMenuFlag()
    {
        return mainMenuFlag;
    }
    public void ActivateMainMenuFlag()
    {
        mainMenuFlag = 1;
    }
}
