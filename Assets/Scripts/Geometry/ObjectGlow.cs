using UnityEngine;
using System.Collections.Generic;

public class ObjectGlow : MonoBehaviour
{
    private List<Material> targetMaterials = new List<Material>();
    
    [Header("Pengaturan Glow")]
    public Color warnaBersinar = Color.red;
    public float kecepatanPulsa = 3f;      
    public float intensitasMaksimum = 3f;  

    [Header("Pengaturan Transisi (Smooth Fade)")]
    [Tooltip("Semakin besar angkanya, semakin cepat proses fade in / fade out-nya")]
    public float kecepatanTransisi = 3f; 

    private bool isGlowing = false;
    private float currentWeight = 0f; // 0 = mati total, 1 = hidup maksimal berdenyut

    void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
            {
                Material mat = rend.material;
                mat.EnableKeyword("_EMISSION");
                targetMaterials.Add(mat);
            }
        }
    }

    void Update()
    {
        // 1. Tentukan target weight (1 jika aktif, 0 jika tidak aktif)
        float targetWeight = isGlowing ? 1f : 0f;
        
        // 2. Geser nilai currentWeight secara halus mendekati targetWeight
        currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, Time.deltaTime * kecepatanTransisi);

        // 3. Jalankan kalkulasi selama transisi belum menyentuh angka 0 murni
        if (currentWeight > 0f && targetMaterials.Count > 0)
        {
            // Rumus gelombang sinus yang kemarin
            float sinWave = Mathf.Sin(Time.time * kecepatanPulsa);
            float intensitasNormal = (sinWave + 1f) / 2f;          
            
            // KUNCI: Kalikan intensitas dengan currentWeight agar saat masuk/keluar nilainya tidak kaget
            float intensitas = intensitasNormal * intensitasMaksimum * currentWeight;

            Color warnaAkhir = warnaBersinar * Mathf.LinearToGammaSpace(intensitas);
            
            foreach (Material mat in targetMaterials)
            {
                if (mat != null)
                {
                    mat.SetColor("_EmissionColor", warnaAkhir);
                }
            }
        }
        else if (currentWeight == 0f && !isGlowing)
        {
            // Pastikan benar-benar padam total saat transisi keluar selesai sepenuhnya
            MatikanEmissionTotal();
        }
    }

    // Fungsi saklar yang dipanggil dari Coroutine Dialog kamu
    public void SetGlow(bool status)
    {
        isGlowing = status;
        // Kita hapus pemaksaan Color.black di sini agar proses meredupnya tidak terpotong kaku
    }

    private void MatikanEmissionTotal()
    {
        foreach (Material mat in targetMaterials)
        {
            if (mat != null)
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}