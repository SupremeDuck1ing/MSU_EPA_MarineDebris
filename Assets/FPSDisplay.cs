using System.Linq; 
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class FPSDisplay : MonoBehaviour
{
    public int FPS { get; private set; } 
    public TextMeshPro displayCurrent;

    // Update is called once per frame
    void Update()
    {
        float current = (int)(1f / Time.deltaTime); 
        if(Time.frameCount % 50 == 0) 
        displayCurrent.text = current.ToString() + " FPS";
    }
}
