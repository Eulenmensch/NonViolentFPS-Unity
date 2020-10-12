using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticDrag : MonoBehaviour
{
    [SerializeField] float drag;                    //Quadratic force applied counter the board's velocity
    public float Drag
    {
        get { return drag; }
        set { drag = value; }
    }
    [SerializeField] private float angularDrag;     //Quadratic force applied counter the board's angular velocity
    public float AngularDrag
    {
        get { return angularDrag; }
        set { angularDrag = value; }
    }

    private Rigidbody RB;                           //A reference to the rigidbody

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (RB != null)
        {
            ApplyQuadraticDrag();
        }
    }

    private void ApplyQuadraticDrag()
    {
        //Apply translational drag
        RB.AddForce(-Drag * RB.velocity.normalized * RB.velocity.sqrMagnitude, ForceMode.Acceleration);
        //Apply rotational drag
        RB.AddTorque(-angularDrag * RB.angularVelocity.normalized * RB.angularVelocity.sqrMagnitude, ForceMode.Acceleration);
    }
}
