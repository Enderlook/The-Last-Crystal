using System.Text.RegularExpressions;

using UnityEngine;

namespace Additions.Utils
{
    public static class PrototypeHelper
    {
        private static readonly Regex regex = new Regex(@"^(?:\(Prototype(?: (\d+))?\) )?(.*)");

        public static string GetPrototypeNameOf(ScriptableObject original)
        {
            if (string.IsNullOrEmpty(original.name))
                return "(Prototype)";

            Match match;
            if ((match = regex.Match(original.name)).Success)
            {
                string name;
                int number = 2;
                if (match.Groups[1].Success)
                    number = int.Parse(match.Groups[0].Value) + 1;
                name = $"(Prototype {number})";
                if (match.Groups[2].Success)
                    name += " " + match.Groups[2].Value;
                return name;
            }
            else
                return "(Prototype) " + original.name;
        }
    }
}