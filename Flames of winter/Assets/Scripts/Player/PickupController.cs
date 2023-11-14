using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private Transform holdTarget;
    private GameObject heldObject;
    private Rigidbody heldObjectRB;
    private bool pickedUp = false;

    [SerializeField] private float pickupRange = 4.0f;
    [SerializeField] private float pickupForce = 10.0f;
    [SerializeField] private float dropDistance = 1.0f;
    [SerializeField] private float minDistance = 1.25f;

    public void Grab()
    {
        if (!heldObject)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickupRange))
            {
                GameObject hitObject = hit.transform.gameObject;
                if ((heldObjectRB = hitObject.GetComponent<Rigidbody>()) && (heldObjectRB.position - transform.parent.position).magnitude > minDistance)
                {
                    heldObjectRB.useGravity = false;
                    heldObjectRB.drag = 10;
                    heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

                    heldObject = hitObject;
                    heldObject.transform.parent = holdTarget;
                }
            }
        }
        else
            Drop();
    }

    public void Drop()
    {
        if (heldObject)
        {
            heldObjectRB.useGravity = true;
            heldObjectRB.drag = 1;
            heldObjectRB.constraints = RigidbodyConstraints.None;

            heldObject.transform.parent = null;
            heldObject = null;
            pickedUp = false;
        }
    }

    public void ProcessHold()
    {
        if (heldObject && Vector3.Distance(heldObject.transform.position, holdTarget.position) > 0.1f)
        {
            Vector3 moveDirection = holdTarget.position - heldObject.transform.position;
            Vector3 difference = heldObjectRB.position - transform.parent.position;
            bool drop = moveDirection.magnitude > dropDistance;

            Vector3 AB = holdTarget.position - transform.position;
            if ((drop && (pickedUp || (transform.position + Vector3.Dot(difference, AB) /
                Vector3.Dot(AB, AB) * AB - heldObjectRB.position).magnitude > dropDistance))
                || difference.magnitude <= minDistance)
                Drop();
            else
            {
                heldObjectRB.AddForce(moveDirection * pickupForce - heldObjectRB.GetAccumulatedForce());
                if (!pickedUp && !drop)
                    pickedUp = true;
            }
        }
    }
}
