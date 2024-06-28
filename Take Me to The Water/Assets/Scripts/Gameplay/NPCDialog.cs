using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public Dialog dialog;

    public void TriggerDialog()
    {
        FindObjectOfType<DialogBoxManager>().StartDialog(dialog);
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FindObjectOfType<DialogBoxManager>().StartDialog(dialog);
        }
    }*/
}
