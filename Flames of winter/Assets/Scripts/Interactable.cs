using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    /**
     * Interact with this object.
     * Receives the GameObject performing the interaction.
     * Returns whether or not the interaction was successful.
     */
    abstract public bool Interact(GameObject interactor);
}
