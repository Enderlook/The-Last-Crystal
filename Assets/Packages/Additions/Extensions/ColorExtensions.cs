using UnityEngine;

namespace Additions.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Warps <paramref name="text"/> in HTML color tag with <paramref name="color"/>.<br>
        /// Do the same as <see cref="ColorizeWith(string, Color)"/>.
        /// </summary>
        /// <param name="color">Color to tint <paramref name="text"/>.</param>
        /// <param name="text">Text to by tinted by <paramref name="color"/>.</param>
        /// <returns>Text with HTML color tag.</returns>
        public static string GetColorTag(this Color color, string text) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";

        /// <summary>
        /// Warps <paramref name="text"/> in HTML color tag with <paramref name="color"/>..<br>
        /// Do the same as <see cref="GetColorTag(Color, string)"/>.
        /// </summary>
        /// <param name="color">Color to tint <paramref name="text"/>.</param>
        /// <param name="text">Text to by tinted by <paramref name="color"/>.</param>
        /// <returns>Text with HTML color tag.</returns>
        public static string ColorizeWith(this string text, Color color) => color.GetColorTag(text);

        /// <summary>
        /// Return the <paramref name="color"/> string version but colored with HTML color tag.
        /// </summary>
        /// <param name="color">Color to get string colored version.</param>
        /// <returns>String HTML colored version of <paramref name="color"/>.</returns>
        public static string ToColoredString(this Color color) => color.GetColorTag(color.ToString());
    }
}