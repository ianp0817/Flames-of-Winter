using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobInteract : MonoBehaviour
{
    [SerializeField] float interactRange = 2.0f;

    public void Interact()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactRange))
        {
            if (hit.transform.gameObject.CompareTag("BobInteractable"))
            {
                hit.transform.gameObject.GetComponent<Interactable>().Interact(gameObject);
            }
        }
    }
}
