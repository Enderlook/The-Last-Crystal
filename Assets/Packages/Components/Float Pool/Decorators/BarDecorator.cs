using Additions.Components.FloatPool.Internal;
using Additions.Prefabs.HealthBarGUI;

using System;

using UnityEngine;

namespace Additions.Components.FloatPool.Decorators
{
    [Serializable]
    public class BarDecorator : Decorator, IHealthBarViewer
    {
        [SerializeField, Tooltip("Bar used to show values.")]
        private HealthBar bar;
        public HealthBar Bar {
            get => bar;
            set {
                bar = value;
                bar.ManualUpdate(Current, Max);
            }
        }

        private void UpdateValues()
        {
            if (Bar != null)
                Bar.UpdateValues(Current, Max);
        }

        public override (float remaining, float taken) Increase(float amount, bool allowOverflow = false)
        {
            (float remaining, float taken) result = base.Increase(amount, allowOverflow);
            UpdateValues();
            return result;
        }

        public override (float remaining, float taken) Decrease(float amount, bool allowUnderflow = false)
        {
            (float remaining, float taken) result = base.Decrease(amount, allowUnderflow);
            UpdateValues();
            return result;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (Bar != null)
                Bar.ManualUpdate(Current, Max);
        }

        public bool IsVisible { get => ((IHealthBarViewer)Bar).IsVisible; set => ((IHealthBarViewer)Bar).IsVisible = value; }
        public bool IsEnabled { get => ((IHealthBarViewer)Bar).IsEnabled; set => ((IHealthBarViewer)Bar).IsEnabled = value; }
        public float HealthBarPercentFill => ((IHealthBarViewer)Bar).HealthBarPercentFill;
        public float? HealingBarPercentFill => ((IHealthBarViewer)Bar).HealingBarPercentFill;
        public float? DamageBarPercentFill => ((IHealthBarViewer)Bar).DamageBarPercentFill;
        public bool IsHealingBarPercentHide => ((IHealthBarViewer)Bar).IsHealingBarPercentHide;
        public bool IsDamageBarPercentHide => ((IHealthBarViewer)Bar).IsDamageBarPercentHide;
    }
}