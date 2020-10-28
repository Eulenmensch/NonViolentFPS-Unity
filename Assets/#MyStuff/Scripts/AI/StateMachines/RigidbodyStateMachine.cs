using UnityEngine;

[RequireComponent( typeof( Rigidbody ) )]
public class RigidbodyStateMachine : StateMachine
{
    [SerializeField] private float jumpForce;
    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = value;
    }
    [SerializeField] private float turnTorque;
    public float TurnTorque
    {
        get => turnTorque;
        set => turnTorque = value;
    }

    public Rigidbody RigidbodyRef { get; private set; }
    public bool Grounded { get; set; }

    protected override void Start()
    {
        base.Start();
        RigidbodyRef = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Grounded = true;
    }
    private void OnCollisionExit(Collision other)
    {
        Grounded = false;
    }
}