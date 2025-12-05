using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI fullscreenModeText;
    [SerializeField] private TextMeshProUGUI resolutionPresetText;

    private int currentFullscreenModeIndex;
    private int currentResolutionPresetIndex;

    public static Action onSettingsChange;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SaveLoadManager.onDataLoaded += onDataLoadedCallback;
    }

    void OnDestroy()
    {
        SaveLoadManager.onDataLoaded -= onDataLoadedCallback;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void onDataLoadedCallback(SaveData saveData)
    {
        currentFullscreenModeIndex = (int)saveData.fullscreenMode;
        currentResolutionPresetIndex = (int)saveData.resolutionPreset;
        UpdateUI();
        ApplySettings();
    }

    public void FullscreenModeRightArrow()
    {
        currentFullscreenModeIndex++;
        if (currentFullscreenModeIndex >= System.Enum.GetValues(typeof(FullscreenMode)).Length)
        {
            currentFullscreenModeIndex = 0;
        }

        UpdateUI();
        ApplySettings();
        onSettingsChange?.Invoke();
    }

    public void FullscreenModeLeftArrow()
    {
        currentFullscreenModeIndex--;
        if (currentFullscreenModeIndex < 0)
        {
            currentFullscreenModeIndex = System.Enum.GetValues(typeof(FullscreenMode)).Length - 1;
        }
        UpdateUI();
        ApplySettings();
        onSettingsChange?.Invoke();
    }
    
    public void ResolutionPresetRightArrow()
    {
        currentResolutionPresetIndex++;
        if (currentResolutionPresetIndex >= System.Enum.GetValues(typeof(ResolutionPreset)).Length)
        {
            currentResolutionPresetIndex = 0;
        }
        UpdateUI();
        ApplySettings();
        onSettingsChange?.Invoke();
    }
    
    public void ResolutionPresetLeftArrow()
    {
        currentResolutionPresetIndex--;
        if (currentResolutionPresetIndex < 0)
        {
            currentResolutionPresetIndex = System.Enum.GetValues(typeof(ResolutionPreset)).Length - 1;
        }
        UpdateUI();
        ApplySettings();
        onSettingsChange?.Invoke();
    }

    private void UpdateUI()
    {
        FullscreenMode currentFullscreenMode = (FullscreenMode)currentFullscreenModeIndex;
        fullscreenModeText.text = GetFullscreenModeName(currentFullscreenMode);

        ResolutionPreset currentResolutionPreset = (ResolutionPreset)currentResolutionPresetIndex;
        resolutionPresetText.text = GetResolutionName(currentResolutionPreset);
    }

    private void ApplySettings()
    {
        ResolutionPreset preset = (ResolutionPreset)currentResolutionPresetIndex;
        Resolution res = GetResolutionFromPreset(preset);

        FullscreenMode fsMode = (FullscreenMode)currentFullscreenModeIndex;
        //unity fullscreen mode
        FullScreenMode mode = GetFsModeFromPreset(fsMode);

        Screen.SetResolution(res.width, res.height, mode);
    }

    private string GetResolutionName(ResolutionPreset resolutionPreset)
    {
        switch (resolutionPreset)
        {
            case ResolutionPreset.R_1280x720: return "1280 x 720";
            case ResolutionPreset.R_1920x1080: return "1920 x 1080";
            case ResolutionPreset.R_2560x1440: return "2560 x 1440";
            case ResolutionPreset.R_3840x2160: return "3840 x 2160";
        }
        return "unknown";
    }

    private string GetFullscreenModeName(FullscreenMode fullscreenMode)
    {
        switch (fullscreenMode)
        {
            case FullscreenMode.FullscreenWindowed: return "Fullscreen Windowed";
            case FullscreenMode.Windowed: return "Windowed";
        }
        return "unknown";
    }

    private Resolution GetResolutionFromPreset(ResolutionPreset preset)
    {
        switch (preset)
        {
            case ResolutionPreset.R_1280x720:
                return new Resolution { width = 1280, height = 720 };
            case ResolutionPreset.R_1920x1080:
                return new Resolution { width = 1920, height = 1080 };
            case ResolutionPreset.R_2560x1440:
                return new Resolution { width = 2560, height = 1440 };
            case ResolutionPreset.R_3840x2160:
                return new Resolution { width = 3840, height = 2160 };
        }

        return Screen.currentResolution;
    }
    
    private FullScreenMode GetFsModeFromPreset(FullscreenMode mode)
    {
        switch (mode)
        {
            case FullscreenMode.FullscreenWindowed:
                return FullScreenMode.FullScreenWindow;
            case FullscreenMode.Windowed:
                return FullScreenMode.Windowed;

        }

        return FullScreenMode.FullScreenWindow;
    }

    public ResolutionPreset GetResolutionPreset()
    {
        return (ResolutionPreset)currentResolutionPresetIndex;
    }
    
    public FullscreenMode GetFullscreenMode()
    {
        return (FullscreenMode)currentFullscreenModeIndex;
    }
}
