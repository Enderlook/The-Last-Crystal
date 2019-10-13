using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AdditionalExtensions;
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

    private static readonly Dictionary<string, System.Func<float, float, float>> operators = new Dictionary<string, System.Func<float, float, float>>()
    {
        { "+", (float l, float r) => l + r },
        { "-", (float l, float r) => l - r },
        { "*", (float l, float r) => l * r },
        { "/", (float l, float r) => l / r },
        { "^", (float l, float r) => Mathf.Pow(l, r) },
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
        IEnumerable<string> mathfMethods = typeof(Mathf).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(e =>
            {
                int parameters = e.GetParameters().Length;
                return parameters == 1 || parameters == 2;
            }).Select(e => e.Name.ToLower());
        string operatorsPattern = string.Join("|", operators.Keys.Select(e => @"\" + e).Concat(mathfMethods));
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
    private string Replace(Match m)
    {
        GroupCollection groups = m.Groups;
        string operation = groups[2].Value;
        float right = float.Parse(groups[3].Value);
        if (string.IsNullOrEmpty(groups[1].Value))
            return operation == "-" ? Compute(operation, 0f, right) : Compute(operation, right);
        else
        {
            float left = float.Parse(groups[1].Value);
            return operators.TryGetValue(operation, out System.Func<float, float, float> func)
                ? func(left, right).ToString() : Compute(operation, left, right);
        }
    }

    /// <summary>
    /// Compute current operations using a method from <see cref="Mathf"/> through reflection.
    /// </summary>
    /// <param name="nameOfMethod">Name of method to look for using reflection.</param>
    /// <param name="parameters">Parameters to pass in <paramref name="nameOfMethod"/> method.</param>
    /// <returns>Result of the <paramref name="nameOfMethod"/> method.</returns>
    private string Compute(string nameOfMethod, params float[] parameters)
    {
        string key = nameOfMethod.FirstCharToUpper();
        MethodInfo method = typeof(Mathf).GetMethod(key);
        if (method != null)
        {
            string Invoke(params float[] args)
            {
                object[] objects = new object[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    objects[i] = args[i];
                }
                return method.Invoke(typeof(Mathf), objects).ToString();
            }

            if (method.GetParameters().Length == parameters.Length)
                return Invoke(parameters);
            throw new System.Exception("The method found doesn't have a correct amount of arguments.");
        }
        throw new System.Exception($"No method found in {nameof(Mathf)} class called {key}.");
    }
}