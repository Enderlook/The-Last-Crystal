using UnityEngine;

namespace CreaturesAddons
{
    public class Slash : Weapon, IAutomatedAttack
    {
        [SerializeField, Tooltip("Damage on hit.")]
        private float damage = 1;
        [SerializeField, Tooltip("Push strength on hit.")]
        private float pushStrength = 0;
        [SerializeField, Tooltip("Animation attack name.")]
        private string animationName;

        [Header("Setup")]
#pragma warning disable CS0649
        //[SerializeField]
        public RayCasting rayCasting;
        [Layer]
        public int layerToHit;
#pragma warning restore CS0649

        private Transform thisTransform;
        private Animator thisAnimator;
        private SpriteRenderer thisSprite;
        private Vector2 fliptSource;
        private Vector2 fliptDirection;
        private Vector2 startSource;
        private Vector2 startDirection;

        public bool TargetInRange => rayCasting.Raycast(1 << layerToHit);
        public bool AutoAttack { get; set; }

        public bool AttackIfIsReadyAndIfTargetInRange(float deltaTime = 0) => TargetInRange ? TryExecute(deltaTime) : Recharge(deltaTime);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0006:Incorrect message signature", Justification = "This isn't Unity method.")]
        public override void Init(Creature creature)
        {
            thisTransform = creature.Transform;
            thisAnimator = creature.animator;
            thisSprite = creature.sprite;
            startSource = rayCasting.source; // Save position of source
            startDirection = rayCasting.direction; // Save position of direction
            fliptSource = new Vector2(rayCasting.source.x * (-1), rayCasting.source.y); // Flip value x in source
            fliptDirection = new Vector2(rayCasting.direction.x * (-1), rayCasting.direction.y); // Flip value y in direction
            base.Init(creature);
        }

        protected override void Attack() => thisAnimator.SetTrigger(animationName);

        // HitTarget() is called through animation event "Attack" for a specific frame
        private void HitTarget()
        {
            Debug.Log("Invoke via event");
            RaycastHit2D[] raycastHits = rayCasting.RaycastAll(1 << layerToHit); // Ignore any layer that isn't layerToHit
            for (int n = 0; n < raycastHits.Length; n++)
            {
                Transform victim = raycastHits[n].transform;
                victim.transform.GetComponent<IPush>()?.Push(thisTransform.position, pushStrength);
                victim.transform.GetComponent<ITakeDamage>()?.TakeDamage(damage);
            }
        }

        private void AttackIfAutomated()
        {
            if (AutoAttack)
                AttackIfIsReadyAndIfTargetInRange();
        }

        public override bool Recharge(float deltaTime)
        {
            bool value = base.Recharge(deltaTime);
            AttackIfAutomated();
            return value;
        }

        public override void UpdateBehaviour(float deltaTime)
        {
            FlipRayCast();
            base.UpdateBehaviour(deltaTime);
            AttackIfAutomated();
        }

        private void FlipRayCast()
        {
            if (thisSprite.flipX)
            {
                rayCasting.source = fliptSource;
                rayCasting.direction = fliptDirection;
            }
            else
            {
                rayCasting.source = startSource;
                rayCasting.direction = startDirection;
            }
        }
    }
}