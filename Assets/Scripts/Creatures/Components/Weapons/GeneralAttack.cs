using UnityEngine;

namespace Creatures.Weapons
{
    public class GeneralAttack : Slash
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Strong attack animation.")]
        private string animationStateStrong;

        [SerializeField, Tooltip("Strong attack probability."), Range(0, 100)]
        private int probabilityStrongAttack;
#pragma warning restore CS0649

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