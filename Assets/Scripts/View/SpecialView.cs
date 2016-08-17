using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialView : GameElement
{

    private Image bar;
    private Text desc;
    private Image hl;

    private Color hideColor = new Color(1, 1, 1, 0);
    private Color fadeColor = new Color(1, 1, 1, 0.5f);
    private Color brightColor = Color.white;

    void Awake()
    {
        int childNo = transform.childCount;
        hl = transform.GetChild(0).GetComponent<Image>();
        bar = transform.GetChild(childNo - 2).GetComponent<Image>();
        desc = transform.GetChild(childNo - 1).GetComponent<Text>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnUpdateSpecial, (sender, param) => UpdateGaugeFill((float)param));
        this.RegisterListener(EventID.OnSpecialReady , (sender, param) => SpecialReady());
    }

    private void UpdateGaugeFill(float amount)
    {
        StartCoroutine(TweenFill(amount));
    }

    private IEnumerator TweenFill(float newAmount)
    {
        float oldAmount = bar.fillAmount;

        if (oldAmount != 1f)
        {
            WaitForSeconds wait = new WaitForSeconds(0);
            float delta = 0;

            StartCoroutine(HighlightBar(newAmount));

            while (bar.fillAmount < newAmount)
            {
                bar.fillAmount = Mathf.Lerp(oldAmount, newAmount, delta);
                desc.text = (bar.fillAmount * 100) + "%";
                delta += 0.1f;
                yield return wait;
            }
        }
    }

    private IEnumerator HighlightBar(float newAmount)
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        yield return ShineHighlight(0.05f);

        while (bar.fillAmount < newAmount)
        {
            yield return wait;
        }

        yield return DimHighlight(0.05f);
    }


    private void SpecialReady()
    {
        StartCoroutine(DescriptionSpecialEffect());

        StartCoroutine(HighlightSpecialEffect());
    }

    private IEnumerator HighlightSpecialEffect()
    {
        int blinkCount = 0;
        const int maxBlink = 10;

        while (blinkCount < maxBlink)
        {
            yield return ShineHighlight(0.2f);

            yield return DimHighlight(0.2f);

            blinkCount++;
        }
    }

    private IEnumerator DescriptionSpecialEffect()
    {
        yield return InitialBlink();

        // from here on similar to kill desc, could refactor

        WaitForSeconds wait = new WaitForSeconds(0);

        bool fade = false;

        Color desired;
        Color start;

        /*while (GameManager.model.special.IsReady())
        {
            if (fade)
            {
                desired = brightColor;
                start = fadeColor;
            }
            else
            {
                desired = fadeColor;
                start = brightColor;
            }

            float delta = 0;

            while (desc.color != desired)
            {
                desc.color = Color.Lerp(start, desired, delta);
                delta += 0.3f;
                yield return wait;
            }
        }*/
    }

    private IEnumerator InitialBlink()
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        int blinkTimes = 0;
        const int maxBlink = 6;

        bool fade = false;

        while (blinkTimes < maxBlink)
        {
            if (!fade)
            {
                desc.color = fadeColor;
                fade = true;
            }
            else
            {
                desc.color = brightColor;
                fade = false;
            }

            blinkTimes++;

            yield return wait;
        }
    }


    private IEnumerator ShineHighlight(float speed)
    {
        WaitForSeconds wait = new WaitForSeconds(0);
        float delta = 0;

        while (hl.color != brightColor)
        {
            hl.color = Color.Lerp(hideColor, brightColor, delta);
            delta += speed;
            yield return wait;
        }
    }

    private IEnumerator DimHighlight(float speed)
    {
        WaitForSeconds wait = new WaitForSeconds(0);
        float delta = 0;

        while (hl.color != hideColor)
        {
            hl.color = Color.Lerp(brightColor, hideColor, delta);
            delta += speed;
            yield return wait;
        }
    }

    
}
