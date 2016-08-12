using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KillTextBehaviour : MonoBehaviour
{

    Vector2 originalPos;
    Vector2 outPos;
    Vector2 centralPos;

    RectTransform rect;
    Text text;

    Coroutine fxCoroutine;

    const string doubleKill = "Double Kill";
    const string multiKill = "Multi Kill";

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();
    }

    void Start()
    {
        originalPos = rect.anchoredPosition;
        outPos = Vector2.Scale(rect.anchoredPosition, new Vector2(-1, 1));
        centralPos = new Vector2(0, originalPos.y); // hacky with 0  
    }

    public void Double()
    {
        if (fxCoroutine != null)
            StopCoroutine(fxCoroutine);

        text.text = doubleKill;
        fxCoroutine = StartCoroutine(ShowKill());
    }

    public void Multi()
    {
        if (fxCoroutine != null)
            StopCoroutine(fxCoroutine);

        text.text = multiKill;
        fxCoroutine = StartCoroutine(ShowKill());
    }

    public IEnumerator ShowKill()
    {
        rect.anchoredPosition3D = originalPos;

        WaitForSeconds wait = new WaitForSeconds(0);
        float delta = 0;
        float interval = 0.05f;

        while (delta < 1)
        {
            rect.anchoredPosition = Vector2.Lerp(originalPos, centralPos, delta);
            delta += interval;

            if (delta >= 0.8f)
            {
                interval = 0.01f;
                Debug.Log("near centre: " + delta);
            }

            yield return wait;
        }

        Debug.Log("reached centre");
        delta = 0;

        while (delta < 1)
        {
            rect.anchoredPosition = Vector2.Lerp(centralPos, outPos, delta);
            delta += interval;

            if (delta >= 0.2f)
                interval = 0.05f;

            yield return wait;
        }
    }
}
