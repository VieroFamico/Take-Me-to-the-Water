using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayNightManager : MonoBehaviour
{
    public float dayLength = 15f;
    public float morningDuration = 4f;
    public float noonDuration = 3.5f;
    public float eveningDuration = 3.5f;
    public float nightDuration = 4f;
    public Color morningColor;
    public Color noonColor;
    public Color eveningColor;
    public Color nightColor;

    private float currentTime;
    private float percentageOfDayPassed = 0f;
    private float percentageOfPhasePassed = 0f;
    private int day = 0;
    private Light sunLight;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FindSunLight();
    }
    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        FindSunLight();
    }

    private void FindSunLight()
    {
        GameObject sunObject = GameObject.FindWithTag("SunLight");
        if (sunObject)
        {
            sunLight = sunObject.GetComponent<Light>();
        }
    }

    void Update()
    {
        UpdateDayCycle();
    }

    private void UpdateDayCycle()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayLength)
        {
            currentTime = 0f;
            day++;
            Debug.Log(day);
        }

        percentageOfDayPassed = currentTime / dayLength;
        sunLight.transform.rotation = Quaternion.Euler(20 + percentageOfDayPassed * (150 - 20), 0, 0);

        float phaseDuration = dayLength / 4f;
        if (currentTime <= morningDuration)
        {
            percentageOfPhasePassed = currentTime / morningDuration;
            sunLight.color = Color.Lerp(morningColor, noonColor, percentageOfPhasePassed);
            
        }
        else if (currentTime <= morningDuration + noonDuration)
        {
            percentageOfPhasePassed = (currentTime - morningDuration) / noonDuration;
            sunLight.color = Color.Lerp(noonColor, eveningColor, percentageOfPhasePassed);
        }
        else if (currentTime <= morningDuration + noonDuration + eveningDuration)
        {
            percentageOfPhasePassed = (currentTime - morningDuration - noonDuration) / eveningDuration;
            sunLight.color = Color.Lerp(eveningColor, nightColor, percentageOfPhasePassed);
        }
        else
        {
            percentageOfPhasePassed = (currentTime - morningDuration - noonDuration - eveningDuration) / nightDuration;
            sunLight.color = Color.Lerp(nightColor, Color.black, percentageOfPhasePassed);
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

}
