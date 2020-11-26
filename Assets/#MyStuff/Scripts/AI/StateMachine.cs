using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private GameObject interactionPrompt;
    public GameObject InteractionPrompt
    {
        get => interactionPrompt;
        set => interactionPrompt = value;
    }

    [Header( "Dialogue" )]
    [SerializeField] private GameObject canvasAttachmentPoint;
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

    [Header("Hit Feedback")]
    [SerializeField] private MMFeedbacks mMFeedbacks;
    public MMFeedbacks MMFeedbacks
    {
        get => mMFeedbacks;
        set => mMFeedbacks = value;
    }

    private State currentState;
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
        Player = GameManager.Instance.Player;
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
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
    }

    public void StartDialogue(string _startNode)
    {
        DialogueManager.Instance.YarnRunner.Stop();
        DialogueManager.Instance.YarnRunner.Clear();
        // DialogueManager.Instance.CanvasTransform.position = canvasAttachmentPoint.transform.position;
        DialogueManager.Instance.CanvasTransform.parent = canvasAttachmentPoint.transform;
        DialogueManager.Instance.CanvasTransform.localPosition = Vector3.zero;
        if ( !DialogueManager.Instance.YarnRunner.yarnScripts.Contains( yarnDialogue ) )
        {
            DialogueManager.Instance.YarnRunner.Add( yarnDialogue );
        }
        DialogueManager.Instance.YarnRunner.StartDialogue( _startNode );
    }
}
