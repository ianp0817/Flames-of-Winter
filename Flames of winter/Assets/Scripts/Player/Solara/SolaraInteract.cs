using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolaraInteract : MonoBehaviour
{
    [SerializeField] float interactRange = 2.0f;

    public void Interact()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactRange))
        {
            if (hit.transform.gameObject.CompareTag("SolaraInteractable"))
            {
                hit.transform.gameObject.GetComponent<Interactable>().Interact(gameObject);
            }
        }
    }
}
