using UnityEngine;
using System.Collections;

public class SpecialModel : MonoBehaviour
{

    int charge;
    const int maxCharge = 50;

    bool specialReady;

    // store missle force explosion here for now, should be in own class
	public float ExplosionForce;
    public float ExplosionRadius;

    void Start()
    {
        this.RegisterListener(EventID.OnNormalKill, (sender, param) => UpdateCharge(1));
        this.RegisterListener(EventID.OnDoubleKill, (sender, param) => UpdateCharge(10));
        this.RegisterListener(EventID.OnMultiKill, (sender, param) => UpdateCharge(25));

        this.RegisterListener(EventID.OnSpecialUsed , (sender, param) => ResetCharge());
    }

    private void UpdateCharge(int amount)
    {
        if (charge >= maxCharge)
            return;

        charge += amount;

        if (charge > maxCharge)
            charge = maxCharge;

        if (charge == maxCharge)
        {
            this.PostEvent(EventID.OnSpecialReady);
            specialReady = true;
        }

        this.PostEvent(EventID.OnUpdateSpecial, (float)charge / (float)maxCharge);
    }

    private void ResetCharge()
    {
        specialReady = false;
        charge = 0;
    }

    public bool IsReady()
    {
        return specialReady;
    }
}
