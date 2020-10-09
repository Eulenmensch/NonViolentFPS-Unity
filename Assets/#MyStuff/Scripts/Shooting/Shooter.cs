using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    [SerializeField] float FireRate = 0;
    [SerializeField] float FireForce = 0;
    [SerializeField] GameObject[] ProjectileTypes = null;
    [SerializeField] Transform ProjectileSpawnPoint = null;
    [SerializeField] Transform ProjectileContainer = null;
    [SerializeField] Slider ProjectileSlider = null;

    private bool Shooting = false;
    private float Timer = 0;
    private GameObject ActiveProjectile = null;

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
        GameObject projectileSpace = Instantiate(ActiveProjectile, ProjectileSpawnPoint.position, Quaternion.identity/*, ProjectileContainer*/);
        Projectile projectile = projectileSpace.GetComponentInChildren<Projectile>();
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
        print(rigidBody);

        rigidBody.AddForce(Camera.main.transform.forward * FireForce, ForceMode.VelocityChange);
    }

    public void ChangeProjectile(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        int projectileCount = ProjectileTypes.Length - 1;
        int currentIndex = Array.IndexOf(ProjectileTypes, ActiveProjectile);

        if (context.started)
        {
            int direction = Mathf.RoundToInt(input.y);

            if (currentIndex < projectileCount && currentIndex > 0)
            {
                ActiveProjectile = ProjectileTypes[currentIndex + direction];
            }
            else if (currentIndex == projectileCount)
            {
                if (direction > 0)
                {
                    ActiveProjectile = ProjectileTypes[0];
                }
                else if (direction < 0)
                {
                    ActiveProjectile = ProjectileTypes[currentIndex + direction];
                }
            }
            else if (currentIndex == 0)
            {
                if (direction < 0)
                {
                    ActiveProjectile = ProjectileTypes[projectileCount];
                }
                else if (direction > 0)
                {
                    ActiveProjectile = ProjectileTypes[1];
                }
            }
        }

        ProjectileSlider.value = (float)currentIndex / (float)projectileCount;
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
