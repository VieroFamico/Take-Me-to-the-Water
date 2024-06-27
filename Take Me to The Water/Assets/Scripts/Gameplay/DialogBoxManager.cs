using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    public Button goNextSentence;
    public Animator animator;
    [Header("Texts")]
    public TextMeshProUGUI nameText;
    public Image npcPicture;
    public TextMeshProUGUI dialogText;


    private Queue<string> sentences;
    private string sentenceBeingTyped;
    private bool isTyping = false;
    void Start()
    {
        sentences = new Queue<string>();
        goNextSentence.onClick.AddListener(DisplayNextSentence);
    }

    public void StartDialog(Dialog dialog)
    {
        animator.SetBool("IsShowing", true);
        nameText.text = dialog.name;
        npcPicture.sprite = dialog.picture;

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(DelayUntilStart());
    }
    IEnumerator DelayUntilStart()
    {
        yield return new WaitForSeconds(0.3f);
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogText.text = sentenceBeingTyped;
            isTyping = false;
            sentenceBeingTyped = "";
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";
        sentenceBeingTyped = sentence;
        foreach (char c in sentence)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(0.01f);
        }
        isTyping = false;
        sentenceBeingTyped = "";
    }
    public void EndDialog()
    {
        animator.SetBool("IsShowing", false);
    }
}
