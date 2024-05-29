using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public float dayLength;
    public float morningDuration;
    public float noonDuration;
    public float eveningDuration;
    public float nightDuration;
    public Light sunLight;
    public Color morningColor;
    public Color noonColor;
    public Color eveningColor;
    public Color nightColor;

    private float currentTime;
    private float sunRotation;
    private float t = 0f;

    private float day;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        //sunLight.transform.rotation = Quaternion.Euler(sunRotation, 0f, 0f);
        SetLightColor();

        if (currentTime > dayLength)
        {
            EndDay();
        }
    }

    private void SetLightColor()
    {
        t += Time.deltaTime / (dayLength * 60);

        sunLight.color = Color.Lerp(Color.white, Color.black, t);
        if (t >= 1)
        {
            t = 0f;
        }
    }

    private void EndDay()
    {

    }
}
