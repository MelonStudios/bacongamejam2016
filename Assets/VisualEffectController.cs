using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class VisualEffectController : MonoBehaviour
{
    private float baseChromaticAberrationValue;
    private float baseBlurredCorners;

    private Coroutine chrom;
    private Coroutine blur;

    void Start()
    {
        baseChromaticAberrationValue = GetComponent<VignetteAndChromaticAberration>().chromaticAberration;
        baseBlurredCorners = GetComponent<VignetteAndChromaticAberration>().blur;
    }

    public void ChromaticAberration(float highValue, float toLowTime)
    {
        if (chrom != null) StopCoroutine(chrom);
        chrom = StartCoroutine(ChromAborration(highValue, toLowTime));
    }

    public void BlurredCorners(float highValue, float toLowTime)
    {
        if (blur != null) StopCoroutine(blur);
        blur = StartCoroutine(Blur(highValue, toLowTime));
    }

    IEnumerator ChromAborration(float highValue, float toLowTime)
    {
        float tempTime = 0;
        GetComponent<VignetteAndChromaticAberration>().chromaticAberration = highValue;

        while (tempTime <= toLowTime)
        {
            tempTime += Time.deltaTime;

            GetComponent<VignetteAndChromaticAberration>().chromaticAberration =
                Mathf.Lerp(highValue, baseChromaticAberrationValue, MathUtility.PercentageBetween(tempTime, 0, toLowTime));

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Blur(float highValue, float toLowTime)
    {
        float tempTime = 0;
        GetComponent<VignetteAndChromaticAberration>().blur = highValue;

        while (tempTime <= toLowTime)
        {
            tempTime += Time.deltaTime;

            GetComponent<VignetteAndChromaticAberration>().blur =
                Mathf.Lerp(highValue, baseBlurredCorners, MathUtility.PercentageBetween(tempTime, 0, toLowTime));

            yield return new WaitForEndOfFrame();
        }
    }
}