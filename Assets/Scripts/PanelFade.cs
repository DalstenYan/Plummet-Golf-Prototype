using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PanelFade : MonoBehaviour
{
    private bool faded = false;
    [SerializeField] private float timer = 3f; // Countdown before fade
    [SerializeField] private float duration = 1f; // Duration of fade
    private bool itHappened = false;

    CanvasGroup canGroup;

    private void Start()
    {
        canGroup = GetComponent<CanvasGroup>();
        Debug.Log("still working");

        
        StartCoroutine(StartFadeAfterDelay());
    }

    private IEnumerator StartFadeAfterDelay()
    {
    
        float countdown = timer;
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            yield return null;
        }

        fade();
    }

    public void fade()
    {
        StartCoroutine(DoFade(canGroup.alpha, faded ? 1 : 0));
        faded = !faded;
    }

    public IEnumerator DoFade(float start, float end)
    {
        float counter = 0.0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }

        if (canGroup.alpha <= 0)
        {
            itHappened = true;
        }
    }
}
