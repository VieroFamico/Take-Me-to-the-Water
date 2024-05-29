using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public float dayLength = 15f;
    public float morningDuration = 4f;
    public float noonDuration = 3.5f;
    public float eveningDuration = 3.5f;
    public float nightDuration = 4f;
    public Light sunLight;
    public Color morningColor;
    public Color noonColor;
    public Color eveningColor;
    public Color nightColor;

    private float currentTime;
    private float sunRotation;
    private float t = 0f;

    private int day;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        sunRotation = 20f;
        day = 1;

    }

    // Update is called once per frame
    void Update()
    {
        float totalDuration = morningDuration + noonDuration + eveningDuration + nightDuration;
        currentTime += Time.deltaTime;

        // Determine which phase of the day we are in
        if (currentTime < morningDuration)
        {
            // Morning phase
            t = currentTime / morningDuration;
            sunLight.color = Color.Lerp(morningColor, noonColor, t);
            sunRotation = Mathf.Lerp(20f, 45f, t);
        }
        else if (currentTime < morningDuration + noonDuration)
        {
            // Noon phase
            t = (currentTime - morningDuration) / noonDuration;
            sunLight.color = Color.Lerp(noonColor, eveningColor, t);
            sunRotation = Mathf.Lerp(45f, 90f, t);
        }
        else if (currentTime < morningDuration + noonDuration + eveningDuration)
        {
            // Evening phase
            t = (currentTime - morningDuration - noonDuration) / eveningDuration;
            sunLight.color = Color.Lerp(eveningColor, nightColor, t);
            sunRotation = Mathf.Lerp(90f, 135f, t);
        }
        else if (currentTime < totalDuration)
        {
            // Night phase
            t = (currentTime - morningDuration - noonDuration - eveningDuration) / nightDuration;
            sunLight.color = Color.Lerp(nightColor, Color.black, t);
            sunRotation = Mathf.Lerp(135f, 150f, t);
        }
        else
        {
            // End of the day
            currentTime = 0f; // Reset the time
            day++; // Increment the day counter
        }

        // Apply the sun rotation
        sunLight.transform.rotation = Quaternion.Euler(sunRotation, 0f, 0f);

    }

    public int GetCurrentDay()
    {
        return day;
    }
}
