using UnityEngine;

public abstract class Powerable : MonoBehaviour
{
    [SerializeField] int power = 0;
    [SerializeField] int powerThreshold = 1;
    private bool wasPowered;

    private void Start()
    {
        wasPowered = IsPowered();
    }

    /**
     * Runs whenever this object becomes powered.
     */
    abstract protected void OnPowered();
    /**
     * Runs whenever this object becomes not powered.
     */
    abstract protected void OnNotPowered();
    /**
     * Runs every frame this object is powered.
     */
    abstract protected void WhilePowered();
    /**
     * Runs every frame this object is not powered.
     */
    abstract protected void WhileNotPowered();

    /**
     * Increases the power level of the object.
     * Returns whether or not the object is powered.
     */
    public bool IncreasePower()
    {
        power++;
        if (!wasPowered && IsPowered())
        {
            OnPowered();
            wasPowered = true;
        }
        return wasPowered;
    }

    /**
     * 
     * Decreases the power level of the object.
     * Returns whether or not the object is powered.
     */
    public bool DecreasePower()
    {
        power--;
        if (wasPowered && !IsPowered())
        {
            OnNotPowered();
            wasPowered = false;
        }
        return wasPowered;
    }

    /**
     * Returns whether or not the object is powered.
     */
    public bool IsPowered()
    {
        return power >= powerThreshold;
    }

    /**
     * Returns the power level of the object.
     */
    public int GetPower()
    {
        return power;
    }

    /**
     * Returns the power threshold of the object.
     */
    public int GetThreshold()
    {
        return powerThreshold;
    }
    
    private void Update()
    {
        if (wasPowered)
            WhilePowered();
        else
            WhileNotPowered();
    }
}
