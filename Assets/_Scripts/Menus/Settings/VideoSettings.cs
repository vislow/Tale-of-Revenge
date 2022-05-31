using Root.Systems.Components;
using UnityEngine;

namespace Root.Systems.Settings
{
    public class VideoSettings : MonoBehaviour
    {
        [SerializeField] private bool debug;
        [Header("Components")]
        [SerializeField] private HorizontalSelectorController vSyncSelector;
        [SerializeField] private HorizontalSelectorController fullscreenSelector;
        [SerializeField] private HorizontalSelectorController resolutionSelector;

        private Resolution[] resolutions;

        private void Awake()
        {
            UpdateResolutionsList();

            vSyncSelector.OnIndexChanged += SetVsync;
            fullscreenSelector.OnIndexChanged += SetFullscreenMode;
            resolutionSelector.OnIndexChanged += SetResolution;
        }

        private void Start() => LoadSettings();

        private void OnDestroy()
        {
            vSyncSelector.OnIndexChanged -= SetVsync;
            fullscreenSelector.OnIndexChanged -= SetFullscreenMode;
            resolutionSelector.OnIndexChanged -= SetResolution;
        }

        // Vsync

        public void SetVsync() => QualitySettings.vSyncCount = vSyncSelector.CurrentIndex == 0 ? 1 : 0;

        // Fullscreen

        public void SetFullscreenMode()
        {
            switch (fullscreenSelector.CurrentIndex)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
            }
        }

        // Resolution 

        private void UpdateResolutionsList()
        {
            resolutionSelector.optionList.Clear();
            resolutions = Screen.resolutions;

            foreach (var res in resolutions)
            {
                resolutionSelector.optionList.Add($"{res.width} x {res.height} @ {res.refreshRate}");
            }

            resolutionSelector.CurrentIndex = resolutions.Length - 1;
        }

        public void SetResolution()
        {
            // Set Resolution
            Resolution newResolution = resolutions[resolutionSelector.CurrentIndex];
            Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreenMode, newResolution.refreshRate);
        }

        public void SetToDefaults()
        {
            vSyncSelector.CurrentIndex = 1;
            fullscreenSelector.CurrentIndex = 2;
            resolutionSelector.CurrentIndex = resolutionSelector.optionList.Count - 1;

            SaveSettings();
        }

        private void OnDisable() => SaveSettings();

        // ----- Saving and Loading -----

        private const string vSyncSetting = "vSyncEnabled";
        private const string fullscreenSetting = "FullscreenMode";
        private const string resolutionSetting = "Resolution";

        private void SaveSettings()
        {
            PlayerPrefs.SetInt(vSyncSetting, vSyncSelector.CurrentIndex);
            PlayerPrefs.SetInt(fullscreenSetting, fullscreenSelector.CurrentIndex);
            PlayerPrefs.SetInt(resolutionSetting, resolutionSelector.CurrentIndex);

            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            vSyncSelector.CurrentIndex = PlayerPrefs.GetInt(vSyncSetting, 1);
            fullscreenSelector.CurrentIndex = PlayerPrefs.GetInt(fullscreenSetting, 2);

            UpdateResolutionsList();
            resolutionSelector.CurrentIndex = PlayerPrefs.GetInt(resolutionSetting, resolutionSelector.optionList.Count - 1);
        }
    }
}