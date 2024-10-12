using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShifter : MonoBehaviour
{
    [SerializeField] Material hueShader;
    [SerializeField] AnimationCurve hueCurve;
    private float newHue;

    private void Start()
    {
        hueShader.SetFloat("_Red", 1);
        hueShader.SetFloat("_Green", 0);
        hueShader.SetFloat("_Blue", 0);
    }

    private void Update()
    {
        Color newColor = Color.HSVToRGB(newHue, 1, 1);

        hueShader.SetFloat("_Red", newColor.r);
        hueShader.SetFloat("_Green", newColor.g);
        hueShader.SetFloat("_Blue", newColor.b);
    }

    public void HueShift()
    {
        StartCoroutine(HueShifting());
    }

    private IEnumerator HueShifting()
    {
        float time = 0;
        while (time < 0.2f)
        {
            if (newHue < 1)
            {
                newHue += Random.Range(2.2f,3.8f) * Time.deltaTime;
            }
            else
            {
                newHue = 0;
            }
            time += Time.deltaTime;
            yield return null;
        }
       
    }

}
