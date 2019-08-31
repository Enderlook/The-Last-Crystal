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
    [Tooltip("Should Regex be compiled.\nIncreases constructor time but decreases matching time. It's only worth with very heavy loads (~1M matches).")]
    public bool compile = false;
    private Regex regex;

    /// <summary>
    /// Formula to calculate.
    /// </summary>
    public string Formula {
        get => formula;
        set {
            formula = value;
            MakeRegex(compile);
        }
    }

    /// <summary>
    /// Whenever the regex object is compiled or not.
    /// </summary>
    public bool Compile {
        get => compile;
        set {
            if (compile != value)
            {
                compile = value;
                if (compile == true)
                    MakeRegex(compile);
            }
        }
    }

    private readonly Dictionary<string, System.Func<float, float, float>> operators = new Dictionary<string, System.Func<float, float, float>>()
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
    /// <param name="compile">Increases constructor time but decreases matching time. It's only worth with very heavy loads (~1M matches).</param>
    public Calculator(string formula, bool compile = false)
    {
        MakeRegex(compile);
        this.compile = compile;
        this.formula = formula;
    }

    /// <summary>
    /// Make regex object.
    /// <paramref name="compile"/>Whenever the regex object should be compiled or not. Compile it increases construction time but reduce matching time. Recomended for very heavy usage.<paramref name="compile"/>
    /// </summary>
    private void MakeRegex(bool compile = false)
    {
        string operatorsPattern = string.Join("|", operators.Keys.Select(e => @"\" + e));
        string numberPattern = @"(\d+(?>\.?\,?\d+)?)";
        string pattern = @"\(?" + numberPattern + @"(" + operatorsPattern + @")" + numberPattern + @"\)?";

        RegexOptions regexOptions = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;
        if (compile)
            regexOptions |= RegexOptions.Compiled;
        regex = new Regex(pattern, regexOptions);
    }

    /// <summary>
    /// Calculate <seealso cref="formula"/> and using the given <paramref name="args"/> in the string formating.
    /// </summary>
    /// <param name="args">Arguments to use in the string formating <c>string.Format(<seealso cref="formula"/>, <paramref name="args"/>)</c></param>
    /// <returns></returns>
    public float Calculate(params float[] args)
    {
        if (regex == null)
            MakeRegex(compile);
        string toCalculate = string.Format(formula, args);
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