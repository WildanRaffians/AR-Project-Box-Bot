using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk pindah scene
using System.Collections;
using TMPro; // Wajib untuk TextMeshPro

public class PencapaianManager : MonoBehaviour
{
    // Nama scene level AR kamu (cek di Build Settings)
    [Header("Kubus")]
    public GameObject CardKubus;  //PopUp complite Level
    public TextMeshProUGUI titleKubus;
    public TextMeshProUGUI messageKubus;
    public StarAnimate[] bintangKuningKubus; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenKubus;
    public GameObject MedaliSilverKubus;
    public GameObject MedaliGoldKubus;
    
    [Header("Balok")]
    public GameObject CardBalok;  //PopUp complite Level
    public TextMeshProUGUI titleBalok;
    public TextMeshProUGUI messageBalok;
    public StarAnimate[] bintangKuningBalok; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenBalok;
    public GameObject MedaliSilverBalok;
    public GameObject MedaliGoldBalok;
    
    [Header("Prisma")]
    public GameObject CardPrisma;  //PopUp complite Level
    public TextMeshProUGUI titlePrisma;
    public TextMeshProUGUI messagePrisma;
    public StarAnimate[] bintangKuningPrisma; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenPrisma;
    public GameObject MedaliSilverPrisma;
    public GameObject MedaliGoldPrisma;
    
    [Header("Limas")]
    public GameObject CardLimas;  //PopUp complite Level
    public TextMeshProUGUI titleLimas;
    public TextMeshProUGUI messageLimas;
    public StarAnimate[] bintangKuningLimas; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenLimas;
    public GameObject MedaliSilverLimas;
    public GameObject MedaliGoldLimas;
    
    [Header("Tabung")]
    public GameObject CardTabung;  //PopUp complite Level
    public TextMeshProUGUI titleTabung;
    public TextMeshProUGUI messageTabung;
    public StarAnimate[] bintangKuningTabung; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenTabung;
    public GameObject MedaliSilverTabung;
    public GameObject MedaliGoldTabung;
    
    [Header("Kerucut")]
    public GameObject CardKerucut;  //PopUp complite Level
    public TextMeshProUGUI titleKerucut;
    public TextMeshProUGUI messageKerucut;
    public StarAnimate[] bintangKuningKerucut; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenKerucut;
    public GameObject MedaliSilverKerucut;
    public GameObject MedaliGoldKerucut;
    
    [Header("Bola")]
    public GameObject CardBola;  //PopUp complite Level
    public TextMeshProUGUI titleBola;
    public TextMeshProUGUI messageBola;
    public StarAnimate[] bintangKuningBola; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenBola;
    public GameObject MedaliSilverBola;
    public GameObject MedaliGoldBola;

