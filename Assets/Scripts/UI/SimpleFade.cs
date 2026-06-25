using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class SimpleFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Fungsi untuk memunculkan (Fade In)
    public void Appear()
    {
        gameObject.SetActive(true);
        StartCoroutine(Fade(0, 1));
    }

    // Fungsi untuk menghilangkan (Fade Out)
    public void Disappear()
    {
        StartCoroutine(Fade(1, 0, () => {
            gameObject.SetActive(false); // Matikan objek setelah hilang
        }));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        onComplete?.Invoke();
    }
}