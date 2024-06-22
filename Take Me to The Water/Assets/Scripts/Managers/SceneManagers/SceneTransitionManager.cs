using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public Image transitionImage; // Reference to the UI Image

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(int sceneIndex)
    {
        StartCoroutine(Transition(sceneIndex));
    }

    private IEnumerator Transition(int sceneIndex)
    {
        // Move the image from the left to cover the screen
        float duration = 0.3f;
        float time = 0;
        RectTransform rectTransform = transitionImage.GetComponent<RectTransform>();
        Vector2 initialPosition = new Vector2(-rectTransform.rect.width, 0);
        Vector2 targetPosition = Vector2.zero;

        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;

        // Wait for 1 second
        yield return new WaitForSeconds(0.5f);

        // Load the new scene
        SceneManager.LoadScene(sceneIndex);

        // Move the image from the screen to the right
        time = 0;
        initialPosition = targetPosition;
        targetPosition = new Vector2(rectTransform.rect.width, 0);

        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
    }

}
