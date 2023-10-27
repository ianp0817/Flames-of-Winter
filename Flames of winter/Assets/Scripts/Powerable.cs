using UnityEngine;

public abstract class Powerable : MonoBehaviour
{
    [SerializeField] const bool startPowered = false;
    private bool powered = startPowered;

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
     * Sets the powered state of this object.
     */
    public void SetPower(bool power)
    {
        powered = power;
        if (powered)
            OnPowered();
        else
            OnNotPowered();
    }

    /**
     * Toggles the powered state of this object.
     */
    public bool TogglePower()
    {
        powered = !powered;
        if (powered)
            OnPowered();
        else
            OnNotPowered();
        return powered;
    }

    /**
     * Returns whether or not the object is powered.
     */
    public bool IsPowered()
    {
        return powered;
    }
    
    private void Update()
    {
        if (powered)
            WhilePowered();
        else
            WhileNotPowered();
    }
}
