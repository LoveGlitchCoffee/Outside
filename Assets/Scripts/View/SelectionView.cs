using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectionView : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] NonSelected;
    public Sprite[] Selected;

    [Header("Buttons")]
    public Image[] Weapons;
    int lastChosen = 0;

    void Start()
    {
        this.RegisterListener(EventID.OnPressWeapon, (sender, param) => SwapChosenWeapon((int)param));

        Weapons[0].sprite = Selected[0];
    }

    private void SwapChosenWeapon(int newChosen)
    {
        Debug.Log("chose " + newChosen);

        Weapons[lastChosen].sprite = NonSelected[lastChosen];

        Weapons[newChosen].sprite = Selected[newChosen];

        lastChosen = newChosen;
    }

    public void Unlock(Chapter chap)
    {
        switch (chap)
        {
            case Chapter.NightTwo:
                {
                    Weapons[1].gameObject.SetActive(true);
                    break;
                }
            case Chapter.NightThree:
                {
                    Weapons[2].gameObject.SetActive(true);
                    break;
                }
        }
    }
}
