using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            
            audioSource = GetComponent<AudioSource>();
            
            // Mengambil data volume terakhir yang tersimpan di HP (Default: 0.5)
            audioSource.volume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // Fungsi ini akan dipanggil otomatis oleh UI Slider saat digeser
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            // Simpan perubahan ke memori HP
            PlayerPrefs.SetFloat("BGMVolume", volume); 
        }
    }
}