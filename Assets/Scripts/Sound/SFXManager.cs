using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Komponen Pemutar Suara")]
    public AudioSource sfxSource;

    [Header("Koleksi Audio Clip")]
    public AudioClip suaraKlik;      // Untuk tombol UI
    public AudioClip suaraKlik2;      // Untuk tombol UI
    public AudioClip suaraPop;       // Untuk kubus muncul / bintang muncul
    public AudioClip suaraSukses;    // Untuk saat medali bersinar / level selesai
    public AudioClip suaraTransisiPop;  // Untuk animasi kubus memudar/menghilang
    public AudioClip suaraTransisiWoosh;  // Untuk animasi kubus memudar/menghilang
    public AudioClip suaraArpyNoise1;  // Untuk animasi kubus memudar/menghilang
    public AudioClip suaraArpyNoise2;  // Untuk animasi kubus memudar/menghilang
    public AudioClip suaraArpyNoise3;  // Untuk animasi kubus memudar/menghilang
    public AudioClip suaraArpyNoise4;  // Untuk animasi kubus memudar/menghilang

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Mengambil data volume sfx terakhir (Default: 1.0)
            if (sfxSource != null) 
                sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi untuk UI Slider
    public void SetVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
    }

    // Fungsi pemanggil suara yang bisa dieksekusi dari mana saja
    public void MainkanClick()
    {
        if (suaraKlik != null) sfxSource.PlayOneShot(suaraKlik);
    }
    
    public void MainkanClick2()
    {
        if (suaraKlik2 != null) sfxSource.PlayOneShot(suaraKlik2);
    }

    public void MainkanPop()
    {
        if (suaraPop != null) sfxSource.PlayOneShot(suaraPop);
    }

    public void MainkanSukses()
    {
        if (suaraSukses != null) sfxSource.PlayOneShot(suaraSukses);
    }

    public void MainkanTransisiPop()
    {
        if (suaraTransisiPop != null) sfxSource.PlayOneShot(suaraTransisiPop);
    }
    
    public void MainkanTransisiWoosh()
    {
        if (suaraTransisiWoosh != null) sfxSource.PlayOneShot(suaraTransisiWoosh);
    }
    
    public void MainkanArpyNoise(int set)
    {
        if(set == 1)
        {
            if (suaraArpyNoise1 != null) sfxSource.PlayOneShot(suaraArpyNoise1);
        }
        else if(set == 2)
        {
            if (suaraArpyNoise2 != null) sfxSource.PlayOneShot(suaraArpyNoise2);
        }
        else if(set == 3)
        {
            if (suaraArpyNoise3 != null) sfxSource.PlayOneShot(suaraArpyNoise3);
        }
        else if(set == 4)
        {
            if (suaraArpyNoise4 != null) sfxSource.PlayOneShot(suaraArpyNoise4);
        }
    }
}