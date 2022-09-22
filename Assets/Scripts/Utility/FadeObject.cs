using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Lowers alpha channel then disables game object.
public class FadeObject : MonoBehaviour
{
    private float objectColorAlpha = 1;
    private bool fadeOut, fadeIn;
    // The object that we want to fade out
    public float fadeSpeed;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        Color objectColor = this.GetComponent<Renderer>().material.color;
        Debug.Log(objectColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut) {
            Color objectColor = this.GetComponent<Renderer>().material.color;
            objectColorAlpha = objectColor.a;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = objectColor;

            if (objectColor.a <= 0) {
                fadeOut = false;
                // After the object is invisible, disable it.
                Target.SetActive(false);
            }
        }
    }

    public void FadeOutObject() {
        fadeOut = true;
    }

    public void FadeInObject() {
        fadeIn = true;
    }
}
