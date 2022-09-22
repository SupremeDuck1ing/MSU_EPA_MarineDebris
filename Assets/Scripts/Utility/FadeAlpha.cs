using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fades in and out until the script is disabled
/// </summary>
public class FadeAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    private Image imgComp;
    void Start()
    {
        imgComp = GetComponent<Image>();
        StartCoroutine(AlphaFade());
    }

    IEnumerator AlphaFade()
    {
        while(this.enabled)
        {
            // Fade out
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                imgComp.color = new Color(1, 1, 1, i);
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);
            // fade in
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                imgComp.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
