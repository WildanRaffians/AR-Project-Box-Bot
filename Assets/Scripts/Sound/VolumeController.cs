using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("UI Referensi")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    // Fungsi OnEnable berjalan otomatis setiap kali panel/popup ini diaktifkan (SetActive = true)
    void OnEnable()
    {
        // Set posisi tuas slider agar sesuai dengan data memori saat ini
        if (bgmSlider != null) 
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            
        if (sfxSlider != null) 
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }

    // Fungsi ini dihubungkan ke fitur OnValueChanged milik BGM Slider
    public void UbahVolumeBGM(float nilaiBaru)
    {
        if (BGMManager.Instance != null) 
            BGMManager.Instance.SetVolume(nilaiBaru);
    }

    // Fungsi ini dihubungkan ke fitur OnValueChanged milik SFX Slider
    public void UbahVolumeSFX(float nilaiBaru)
    {
        if (SFXManager.Instance != null) 
            SFXManager.Instance.SetVolume(nilaiBaru);
    }
}