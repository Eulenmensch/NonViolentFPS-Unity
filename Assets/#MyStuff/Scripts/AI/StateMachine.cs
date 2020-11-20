using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State StartState;
    [SerializeField] private float range;
    public float Range
    {
        get => range;
        set => range = value;
    }
    [SerializeField] private Transform lookAtTarget;
    public Transform LookAtTarget
    {
        get => lookAtTarget;
        set => lookAtTarget = value;
    }
    [SerializeField] private Transform head;
    public Transform Head
    {
        get => head;
        set => head = value;
    }
    [SerializeField] private bool playerInTrigger;
    public bool PlayerInTrigger
    {
        get => playerInTrigger;
        set => playerInTrigger = value;
    }
    [SerializeField] private GameObject dialogueContainer;
    public GameObject DialogueContainer
    {
        get => dialogueContainer;
        private set => dialogueContainer = value;
    }

    [Header( "Dialogue" )]
    [SerializeField] private YarnProgram yarnDialogue;
    public YarnProgram YarnDialogue
    {
        get => yarnDialogue;
        set => yarnDialogue = value;
    }
    [SerializeField] private string startNode;
    public string StartNode
    {
        get => startNode;
        set => startNode = value;
    }

    [Header("Hit Feedback")] [SerializeField]
    private MMFeedbacks mMFeedbacks;
    public MMFeedbacks MMFeedbacks
    {
        get => mMFeedbacks;
        set => mMFeedbacks = value;
    }

    [SerializeField] private State currentState;
    public GameObject Player { get; private set; }
    public List<Collision> ActiveCollisions { get; private set; }

    public bool interacted { get; set; }
    public bool hit { get; set; }
    public bool Waiting { get; set; }

    private void Awake()
    {
        currentState = StartState;
    }

    protected virtual void Start()
    {
        ActiveCollisions = new List<Collision>();
        Player = GameObject.Find( "FaceLookAtTarget" ); //TODO: This is terrible design, replace it with some actual code asap!
    }

    private void Update()
    {
        currentState.UpdateState( this );
    }

    public void TransitionToState(State _newState)
    {
        if ( _newState.GetType() != typeof( RemainInState ) )
        {
            currentState.Exit( this );
            currentState = _newState;
            currentState.Enter( this );
        }
    }

    protected virtual void OnCollisionEnter(Collision _other)
    {
        ActiveCollisions.Add(_other);
    }

    protected virtual void OnCollisionExit(Collision _other)
    {
        ActiveCollisions.Remove(_other);
    }

    public IEnumerator WaitForSeconds(float _seconds)
    {
        Waiting = true;
        yield return new WaitForSeconds(_seconds);
        Waiting = false;
        print("stopped waiting " + Waiting);
    }
}
