using UnityEngine;
using UnityEngine.UIElements;

public class FadeObject : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float fadeAmount;
    private float originalOpacity=255;
    [SerializeField] private bool doFade=false;
    Material Mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }
    private void FadeNow()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Mat = this.transform.GetChild(i).GetComponent<Renderer>().material;
            
            Color currentColor = Mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed));
            Mat.color = smoothColor;
        }
    }
    private void ResetFade()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Mat = this.transform.GetChild(i).GetComponent<Renderer>().material;

            Color currentColor = Mat.color;
            Debug.Log("Current Color: " + Mat.color);
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed));
            Debug.Log(smoothColor);
            
     
            Mat.color = smoothColor;
        }
    }
}
