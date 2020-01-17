using Additions.Attributes;
using Additions.Utils;
using Additions.Utils.ColorCombiner;

using System.Collections.Generic;

using UnityEngine;

namespace Additions.Components.ColorCombiner
{
    public class SpriteRendererColorer : MonoBehaviour, IInitialize, IColorAverager
    {
#pragma warning disable CS0649
        [Tooltip("Sprite to change color.")]
        public SpriteRenderer spriteRenderer;

        [Tooltip("Use Sprite Renderer color as default color.")]
        public bool useSpriteRendererDefaultColor;

        [SerializeField, Tooltip("Default color."), ShowIf(nameof(useSpriteRendererDefaultColor), goal: false)]
        private Color defaultColor = Color.white;

        [SerializeField, Tooltip("If true, it will automatically configure on Awake.")]
        private bool autoManage;
#pragma warning restore CS0649

        private readonly ColorAverager colorAverager = new ColorAverager();

        private void SetColorToSpriteRenderer() => spriteRenderer.color = GetColor();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            if (autoManage)
                Initialize();
        }

        public void Initialize() => DefaultColor = useSpriteRendererDefaultColor ? spriteRenderer.color : defaultColor;

        public Color GetColor() => colorAverager.GetColor();

        public void Add(Color color)
        {
            colorAverager.Add(color);
            SetColorToSpriteRenderer();
        }

        public bool Remove(Color color)
        {
            bool found = colorAverager.Remove(color);
            SetColorToSpriteRenderer();
            return found;
        }

        public bool Contains(Color color) => colorAverager.Contains(color);

        public void Clear()
        {
            colorAverager.Clear();
            SetColorToSpriteRenderer();
        }

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
    }
}