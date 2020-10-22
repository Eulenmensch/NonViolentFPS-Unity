using UnityEngine;
using UnityEditor;

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
    [SerializeField] bool playerInTrigger;
    public bool PlayerInTrigger
    {
        get => playerInTrigger;
        set => playerInTrigger = value;
    }
    [SerializeField] GameObject dialogueContainer;
    public GameObject DialogueContainer
    {
        get => dialogueContainer;
        private set => dialogueContainer = value;
    }

    [Header( "Dialogue" )]
    [SerializeField] YarnProgram yarnDialogue;
    public YarnProgram YarnDialogue
    {
        get => yarnDialogue;
        set => yarnDialogue = value;
    }
    [SerializeField] string startNode;
    public string StartNode
    {
        get => startNode;
        set => startNode = value;
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