    void Awake()
    {
        MedaliGoldKubus.SetActive(false);
        MedaliSilverKubus.SetActive(false);
        MedaliOrenKubus.SetActive(false);
        
        MedaliGoldBalok.SetActive(false);
        MedaliSilverBalok.SetActive(false);
        MedaliOrenBalok.SetActive(false);

        MedaliGoldPrisma.SetActive(false);
        MedaliSilverPrisma.SetActive(false);
        MedaliOrenPrisma.SetActive(false);

        MedaliGoldLimas.SetActive(false);
        MedaliSilverLimas.SetActive(false);
        MedaliOrenLimas.SetActive(false);

        MedaliGoldTabung.SetActive(false);
        MedaliSilverTabung.SetActive(false);
        MedaliOrenTabung.SetActive(false);

        MedaliGoldKerucut.SetActive(false);
        MedaliSilverKerucut.SetActive(false);
        MedaliOrenKerucut.SetActive(false);

        MedaliGoldBola.SetActive(false);
        MedaliSilverBola.SetActive(false);
        MedaliOrenBola.SetActive(false);
    }
    void Start()
    {
        //KUBUS
        int levelTerakhirKubus = SaveManager.Instance.AmbilLevelTertinggi("kubus");
        int jumlahBintangKubus = 0;
        if(levelTerakhirKubus == 4)
        {
            titleKubus.text = "Pakar Kubus";
            messageKubus.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangKubus = 3;
            MedaliGoldKubus.SetActive(true);
            MedaliSilverKubus.SetActive(false);
            MedaliOrenKubus.SetActive(false);
        }
        else if(levelTerakhirKubus == 3)
        {
            titleKubus.text = "Kawan Kubus";
            messageKubus.text = "Kamu telah paham cara mengisi dan menghitung volume kubus.";
            jumlahBintangKubus = 2;
            MedaliGoldKubus.SetActive(false);
            MedaliSilverKubus.SetActive(true);
            MedaliOrenKubus.SetActive(false);
        }
        else if(levelTerakhirKubus == 2)
        {
            titleKubus.text = "Kenal Kubus";
            messageKubus.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar kubus.";
            jumlahBintangKubus = 1;
            MedaliGoldKubus.SetActive(false);
            MedaliSilverKubus.SetActive(false);
            MedaliOrenKubus.SetActive(true);
        }
        else{
            titleKubus.text = "...";
            messageKubus.text = "Selesaikan level kubus";
            titleKubus.alpha = Mathf.Clamp01(0.45f);
            messageKubus.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangKubus = 0;
        }        
        
        //Balok
        int levelTerakhirBalok = SaveManager.Instance.AmbilLevelTertinggi("balok");
        int jumlahBintangBalok = 0;
        if(levelTerakhirBalok == 4)
        {
            titleBalok.text = "Pakar Balok";
            messageBalok.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangBalok = 3;
            MedaliGoldBalok.SetActive(true);
            MedaliSilverBalok.SetActive(false);
            MedaliOrenBalok.SetActive(false);
        }
        else if(levelTerakhirBalok == 3)
        {
            titleBalok.text = "Kawan Balok";
            messageBalok.text = "Kamu telah paham cara mengisi dan menghitung volume balok.";
            jumlahBintangBalok = 2;
            MedaliGoldBalok.SetActive(false);
            MedaliSilverBalok.SetActive(true);
            MedaliOrenBalok.SetActive(false);
        }
        else if(levelTerakhirBalok == 2)
        {
            titleBalok.text = "Kenal Balok";
            messageBalok.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar balok.";
            jumlahBintangBalok = 1;
            MedaliGoldBalok.SetActive(false);
            MedaliSilverBalok.SetActive(false);
            MedaliOrenBalok.SetActive(true);
        }
        else{
            titleBalok.text = "...";
            messageBalok.text = "Selesaikan level balok";
            titleBalok.alpha = Mathf.Clamp01(0.45f);
            messageBalok.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangBalok = 0;
        }
        
        //Prisma
        int levelTerakhirPrisma = SaveManager.Instance.AmbilLevelTertinggi("prisma segitiga");
        int jumlahBintangPrisma = 0;
        if(levelTerakhirPrisma == 4)
        {
            titlePrisma.text = "Pakar Prisma";
            messagePrisma.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangPrisma = 3;
            MedaliGoldPrisma.SetActive(true);
            MedaliSilverPrisma.SetActive(false);
            MedaliOrenPrisma.SetActive(false);
        }
        else if(levelTerakhirPrisma == 3)
        {
            titlePrisma.text = "Kawan Prisma";
            messagePrisma.text = "Kamu telah paham cara mengisi dan menghitung volume prisma.";
            jumlahBintangPrisma = 2;
            MedaliGoldPrisma.SetActive(false);
            MedaliSilverPrisma.SetActive(true);
            MedaliOrenPrisma.SetActive(false);
        }
        else if(levelTerakhirPrisma == 2)
        {
            titlePrisma.text = "Kenal Prisma";
            messagePrisma.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar prisma.";
            jumlahBintangPrisma = 1;
            MedaliGoldPrisma.SetActive(false);
            MedaliSilverPrisma.SetActive(false);
            MedaliOrenPrisma.SetActive(true);
        }
        else{
            titlePrisma.text = "...";
            messagePrisma.text = "Selesaikan level prisma";
            titlePrisma.alpha = Mathf.Clamp01(0.45f);
            messagePrisma.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangPrisma = 0;
            
        }
        
        //Limas
        int levelTerakhirLimas = SaveManager.Instance.AmbilLevelTertinggi("limas");
        int jumlahBintangLimas = 0;
        if(levelTerakhirLimas == 4)
        {
            titleLimas.text = "Pakar Limas";
            messageLimas.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangLimas = 3;
            MedaliGoldLimas.SetActive(true);
            MedaliSilverLimas.SetActive(false);
            MedaliOrenLimas.SetActive(false);
        }
        else if(levelTerakhirLimas == 3)
        {
            titleLimas.text = "Kawan Limas";
            messageLimas.text = "Kamu telah paham cara mengisi dan menghitung volume Limas.";
            jumlahBintangLimas = 2;
            MedaliGoldLimas.SetActive(false);
            MedaliSilverLimas.SetActive(true);
            MedaliOrenLimas.SetActive(false);
        }
        else if(levelTerakhirLimas == 2)
        {
            titleLimas.text = "Kenal Limas";
            messageLimas.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar Limas.";
            jumlahBintangLimas = 1;
            MedaliGoldLimas.SetActive(false);
            MedaliSilverLimas.SetActive(false);
            MedaliOrenLimas.SetActive(true);
        }
        else{
            titleLimas.text = "...";
            messageLimas.text = "Selesaikan level Limas";
            titleLimas.alpha = Mathf.Clamp01(0.45f);
            messageLimas.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangLimas = 0;
            
        }
        
        //Tabung
        int levelTerakhirTabung = SaveManager.Instance.AmbilLevelTertinggi("tabung");
        int jumlahBintangTabung = 0;
        if(levelTerakhirTabung == 4)
        {
            titleTabung.text = "Pakar Tabung";
            messageTabung.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangTabung = 3;
            MedaliGoldTabung.SetActive(true);
            MedaliSilverTabung.SetActive(false);
            MedaliOrenTabung.SetActive(false);
        }
        else if(levelTerakhirTabung == 3)
        {
            titleTabung.text = "Kawan Tabung";
            messageTabung.text = "Kamu telah paham cara mengisi dan menghitung volume Tabung.";
            jumlahBintangTabung = 2;
            MedaliGoldTabung.SetActive(false);
            MedaliSilverTabung.SetActive(true);
            MedaliOrenTabung.SetActive(false);
        }
        else if(levelTerakhirTabung == 2)
        {
            titleTabung.text = "Kenal Tabung";
            messageTabung.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar Tabung.";
            jumlahBintangTabung = 1;
            MedaliGoldTabung.SetActive(false);
            MedaliSilverTabung.SetActive(false);
            MedaliOrenTabung.SetActive(true);
        }
        else{
            titleTabung.text = "...";
            messageTabung.text = "Selesaikan level Tabung";
            titleTabung.alpha = Mathf.Clamp01(0.45f);
            messageTabung.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangTabung = 0;
            
        }
        
        //Kerucut
        int levelTerakhirKerucut = SaveManager.Instance.AmbilLevelTertinggi("kerucut");
        int jumlahBintangKerucut = 0;
        if(levelTerakhirKerucut == 4)
        {
            titleKerucut.text = "Pakar Kerucut";
            messageKerucut.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangKerucut = 3;
            MedaliGoldKerucut.SetActive(true);
            MedaliSilverKerucut.SetActive(false);
            MedaliOrenKerucut.SetActive(false);
        }
        else if(levelTerakhirKerucut == 3)
        {
            titleKerucut.text = "Kawan Kerucut";
            messageKerucut.text = "Kamu telah paham cara mengisi dan menghitung volume Kerucut.";
            jumlahBintangKerucut = 2;
            MedaliGoldKerucut.SetActive(false);
            MedaliSilverKerucut.SetActive(true);
            MedaliOrenKerucut.SetActive(false);
        }
        else if(levelTerakhirKerucut == 2)
        {
            titleKerucut.text = "Kenal Kerucut";
            messageKerucut.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar Kerucut.";
            jumlahBintangKerucut = 1;
            MedaliGoldKerucut.SetActive(false);
            MedaliSilverKerucut.SetActive(false);
            MedaliOrenKerucut.SetActive(true);
        }
        else{
            titleKerucut.text = "...";
            messageKerucut.text = "Selesaikan level Kerucut";
            titleKerucut.alpha = Mathf.Clamp01(0.45f);
            messageKerucut.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangKerucut = 0;
            
        }
        
        //Bola
        int levelTerakhirBola = SaveManager.Instance.AmbilLevelTertinggi("bola");
        int jumlahBintangBola = 0;
        if(levelTerakhirBola == 4)
        {
            titleBola.text = "Pakar Bola";
            messageBola.text = "Kamu telah menaklukkan rahasia jaring-jaring dan luas permukaan.";
            jumlahBintangBola = 3;
            MedaliGoldBola.SetActive(true);
            MedaliSilverBola.SetActive(false);
            MedaliOrenBola.SetActive(false);
        }
        else if(levelTerakhirBola == 3)
        {
            titleBola.text = "Kawan Bola";
            messageBola.text = "Kamu telah paham cara mengisi dan menghitung volume Bola.";
            jumlahBintangBola = 2;
            MedaliGoldBola.SetActive(false);
            MedaliSilverBola.SetActive(true);
            MedaliOrenBola.SetActive(false);
        }
        else if(levelTerakhirBola == 2)
        {
            titleBola.text = "Kenal Bola";
            messageBola.text = "Kamu telah berhasil mengenal bentuk dan kerangka dasar Bola.";
            jumlahBintangBola = 1;
            MedaliGoldBola.SetActive(false);
            MedaliSilverBola.SetActive(false);
            MedaliOrenBola.SetActive(true);
        }
        else{
            titleBola.text = "...";
            messageBola.text = "Selesaikan level Bola";
            titleBola.alpha = Mathf.Clamp01(0.45f);
            messageBola.alpha = Mathf.Clamp01(0.45f);
            jumlahBintangBola = 0;
            
        }

        
        CardKubus.transform.localScale = Vector3.zero;
        CardBalok.transform.localScale = Vector3.zero;
        CardPrisma.transform.localScale = Vector3.zero;
        CardLimas.transform.localScale = Vector3.zero;
        CardTabung.transform.localScale = Vector3.zero;
        CardKerucut.transform.localScale = Vector3.zero;
        CardBola.transform.localScale = Vector3.zero;
        
        CardKubus.SetActive(true);
        CardBalok.SetActive(true);
        CardPrisma.SetActive(true);
        CardLimas.SetActive(true);
        CardTabung.SetActive(true);
        CardKerucut.SetActive(true);
        CardBola.SetActive(true);
        
        StartCoroutine(ScalePopup(0.5f)); // Durasi 0.5 detik

        foreach(var s in bintangKuningKubus) s.gameObject.SetActive(false);
        foreach(var s in bintangKuningBalok) s.gameObject.SetActive(false);
        foreach(var s in bintangKuningPrisma) s.gameObject.SetActive(false);
        foreach(var s in bintangKuningLimas) s.gameObject.SetActive(false);
        foreach(var s in bintangKuningTabung) s.gameObject.SetActive(false);
        foreach(var s in bintangKuningKerucut) s.gameObject.SetActive(false);
        foreach(var s in bintangKuningBola) s.gameObject.SetActive(false);
        // StopAllCoroutines(); // Agar tidak tumpang tindih
        for (int i = 0; i < jumlahBintangKubus; i++)
        {
            bintangKuningKubus[i].PlayAnim(0.5f + (i * 0.2f));
        }
        
        for (int i = 0; i < jumlahBintangBalok; i++)
        {
            bintangKuningBalok[i].PlayAnim(0.5f + (i * 0.2f));
        }
        
        for (int i = 0; i < jumlahBintangPrisma; i++)
        {
            bintangKuningPrisma[i].PlayAnim(0.5f + (i * 0.2f));
        }
        for (int i = 0; i < jumlahBintangLimas; i++)
        {
            bintangKuningLimas[i].PlayAnim(0.5f + (i * 0.2f));
        }
        for (int i = 0; i < jumlahBintangTabung; i++)
        {
            bintangKuningTabung[i].PlayAnim(0.5f + (i * 0.2f));
        }
        for (int i = 0; i < jumlahBintangKerucut; i++)
        {
            bintangKuningKerucut[i].PlayAnim(0.5f + (i * 0.2f));
        }
        for (int i = 0; i < jumlahBintangBola; i++)
        {
            bintangKuningBola[i].PlayAnim(0.5f + (i * 0.2f));
        }
    
    }

