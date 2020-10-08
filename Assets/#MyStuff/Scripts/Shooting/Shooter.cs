using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] float FireRate = 0;
    [SerializeField] float FireForce = 0;
    [SerializeField] Projectile[] ProjectileTypes = null;
    [SerializeField] Transform ProjectileSpawnPoint = null;
    [SerializeField] Transform ProjectileContainer = null;

    private bool Shooting = false;
    private float Timer = 0;
    private Projectile ActiveProjectile = null;

    void Start()
    {
        ActiveProjectile = ProjectileTypes[0];
        Timer = FireRate;
    }

    void Update()
    {
        if (Shooting)
        {
            Timer += Time.deltaTime;
            if (Timer >= FireRate)
            {
                Timer = 0;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(ActiveProjectile.gameObject, ProjectileSpawnPoint.position, Quaternion.identity, ProjectileContainer);
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        print(rigidBody);

        rigidBody.AddForce(Camera.main.transform.forward * FireForce, ForceMode.VelocityChange);
    }

    public void ChangeProjectile(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (context.started)
        {
            int direction = Mathf.RoundToInt(input.y);
            print(direction);
            int currentIndex = Array.IndexOf(ProjectileTypes, ActiveProjectile);
            if (currentIndex < ProjectileTypes.Length - 1 && currentIndex > 0)
            {
                ActiveProjectile = ProjectileTypes[currentIndex + direction];
            }
            else if (currentIndex == ProjectileTypes.Length - 1 && direction > 0)
            {
                ActiveProjectile = ProjectileTypes[0];
            }
            else if (currentIndex == 0 && direction < 0)
            {
                ActiveProjectile = ProjectileTypes[ProjectileTypes.Length - 1];
            }
        }
    }

    public void GetShootInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Shooting = true;
        }
        if (context.canceled)
        {
            Shooting = false;
            Timer = FireRate;
        }
    }
}
