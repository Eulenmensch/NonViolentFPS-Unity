using System.Linq;
using UnityEngine;
using Yarn.Unity;

namespace NonViolentFPS.Manager
{
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton
        public static DialogueManager Instance { get; private set; }

        private void Awake()
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

        [SerializeField] private DialogueRunner yarnRunner;
        [SerializeField] private DialogueUI yarnUI;
        [SerializeField] private Transform canvasTransform;

        public void SetActiveDialogueContainer(GameObject _container)
        {
            yarnUI.dialogueContainer = _container;
        }

        public void StartDialogue(YarnProgram _yarnProgram ,string _startNode, Transform _attachmentPoint)
        {
            yarnRunner.Stop();
            yarnRunner.Clear();
            canvasTransform.parent = _attachmentPoint.transform;
            canvasTransform.localPosition = Vector3.zero;
            if ( !yarnRunner.yarnScripts.Contains( _yarnProgram ))
            {
                yarnRunner.Add( _yarnProgram );
            }
            yarnRunner.StartDialogue( _startNode );
        }
    }
}
