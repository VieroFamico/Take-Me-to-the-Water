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
        public Button panelButton;
        public MonoBehaviour panelScript;
    }

    public Panel[] panels;

    void Start()
    {
        InitializePanel();
        closeButton.onClick.AddListener(CloseDisplay);
    }
    private void InitializePanel()
    {
        foreach (Panel panel in panels)
        {
            panel.panelGameObject.SetActive(false);  
            if (panel.panelScript != null)
            {
                panel.panelScript.enabled = false;
            }
            panel.panelButton.onClick.AddListener(() => SwitchPanel(panel));
        }

        SwitchPanel(panels[0]);
    }
    public void SwitchPanel(Panel choosenPanel)
    {
        foreach (Panel panel in panels)
        {
            panel.panelGameObject.SetActive(false);
            if (panel.panelScript != null)
            {
                panel.panelScript.enabled = false;
            }
        }

        choosenPanel.panelGameObject.SetActive(true);
        if (choosenPanel.panelScript != null)
        {
            choosenPanel.panelScript.enabled = true;
            if(choosenPanel.panelScript.GetComponent<CraftingManager>())
            {
                choosenPanel.panelScript.GetComponent<CraftingManager>().UpdateUI();
            }
        }
    }
}
