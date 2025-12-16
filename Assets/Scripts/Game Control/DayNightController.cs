using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    // Total in-game minutes elapsed
    public float totalGameMinutes = 0f;

    // Global time of day (0-1; 0 = midnight, 0.5 = noon)
    public float timeOfDay = 0.5f;

    public int currentHour = 12;
    public int currentMinute = 0;

    public float cycleDurationMinutes = 30f;

    public Light sunLight;
    public Material skyboxMaterial;

    // Intensity of the sun at noon
    public float maxSunIntensity = 1.5f;

    // Minimum intensity at night
    public float minSunIntensity = 0.2f;

    // Colors of the sun at different times
    public Color dawnColor = new Color(1f, 0.7f, 0.4f);
    public Color noonColor = Color.white;
    public Color duskColor = new Color(1f, 0.4f, 0.2f);
    public Color nightColor = new Color(0.1f, 0.1f, 0.3f);

    public static DayNightController instance;

    public static DayNightController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DayNightController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("DayNightController");
                    instance = obj.AddComponent<DayNightController>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        LoadSkyboxMaterial();

        // Try to load saved time
        LoadTimeData();
    }

    // Load the skybox material from RenderSettings
    private void LoadSkyboxMaterial()
    {
        skyboxMaterial = RenderSettings.skybox;
        if (skyboxMaterial == null)
        {
            Debug.LogWarning("No skybox found!");
        }
    }

    void Start()
    {
        LoadTimeData();
        UpdateSunlight();
    }

    void Update()
    {
        // Log if no sunlight
        if (sunLight == null)
        {
            Debug.Log("No directional light.");
        }

        // Only advance time if game is not paused
        if (Time.timeScale > 0f)
        {
            AdvanceTime();

            SaveTimeData();
        }
    }

    // Advance in-game time
    private void AdvanceTime()
    {
        // Calculate elapsed time
        float gameMinutesPerSecond = 1440f / (cycleDurationMinutes * 60f);
        totalGameMinutes += Time.deltaTime * gameMinutesPerSecond;

        if (totalGameMinutes >= 1440f)
        {
            totalGameMinutes -= 1440f;
        }

        // Update
        UpdateTimeValues();
        UpdateSunlight();
    }

    // Update current time based on total game time
    private void UpdateTimeValues()
    {
        currentHour = Mathf.FloorToInt(totalGameMinutes / 60f) % 24;
        currentMinute = Mathf.FloorToInt(totalGameMinutes) % 60;
        timeOfDay = totalGameMinutes / 1440f;
    }

    // Update the sunlight rotation, intensity, and color based on time of day
    private void UpdateSunlight()
    {
        if (sunLight == null)
            return;

        // Sun's rotation
        float sunRotation = timeOfDay * 360f;
        sunLight.transform.rotation = Quaternion.AngleAxis(sunRotation, Vector3.right);

        // Calculate brightness
        float brightness = CalculateBrightness(timeOfDay);
        
        // Sun's intensity
        sunLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, brightness);

        // Sun's color
        Color sunColor = CalculateSunColor(timeOfDay);
        sunLight.color = sunColor;

        UpdateSkybox(brightness);
    }

    // Update skybox brightness based on sun brightness
    private void UpdateSkybox(float brightness)
    {
        float exposure = Mathf.Lerp(minSunIntensity, maxSunIntensity, brightness);
        skyboxMaterial.SetFloat("_Exposure", exposure);

        RenderSettings.ambientIntensity = exposure;
    }

    // Calculate brightness based on time of day
    private float CalculateBrightness(float time)
    {
        // tricknometry
        float sinCurve = Mathf.Sin(time * Mathf.PI);
        return Mathf.Max(0f, sinCurve);
    }

    // Determine light color based on time of day
    private Color CalculateSunColor(float time)
    {
        // Get time of day
        if (time < 0.25f)
        {
            float t = time / 0.25f;
            return Color.Lerp(nightColor, dawnColor, t);
        }
        else if (time < 0.5f)
        {
            float t = (time - 0.25f) / 0.25f;
            return Color.Lerp(dawnColor, noonColor, t);
        }
        else if (time < 0.75f)
        {
            float t = (time - 0.5f) / 0.25f;
            return Color.Lerp(noonColor, duskColor, t);
        }
        else
        {
            float t = (time - 0.75f) / 0.25f;
            return Color.Lerp(duskColor, nightColor, t);
        }
    }

    // Get current time of day
    public float GetTimeOfDay()
    {
        return timeOfDay;
    }

    // Get current hour
    public int GetCurrentHour()
    {
        return currentHour;
    }

    // Get current minute
    public int GetCurrentMinute()
    {
        return currentMinute;
    }

    /// Get total in-game minutes elapsed
    public float GetTotalGameMinutes()
    {
        return totalGameMinutes;
    }

    /// Save time data to PlayerPrefs
    public void SaveTimeData()
    {
        PlayerPrefs.SetFloat("GameTime_TotalMinutes", totalGameMinutes);
        PlayerPrefs.Save();
    }

    /// Load time data from PlayerPrefs
    private void LoadTimeData()
    {
        if (PlayerPrefs.HasKey("GameTime_TotalMinutes"))
        {
            totalGameMinutes = PlayerPrefs.GetFloat("GameTime_TotalMinutes");
            UpdateTimeValues();
        }
    }
}
