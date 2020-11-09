using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class ProjectileGun : MonoBehaviour, IGun
{
    [SerializeField] private float fireRate;
    [SerializeField] private float fireForce;
    [SerializeField] private GameObject[] projectileTypes;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform projectileContainer;
    [SerializeField] private Slider projectileSlider;
    [SerializeField] private bool invertScrollDirection;

    private float timer;
    private GameObject activeProjectile;

    private void Start()
    {
        activeProjectile = projectileTypes[0];
    }

    public void PrimaryMouseButtonEnter()
    {
        timer = fireRate;
    }
    public void PrimaryMouseButtonExit() { }
    public void PrimaryMouseButtonAction()
    {
        timer += Time.deltaTime;
        if (!(timer >= fireRate)) return;
        timer = 0;
        Shoot();
    }

    public void SecondaryMouseButtonEnter() { }
    public void SecondaryMouseButtonExit() { }
    public void SecondaryMouseButtonAction() { }

    public void ScrollWheelAction(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        int projectileCount = projectileTypes.Length - 1;
        int currentIndex = Array.IndexOf(projectileTypes, activeProjectile);

        if(_context.started)
        {
            int direction = Mathf.RoundToInt(input.y);
            direction = invertScrollDirection ? -direction : direction;

            if (currentIndex < projectileCount && currentIndex > 0)
            {
                activeProjectile = projectileTypes[currentIndex + direction];
            }
            else if (currentIndex == projectileCount)
            {
                if (direction > 0)
                {
                    activeProjectile = projectileTypes[0];
                }
                else if (direction < 0)
                {
                    activeProjectile = projectileTypes[currentIndex + direction];
                }
            }
            else if (currentIndex == 0)
            {
                if (direction < 0)
                {
                    activeProjectile = projectileTypes[projectileCount];
                }
                else if (direction > 0)
                {
                    activeProjectile = projectileTypes[1];
                }
            }
        }

        projectileSlider.value = (float)currentIndex / (float)projectileCount;
    }

    public void Shoot()
    {
        GameObject projectileSpace = Instantiate(activeProjectile, projectileSpawnPoint.position, Quaternion.identity, projectileContainer);
        PhysicsProjectile projectile = projectileSpace.GetComponentInChildren<PhysicsProjectile>();
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(Camera.main.transform.forward * fireForce, ForceMode.VelocityChange);
    }
}
