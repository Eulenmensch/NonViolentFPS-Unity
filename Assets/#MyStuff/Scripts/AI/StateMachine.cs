using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] State StartState;
    [SerializeField] float range;
    public float Range
    {
        get => range;
        set => range = value;
    }
    [SerializeField] Transform lookAtTarget;
    public Transform LookAtTarget
    {
        get => lookAtTarget;
        set => lookAtTarget = value;
    }
    [SerializeField] Transform head;
    public Transform Head
    {
        get => head;
        set => head = value;
    }

    public State CurrentState { get; private set; }
    public GameObject Player { get; private set; }


    private void Awake()
    {
        CurrentState = StartState;
    }

    protected virtual void Start()
    {
        Player = GameObject.Find( "FaceLookAtTarget" ); //TODO: This is terrible design, replace it with some actual code asap!
    }

    private void Update()
    {
        CurrentState.UpdateState( this );
    }

    public void TransitionToState(State _newState)
    {
        if ( _newState.GetType() != typeof( RemainInState ) )
        {
            CurrentState.Exit( this );
            CurrentState = _newState;
            CurrentState.Enter( this );
        }
    }
}
