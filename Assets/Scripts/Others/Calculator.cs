using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Used to calculate formulas. It can be either serialized in Unity inspector or construct using new.
/// </summary>
[System.Serializable]
public class Calculator
{
    [Tooltip("Formula to calculate.\nIt doesn't support operator precedence, instead use brackets.\nSupports string formating.")]
    public string formula;
    private Regex regex;

    private static readonly Dictionary<string, System.Func<float, float, float>> operators = new Dictionary<string, System.Func<float, float, float>>()
    {
        { "+", (float l, float r) => l + r },
        { "-", (float l, float r) => l - r },
        { "*", (float l, float r) => l * r },
        { "/", (float l, float r) => l / r },
        { "^", Mathf.Pow },
        { "log", Mathf.Log },
    };

    /// <summary>
    /// Construct a <see cref="Calculator"/> class.
    /// </summary>
    /// <param name="formula">Formula to calculate.<br/>It doesn't support operator precedence, instead use brackets.<br/>Supports string formating.</param>
    public Calculator(string formula)
    {
        MakeRegex();
        this.formula = formula;
    }

    /// <summary>
    /// Make regex object.
    /// </summary>
    private void MakeRegex()
    {
        string numberPattern = @"(\d+(?>\.?\,?\d+)?)";
        string pattern = @"\(?" + numberPattern + @"([+\-*\/^]|log)" + numberPattern + @"\)?";
        regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Calculate <seealso cref="formula"/> and using the given <paramref name="args"/> in the string formating.
    /// </summary>
    /// <param name="args">Arguments to use in the string formating <c>string.Format(<seealso cref="formula"/>, <paramref name="args"/>)</c></param>
    /// <returns></returns>
    public float Calculate(params float[] args)
    {
        if (regex == null)
            MakeRegex();
        if (string.IsNullOrEmpty(formula))
            formula = string.Join("", args);
        string toCalculate = string.Format(formula, args.Select(e => e.ToString()).ToArray());
        do
        {
            MatchEvaluator matchEvaluator = new MatchEvaluator(Replace);
            toCalculate = regex.Replace(toCalculate, matchEvaluator, 1);
        } while (regex.IsMatch(toCalculate));
        return float.Parse(toCalculate);
    }

    /// <summary>
    /// Performs a math operation with the captured groups of the regex, using the <seealso cref="operators"/>.
    /// </summary>
    /// <param name="match">Match from the regex.</param>
    /// <returns></returns>
    private string Replace(Match match)
    {
        GroupCollection groups = match.Groups;
        return operators[groups[2].Value](float.Parse(groups[1].Value), float.Parse(groups[3].Value)).ToString();
    }
}