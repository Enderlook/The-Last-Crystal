using Additions.Components.FloatPool;
using Additions.Components.FloatPool.Decorators;
using Additions.Prefabs.HealthBarGUI;

using UnityEngine;

namespace Creatures
{
    public class CreatureWithEnergy : Creature
    {
#pragma warning disable CS0649
        [Header("Setup")]
        [SerializeField, Tooltip("Energy pool.")]
        private Pool energy;

        [SerializeField, Tooltip("Color tint for Sprite Renderer used when add energy.")]
        private Color addEnergySpriteColor = Color.green;

        [SerializeField, Tooltip("Color of floating text when add energy.")]
        private Color addEnergyTextColor = Color.green;
#pragma warning restore CS0649

        protected override void Awake()
        {
            base.Awake();
            energy.Initialize();
            updates.Add(energy);
        }

        public void SetEnergyBar(HealthBar energyBar) => energy.GetLayer<BarDecorator>().Bar = energyBar;

        public void AddEnergy(float amount, bool displayText = true, bool produceFeedback = true)
        {
            (_, float increased) = energy.Increase(amount);
            if (increased > 0)
            {
                if (produceFeedback)
                    AddColorTint(addEnergySpriteColor, .1f);
                if (displayText)
                    SpawnFloatingText(amount, addEnergyTextColor);
            }
        }
    }
}