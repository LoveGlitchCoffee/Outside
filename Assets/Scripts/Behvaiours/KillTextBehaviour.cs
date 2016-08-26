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

    [Header("Flashing")]
    public Color ShineColour;
    public Color DimColour;
    public float FlashSpeed = 0.1f;

    public int ChangeRounds = 6;

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

        this.RegisterListener(EventID.OnGameEnd , (sender, param) => ResetText());
    }

    private void ResetText()
    {
        rect.anchoredPosition3D = originalPos;
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
            }

            yield return wait;
        }

        //Debug.Log("reached centre");
        delta = 0;
        StartCoroutine(FlashText());
        yield return new WaitForSeconds(1);

        while (delta < 1)
        {
            rect.anchoredPosition = Vector2.Lerp(centralPos, outPos, delta);
            delta += interval;

            if (delta >= 0.2f)
                interval = 0.05f;

            yield return wait;
        }
    }

    IEnumerator FlashText()
    {
        WaitForSeconds wait = new WaitForSeconds(FlashSpeed);

        int flash = 0;

        text.color = DimColour;

        bool dim = true;

        while (flash < ChangeRounds)
        {
            Color desired;
            Color start;

            if (dim)
            {
                desired = ShineColour;
                start = DimColour;

                dim = false;
            }
            else
            {
                desired = DimColour;
                start = ShineColour;

                dim = true;
            }

            float delta = 0;

            while (text.color != desired)
            {
                text.color = Color.Lerp(start, desired, delta);
                delta += 0.3f;
                yield return wait;                
            }

            flash++;

            //Debug.Log("round " + flash);
        }
    }
}
