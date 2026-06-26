using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarAnimate : MonoBehaviour
{
    private Image starImage;
    private Vector3 originalScale;

    void Awake()
    {
        starImage = GetComponent<Image>();
        originalScale = Vector3.one * 0.66f; // Atau sesuaikan jika skala asli di Inspector bukan 1
    }

    public void PlayAnim(float delay)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateStar(delay));
    }

    IEnumerator AnimateStar(float delay)
    {
        // 1. Tunggu giliran (jika ada banyak bintang muncul bergantian)
        yield return new WaitForSeconds(delay);

        // 2. Set kondisi awal (Besar & Semi-Transparan)
        transform.localScale = originalScale * 1.5f;
        Color col = starImage.color;
        col.a = 0.5f;
        starImage.color = col;

        float durasi = 0.4f;
        float timer = 0;

        while (timer < durasi)
        {
            timer += Time.deltaTime;
            float t = timer / durasi;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            // 3. Animasi mengecil dan menjadi solid
            transform.localScale = Vector3.Lerp(originalScale * 1.5f, originalScale, smoothT);
            col.a = Mathf.Lerp(0.5f, 1.0f, smoothT);
            starImage.color = col;

            yield return null;
        }

        // Pastikan nilai akhir sempurna
        transform.localScale = originalScale;
        col.a = 1.0f;
        starImage.color = col;
    }
}