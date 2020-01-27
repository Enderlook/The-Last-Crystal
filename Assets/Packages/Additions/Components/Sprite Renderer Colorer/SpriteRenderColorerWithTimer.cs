using Additions.Attributes;
using Additions.Utils;
using Additions.Utils.ColorCombiner;

using System.Collections.Generic;

using UnityEngine;

namespace Additions.Components.ColorCombiner
{
    public class SpriteRenderColorerWithTimer : MonoBehaviour, IInitialize, IColorAveragerTimer
    {
#pragma warning disable CS0649
        [Tooltip("Sprite to change color.")]
        public SpriteRenderer spriteRenderer;

        [Tooltip("Use Sprite Renderer color as default color.")]
        public bool useSpriteRendererDefaultColor;

        [SerializeField, Tooltip("Default color."), ShowIf(nameof(useSpriteRendererDefaultColor), goal: false)]
        private Color defaultColor;

        [SerializeField, Tooltip("If true, it will automatically configure and manage.")]
        private bool autoManage;
#pragma warning restore CS0649

        private readonly ColorAveragerWithTimer colorAverager = new ColorAveragerWithTimer();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            if (autoManage)
                Initialize();
        }

        public void Initialize() => DefaultColor = useSpriteRendererDefaultColor ? spriteRenderer.color : defaultColor;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update()
        {
            if (autoManage)
                UpdateBehaviour(Time.deltaTime);
        }

        public void UpdateBehaviour(float deltaTime)
        {
            colorAverager.UpdateBehaviour(deltaTime);
            spriteRenderer.color = GetColor();
        }

        public Color GetColor() => colorAverager.GetColor();

        public void Add(Color color) => colorAverager.Add(color);

        public bool Remove(Color color) => colorAverager.Remove(color);

        public bool Contains(Color color) => colorAverager.Contains(color);

        public void Clear() => colorAverager.Clear();

        public int GetColorAmount(Color color) => colorAverager.GetColorAmount(color);

        public IEnumerable<KeyValuePair<Color, int>> GetColorAndAmounts() => colorAverager.GetColorAndAmounts();

        public IEnumerable<Color> GetColors() => colorAverager.GetColors();

        public IEnumerable<Color> GetUniqueColors() => colorAverager.GetUniqueColors();

        public bool HasChanged => colorAverager.HasChanged;

        public int Count => colorAverager.Count;

        public int UniqueColorsCount => colorAverager.UniqueColorsCount;

        public Color DefaultColor {
            get => colorAverager.DefaultColor;
            set => colorAverager.DefaultColor = value;
        }

        public void Add(Color color, float duration) => colorAverager.Add(color, duration);

        public bool Remove(Color color, float expirationTime) => colorAverager.Remove(color, expirationTime);

        public IEnumerable<(Color color, float expirationTime)> GetColorsAndExpirationTime() => colorAverager.GetColorsAndExpirationTime();
    }
}