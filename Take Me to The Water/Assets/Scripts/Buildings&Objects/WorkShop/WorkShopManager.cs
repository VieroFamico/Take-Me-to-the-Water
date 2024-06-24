using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopManager : BuildingManager
{
    [System.Serializable]
    public class Panel
    {
        public GameObject panelGameObject;
        public MonoBehaviour panelScript;
    }

    public Panel[] panels;

    void Start()
    {
        // Deactivate all panels and their scripts at the start
        foreach (Panel panel in panels)
        {
            panel.panelGameObject.SetActive(false);
            if (panel.panelScript != null)
            {
                panel.panelScript.enabled = false;
            }
        }
        closeButton.onClick.AddListener(CloseDisplay);
    }

    public void SwitchPanel(int panelIndex)
    {
        // Deactivate all panels and their scripts
        foreach (Panel panel in panels)
        {
            panel.panelGameObject.SetActive(false);
            if (panel.panelScript != null)
            {
                panel.panelScript.enabled = false;
            }
        }

        // Activate the selected panel and its script
        panels[panelIndex].panelGameObject.SetActive(true);
        if (panels[panelIndex].panelScript != null)
        {
            panels[panelIndex].panelScript.enabled = true;
        }
    }
}
