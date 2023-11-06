using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobShoot : MonoBehaviour
{
    public bool hasCannon = false;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float power = 5f;
    [SerializeField] float offset = 0f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject bob;

    private float cooldownRemaining = 0f;
    private int blockers = 0;

    private void Update()
    {
        if (cooldownRemaining > 0f)
            cooldownRemaining -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (hasCannon && cooldownRemaining <= 0f && blockers == 0)
        {
            GameObject proj = Instantiate(projectilePrefab, offset * transform.forward + transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().parent = bob;
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.velocity = power * transform.forward;
            cooldownRemaining = cooldown;
        }
    }

    public void Block()
    {
        blockers++;
    }

    public void Unblock()
    {
        blockers--;
    }
}
