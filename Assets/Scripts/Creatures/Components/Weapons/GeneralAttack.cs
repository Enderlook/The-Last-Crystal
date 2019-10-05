using UnityEngine;

namespace CreaturesAddons
{
    public class GeneralAttack : Slash
    {
        [SerializeField, Tooltip("Strong attack animation.")]
        public string animationStateStrong;
        [SerializeField, Tooltip("Strong attack probability."), Range(0, 100)]
        private int probabilityStrongAttack;

        protected override void Attack()
        {
            if (thisAnimator == null)
                HitTarget();
            else if (string.IsNullOrEmpty(animationStateStrong))
                base.Attack();
            else
            {
                if (Random.value * 100 <= probabilityStrongAttack)
                    thisAnimator.SetTrigger(animationStateStrong);
                else
                    thisAnimator.SetTrigger(animationState);
            }
        }
    }
}