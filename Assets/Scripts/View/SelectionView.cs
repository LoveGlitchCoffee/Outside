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
    int lastChosen = -1;

    void Start()
    {
		this.RegisterListener(EventID.OnPressWeapon , (sender, param) => SwapChosenWeapon((int) param));
    }

    private void SwapChosenWeapon(int newChosen)
    {
        if (lastChosen != -1)
        {
            Weapons[lastChosen].sprite = NonSelected[lastChosen];
        }

		Weapons[newChosen].sprite = Selected[newChosen];

		lastChosen = newChosen;
    }
}
