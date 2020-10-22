using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager Instance { get; private set; }

    void Awake()
    {
        if ( Instance != null && Instance != this )
        {
            Destroy( this );
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] Yarn.Unity.DialogueRunner yarnRunner;
    public Yarn.Unity.DialogueRunner YarnRunner
    {
        get => yarnRunner;
        private set => yarnRunner = value;
    }
    [SerializeField] Yarn.Unity.DialogueUI yarnUI;
    public Yarn.Unity.DialogueUI YarnUI
    {
        get => yarnUI;
        private set => yarnUI = value;
    }

}
