using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RainManagers : MonoBehaviour
{
    public ParticleSystem[] rainParticleSystems;
    public float minRainInterval = 18f;
    public float maxRainInterval = 22f;
    public float minRainDuration = 5f;
    public float maxRainDuration = 8f;
    public float maxEmissionRate = 2000f; // Adjust according to your particle system's settings

    private DayNightManager dayNightManager;
    private float rainStartTime;
    private float rainEndTime;
    private bool isRaining = false;
    private float emissionRateIncrement;
    private float currentTime;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FindRainParticleSystems();
        dayNightManager = GetComponent<DayNightManager>();
        ScheduleNextRain();
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
        FindRainParticleSystems();
    }

    private void FindRainParticleSystems()
    {
        GameObject[] rainObjects = GameObject.FindGameObjectsWithTag("Rain");
        Debug.Log(rainObjects.Length);
        rainParticleSystems = new ParticleSystem[rainObjects.Length];
        for (int i = 0; i < rainObjects.Length; i++)
        {
            rainParticleSystems[i] = rainObjects[i].GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime/60f;

        if (!isRaining && currentTime >= rainStartTime)
        {
            foreach(ParticleSystem rain in rainParticleSystems)
            {
                StartCoroutine(StartRain(rain));
            }
            
        }
        else if (isRaining && currentTime >= rainEndTime)
        {
            foreach (ParticleSystem rain in rainParticleSystems)
            {
                StartCoroutine(StopRain(rain));
            }
        }
    }

    private void ScheduleNextRain()
    {
        float nextRainInterval = Random.Range(minRainInterval, maxRainInterval);
        rainStartTime = currentTime + nextRainInterval;
        float rainDuration = Random.Range(minRainDuration, maxRainDuration);
        rainEndTime = rainStartTime + rainDuration;
        emissionRateIncrement = maxEmissionRate / (rainDuration / 2f); // Duration to ramp up and down
    }

    private IEnumerator StartRain(ParticleSystem rain)
    {
        isRaining = true;
        var emission = rain.emission;
        emission.enabled = true;
        float currentRate = 0f;
        rain.Play();
        while (currentRate < maxEmissionRate)
        {
            currentRate += emissionRateIncrement * Time.deltaTime / 60f;
            emission.rateOverTime = currentRate;
            yield return null;
        }
        emission.rateOverTime = maxEmissionRate;
    }

    private IEnumerator StopRain(ParticleSystem rain)
    {
        var emission = rain.emission;
        float currentRate = maxEmissionRate;
        while (currentRate > 0f)
        {
            currentRate -= emissionRateIncrement * Time.deltaTime;
            emission.rateOverTime = currentRate;
            yield return null;
        }
        emission.enabled = false;
        rain.Stop();
        isRaining = false;
        ScheduleNextRain();
    }

}
