using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Root.Systems.Settings
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private bool debug;
        [Space]
        [SerializeField] private List<SliderData> sliderDataList = new List<SliderData>(4);

        private AudioMixer MasterMixer;

        [System.Serializable]
        public class SliderData
        {
            public Slider slider;
            public int defaultValue;
        }

        private void Awake() => MasterMixer = (AudioMixer)Resources.Load("MasterMixer");

        private void Start()
        {
            foreach (var sliderComponents in sliderDataList)
            {
                Slider slider = sliderComponents.slider;

                slider.onValueChanged.AddListener(delegate { UpdateMixer(slider); });
                UpdateMixer(slider);
            }

            LoadSettings();
        }

        private void UpdateMixer(Slider slider) => MasterMixer.SetFloat(slider.name, slider.value - 80);

        private void OnDisable() => SaveSettings();

        public void SetToDefaults()
        {
            foreach (var components in sliderDataList)
            {
                components.slider.value = components.defaultValue;
            }

            SaveSettings();
        }

        // ----- Saving and Loading -----

        private const string masterVolume = "MasterVolume";
        private const string musicVolume = "MusicVolume";
        private const string sfxVolume = "SfxVolume";
        private const string uiVolume = "UIVolume";

        private void SaveSettings()
        {
            PlayerPrefs.SetFloat(masterVolume, sliderDataList[0].slider.value);
            PlayerPrefs.SetFloat(musicVolume, sliderDataList[1].slider.value);
            PlayerPrefs.SetFloat(sfxVolume, sliderDataList[2].slider.value);
            PlayerPrefs.SetFloat(uiVolume, sliderDataList[3].slider.value);

            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            sliderDataList[0].slider.value = PlayerPrefs.GetFloat(masterVolume, sliderDataList[0].defaultValue);
            sliderDataList[1].slider.value = PlayerPrefs.GetFloat(musicVolume, sliderDataList[1].defaultValue);
            sliderDataList[2].slider.value = PlayerPrefs.GetFloat(sfxVolume, sliderDataList[2].defaultValue);
            sliderDataList[3].slider.value = PlayerPrefs.GetFloat(uiVolume, sliderDataList[3].defaultValue);
        }
    }
}