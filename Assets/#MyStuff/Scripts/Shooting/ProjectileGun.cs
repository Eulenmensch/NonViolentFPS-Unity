using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class ProjectileGun : MonoBehaviour, IGun
{
    [SerializeField] private float FireRate = 0;
    [SerializeField] private float FireForce = 0;
    [SerializeField] private GameObject[] ProjectileTypes = null;
    [SerializeField] private Transform ProjectileSpawnPoint = null;
    [SerializeField] private Transform ProjectileContainer = null;
    [SerializeField] private Slider ProjectileSlider = null;
    [SerializeField] private bool InvertScrollDirection;

    private bool Shooting = false;
    private float Timer = 0;
    private GameObject ActiveProjectile = null;

    private void Start()
    {
        ActiveProjectile = ProjectileTypes[0];
        Timer = FireRate;
    }

    /*private void Update()
    {
        if (Shooting)
        {
            Timer += Time.deltaTime;
            if (!(Timer >= FireRate)) return;
            Timer = 0;
            Shoot();
        }
    }*/

    public void PrimaryMouseButtonEnter() { }
    public void PrimaryMouseButtonExit() { }
    public void PrimaryMouseButtonAction()
    {
        Timer += Time.deltaTime;
        if (!(Timer >= FireRate)) return;
        Timer = 0;
        Shoot();
    }

    public void SecondaryMouseButtonEnter() { }
    public void SecondaryMouseButtonExit() { }
    public void SecondaryMouseButtonAction() { }

    public void ScrollWheelAction(Vector2 _direction)
    {
        Vector2 input = _direction;
        int projectileCount = ProjectileTypes.Length - 1;
        int currentIndex = Array.IndexOf(ProjectileTypes, ActiveProjectile);

        
        int direction = Mathf.RoundToInt(input.y);
        direction = InvertScrollDirection ? -direction : direction;

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

        ProjectileSlider.value = (float)currentIndex / (float)projectileCount;
    }

    public void Shoot()
    {
        GameObject projectileSpace = Instantiate(ActiveProjectile, ProjectileSpawnPoint.position, Quaternion.identity/*, ProjectileContainer*/);
        PhysicsProjectile projectile = projectileSpace.GetComponentInChildren<PhysicsProjectile>();
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(Camera.main.transform.forward * FireForce, ForceMode.VelocityChange);
    }

    /*public void ChangeProjectile(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        int projectileCount = ProjectileTypes.Length - 1;
        int currentIndex = Array.IndexOf(ProjectileTypes, ActiveProjectile);

        if (context.started)
        {
            int direction = Mathf.RoundToInt(input.y);
            direction = InvertScrollDirection ? -direction : direction;

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
    }*/
}
