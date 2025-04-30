using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PanelFade : MonoBehaviour
{
    private bool faded = false;
    [SerializeField] private float timer = 3f; // Countdown before fade
    [SerializeField] private float duration = 1f; // Duration of fade

    CanvasGroup canGroup;

    private void Start()
    {
        timer = 2f;
        canGroup = GetComponent<CanvasGroup>();
        Debug.Log("still working");
        Time.timeScale = 1f;
        
        StartCoroutine(StartFadeAfterDelay());


       // if (itHappened == true)
       // {
         //   Destroy(panel);
       // }
    }

    private IEnumerator StartFadeAfterDelay()
    {
        
        float countdown = timer;
        while (countdown > 0)
        {
            Debug.Log("This is working");
            countdown -= Time.deltaTime;
            yield return null;
        }

        fade();

    }

    public void fade()
    {
        StartCoroutine(DoFade(canGroup.alpha, faded ? 1 : 0));
        faded = !faded;
        Debug.Log("this is still working");
    }

    public IEnumerator DoFade(float start, float end)
    {
        float counter = 0.0f;
        Debug.Log("Is this working");
        while (counter < duration)
        {
            counter += Time.deltaTime;
            canGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }
}
