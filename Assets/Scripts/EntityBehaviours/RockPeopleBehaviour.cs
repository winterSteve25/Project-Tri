using Sirenix.OdinInspector;
using UI.TextContents;
using UnityEngine;

namespace EntityBehaviours
{
    public class RockPeopleBehaviour : SelectableEntity
    {
        private static readonly int StartWalking = Animator.StringToHash("Start Walking");
        private static readonly int EndWalking = Animator.StringToHash("Stop Walking");

        [SerializeField, Required] 
        private Animator animator;
        
        protected override void Update()
        {
            base.Update();
            animator.SetTrigger(StartWalking);
            transform.position += Time.deltaTime * new Vector3(0, 3f, 0);
        }

        protected override TextContent BuildEntityPanel()
        {
            return TextContent.Titled("Rock People");
        }
    }
}