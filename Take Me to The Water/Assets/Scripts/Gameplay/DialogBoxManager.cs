using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    public Button goNextSentence;
    [Header("Texts")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;


    private Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
        goNextSentence.onClick.AddListener(DisplayNextSentence);
    }

    public void StartDialog(Dialog dialog)
    {
        nameText.text = dialog.name;

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogText.text = sentence;

    }

    public void EndDialog()
    {

    }
}