    IEnumerator ScalePopup(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // RUMUS BOUNCE (Back Out Easing)
            float s = 1.70158f; // Kekuatan pantulan (semakin besar semakin membal)
            t = t - 1;
            float bounceValue = t * t * ((s + 1) * t + s) + 1;

            CardKubus.transform.localScale = Vector3.one * bounceValue;
            CardBalok.transform.localScale = Vector3.one * bounceValue;
            CardPrisma.transform.localScale = Vector3.one * bounceValue;
            CardLimas.transform.localScale = Vector3.one * bounceValue;
            CardTabung.transform.localScale = Vector3.one * bounceValue;
            CardKerucut.transform.localScale = Vector3.one * bounceValue;
            CardBola.transform.localScale = Vector3.one * bounceValue;

            yield return null;
        }

        CardKubus.transform.localScale = Vector3.one;
        CardBalok.transform.localScale = Vector3.one;
        CardPrisma.transform.localScale = Vector3.one;
        CardLimas.transform.localScale = Vector3.one;
        CardTabung.transform.localScale = Vector3.one;
        CardKerucut.transform.localScale = Vector3.one;
        CardBola.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(1f);
    }

    public void TombolKembali()
    {
        SFXManager.Instance.MainkanClick();

        SceneTransition.Instance.PindahScene("MainMenu");
    }
    
}