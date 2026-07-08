using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem; // Tambahkan ini di baris paling atas
using System.Collections.Generic;
public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    // Tarik (drag & drop) objek Panel Tutorial milikmu ke kolom ini di Inspector
    public GameObject tutorialPanel; 
    public GameObject tutorial1; 
    public GameObject tutorial2; 
    public GameObject tutorial3; 
    public GameObject tombol1; 
    public GameObject tombol2; 
    public GameObject tombol3; 
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = tutorialPanel.transform.localScale;
    }
    void Start()
    {
        // Jalankan pengecekan saat scene pertama kali dimuat
        CheckFirstTimePlay();
    }

    private void CheckFirstTimePlay()
    {
        // Mengecek key "IsFirstTime". Jika key belum pernah dibuat, kembalikan nilai 1 (True)
        int isFirstTime = PlayerPrefs.GetInt("IsFirstTime", 1);

        if (isFirstTime == 1)
        {
            // Munculkan tutorial
            StartCoroutine(SequencePlayTutorial()); 
        }
        else
        {
            // Sembunyikan tutorial
            tutorialPanel.SetActive(false);
        }
    }
    
    IEnumerator SequencePlayTutorial()
    {
        tutorial1.SetActive(true);
        tombol1.SetActive(true);

        tutorial2.SetActive(false);
        tutorial3.SetActive(false);
        tombol2.SetActive(false);
        tombol3.SetActive(false);

        tutorialPanel.SetActive(true);
        yield return StartCoroutine (ScalePopup(0.5f));
    }

    public void KeTutorial1()
    {
        tutorial1.SetActive(true);
        tombol1.SetActive(true);

        tutorial2.SetActive(false);
        tutorial3.SetActive(false);
        tombol2.SetActive(false);
        tombol3.SetActive(false);
    }
    public void KeTutorial2()
    {
        tutorial2.SetActive(true);
        tombol2.SetActive(true);

        tutorial1.SetActive(false);
        tutorial3.SetActive(false);
        tombol1.SetActive(false);
        tombol3.SetActive(false);
    }
    public void KeTutorial3()
    {
        tutorial3.SetActive(true);
        tombol3.SetActive(true);

        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tombol1.SetActive(false);
        tombol2.SetActive(false);
    }

    public void FinishTutorial()
    {
        StartCoroutine(SequenceFinishTutorial());
    }
    
    IEnumerator SequenceFinishTutorial()
    {
        // Tutup panel
        yield return StartCoroutine (HidePopup(0.25f));

        // Setel key "IsFirstTime" menjadi 0 agar tutorial tidak otomatis muncul lagi
        PlayerPrefs.SetInt("IsFirstTime", 0);
        
        // Simpan perubahan secara permanen ke dalam penyimpanan lokal perangkat
        PlayerPrefs.Save(); 
    }

    
    public void OpenTutorialFromSettings()
    {
        StartCoroutine(SequencePlayTutorial()); 
    }
    public void CloseTutorialFromSettings()
    {
        StartCoroutine(SeqCloseTutorialFromSettings()); 
    }
    IEnumerator SeqCloseTutorialFromSettings()
    {
        yield return StartCoroutine (HidePopup(0.25f));
    }

    IEnumerator ScalePopup(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // RUMUS BOUNCE (Back Out Easing)
            // Objek akan membesar sampai ~1.1 lalu kembali ke 1.0 secara smooth
            float s = 1.70158f; // Kekuatan pantulan (semakin besar semakin membal)
            t = t - 1;
            float bounceValue = t * t * ((s + 1) * t + s) + 1;

            
            tutorialPanel.transform.localScale = originalScale * bounceValue;
           

            yield return null;
        }

        
            
            tutorialPanel.transform.localScale = originalScale;
    }
    IEnumerator HidePopup(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            
            // Animasi mengecil biasa (SmoothStep)
            
                Vector3 startScale1 = originalScale;
                tutorialPanel.transform.localScale = Vector3.Lerp(startScale1, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            
            yield return null;
        }

        
            tutorialPanel.SetActive(false);
        
        
    }
}