using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    private Light lightSource;

    void Start()
    {
        lightSource = GetComponent<Light>();
        StartCoroutine(FlickerEffect());
    }

    IEnumerator FlickerEffect()
    {
        while (true)
        {
            lightSource.intensity = Random.Range(2f, 3f);
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}