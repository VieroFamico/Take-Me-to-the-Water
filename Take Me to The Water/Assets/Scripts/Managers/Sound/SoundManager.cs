using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip buttonClickSound;
    // Start is called before the first frame update
    void Start()
    {
        Button[] allButton = FindObjectsOfType<Button>(true);
        foreach(Button button in allButton)
        {
            AudioSource buttonAudioSource = button.AddComponent<AudioSource>();
            buttonAudioSource.clip = buttonClickSound;
            buttonAudioSource.volume = 0.5f;
            button.onClick.AddListener(() => PlayButtonClick(buttonAudioSource));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayButtonClick(AudioSource audioSource)
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(buttonClickSound);
    }
}
