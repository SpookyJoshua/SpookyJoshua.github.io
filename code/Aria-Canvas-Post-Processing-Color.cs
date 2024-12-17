using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingColor : MonoBehaviour
{
    private PostProcessVolume volume;
    private ColorGrading colorGrading;
    public float currentSat;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        if (volume != null && volume.profile.TryGetSettings(out colorGrading))
        {
        }
        else
        {
            Debug.LogError("PostProcessVolume or ColorGrading not found!");
        }
    }

    private void Update()
    {
        currentSat = colorGrading.saturation.value;
    }

    public void ChangeSaturation(float targetSaturation, float duration)
    {
        StartCoroutine(AnimateSaturationChange(targetSaturation, duration));
    }

    public IEnumerator AnimateSaturationChange(float targetSaturation, float duration)
    {
        float startSaturation = colorGrading.saturation.value;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newSaturation = Mathf.Lerp(startSaturation, targetSaturation, time / duration);
            colorGrading.saturation.value = newSaturation;
            yield return null;
        }
        colorGrading.saturation.value = targetSaturation;
    }
}
