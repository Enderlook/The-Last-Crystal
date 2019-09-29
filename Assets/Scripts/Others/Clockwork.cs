using UnityEngine;

public interface IBasicClockWork : CreaturesAddons.IUpdate
{
    /// <summary>
    /// Current cooldown time.
    /// </summary>
    ///
    float CooldownTime { get; }
    /// <summary>
    /// Total cooldown time.
    /// </summary>
    float TotalCooldown { get; }

    /// <summary>
    /// Cooldown percent from 0 to 1. When 0, it's ready to execute.
    /// </summary>
    float CooldownPercent { get; }

    /// <summary>
    /// Whenever it's ready or is still in cooldown.
    /// </summary>
    bool IsReady { get; }

    /// <summary>
    /// Reset <see cref="CooldownTime"/> time to maximum.
    /// </summary>
    void ResetCooldown();

    /// <summary>
    /// Reduce <see cref="CooldownTime"/> time and checks if the <see cref="CooldownTime"/> is over.
    /// </summary>
    /// <param name="deltaTime"><see cref="Time.deltaTime"/></param>
    /// <returns><see langword="true"/> if the weapon is ready to attack, <see langword="false"/> if it's on cooldown.</returns>

    bool Recharge(float deltaTime);
}

public interface IClockWork : IBasicClockWork
{
    /// <summary>
    /// Execute <see cref="Callback"/> and call <see cref="ResetCooldown"/>.<br/>
    /// It ignores the <see cref="IsReady"/>. Use <seealso cref="TryExecute(float)"/> to use it.
    /// </summary>
    /// <seealso cref="TryExecute(float)"/>
    void Execute();

    /// <summary>
    /// Try to execute <see cref="Callback"/>. It will check for the <see cref="CooldownTime"/>, and if possible, execute.
    /// </summary>
    /// <param name="deltaTime">Time since the last frame. <see cref="Time.deltaTime"/></param>
    /// <returns><see langword="true"/> if it was executed, <see langword="false"/> if it's still on cooldown.</returns>
    /// <seealso cref="Execute"/>
    bool TryExecute(float deltaTime = 0);
}

public interface IClockWork<T> : IClockWork
{
    /// <summary>
    /// Execute <see cref="Callback"/> and call <see cref="ResetCooldown"/>.<br/>
    /// It ignores the <see cref="IsReady"/>. Use <seealso cref="TryExecute(float)"/> to use it.
    /// </summary>
    /// <returns>The result of <see cref="Callback"/>.</returns>
    /// <seealso cref="TryExecute(float)"/>
    new T Execute();

    /// <summary>
    /// Try to execute <see cref="Callback"/>. It will check for the <see cref="CooldownTime"/>, and if possible, execute.
    /// </summary>
    /// <param name="deltaTime">Time since the last frame. <see cref="Time.deltaTime"/></param>
    /// <param name="result">The result of <see cref="Callback"/>.</param>
    /// <returns><see langword="true"/> if it was executed, <see langword="false"/> if it's still on cooldown.</returns>
    /// <seealso cref="Execute"/>
    bool TryExecute(ref T result, float deltaTime = 0);
}

public class Clockwork<T> : Clockwork, IClockWork<T>
{
    private System.Func<T> Callback;

    /// <summary>
    /// Create a timer that executes <paramref name="Callback"/> each <paramref name="cooldown"/> seconds.<br/>
    /// Time must be manually updated using <see cref="Recharge(float)"/>, <see cref="TryExecute(float)"/> or <see cref="TryExecute(ref T, float)"/> methods.
    /// </summary>
    /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
    /// <param name="Callback">Action to execute.</param>
    /// <param name="autoExecute">Whenever <see cref="Update(float)"/> must call <see cref="Execute"/> when <see cref="CooldownTime"/> is 0.</param>
    public Clockwork(float cooldown, System.Func<T> Callback, bool autoExecute) : base(cooldown, () => Callback(), autoExecute)
    {
        TotalCooldown = cooldown;
        ResetCooldown();
        this.Callback = Callback;
    }

    public bool TryExecute(ref T result, float deltaTime = 0)
    {
        if (Recharge(deltaTime))
        {
            result = Execute();
            return true;
        }
        return false;
    }

    public new T Execute()
    {
        ResetCooldown();
        return Callback();
    }
}

public class Clockwork : IClockWork
{
    private System.Action Callback;
    private bool autoExecute;
    public float CooldownTime {
        get => cooldownTime;
        private set {
            cooldownTime = value;
            if (cooldownTime < 0)
                cooldownTime = 0;
        }
    }
    protected float cooldownTime = 0f;
    public float TotalCooldown { get; protected set; }
    public float CooldownPercent => Mathf.Clamp01(CooldownTime / TotalCooldown);
    public bool IsReady => CooldownTime <= 0;

    /// <summary>
    /// Create a timer that executes <paramref name="Callback"/> each <paramref name="cooldown"/> seconds.<br/>
    /// Time must be manually updated using <see cref="Recharge(float)"/>, <see cref="TryExecute(float)"/> or <see cref="TryExecute(ref T, float)"/> methods.
    /// </summary>
    /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
    /// <param name="Callback">Action to execute.</param>
    /// <param name="autoExecute">Whenever <see cref="Update(float)"/> must call <see cref="Execute"/> when <see cref="CooldownTime"/> is 0.</param>
    public Clockwork(float cooldown, System.Action Callback, bool autoExecute)
    {
        TotalCooldown = cooldown;
        ResetCooldown();
        this.Callback = Callback;
        this.autoExecute = autoExecute;
    }

    public void Execute()
    {
        ResetCooldown();
        Callback();
    }

    public bool TryExecute(float deltaTime = 0)
    {
        if (Recharge(deltaTime))
        {
            Execute();
            return true;
        }
        return false;
    }

    public void ResetCooldown() => CooldownTime = TotalCooldown;
    public bool Recharge(float deltaTime)
    {
        CooldownTime -= deltaTime;
        return IsReady;
    }

    /// <summary>
    /// Calls <see cref="Recharge(float)"/>. If returns <see langword="true"/> and <see cref="autoExecute"/> is <see langword="true"/> it calls <see cref="Execute"/>.
    /// </summary>
    /// <param name="deltaTime">Time since last increase.</param>
    public void Update(float deltaTime)
    {
        if (Recharge(deltaTime) && autoExecute)
            Execute();
    }
}

public class BasicClockwork : IBasicClockWork
{
    public float CooldownTime {
        get => cooldownTime;
        private set {
            cooldownTime = value;
            if (cooldownTime < 0)
                cooldownTime = 0;
        }
    }
    protected float cooldownTime = 0f;
    public float TotalCooldown { get; protected set; }
    public float CooldownPercent => Mathf.Clamp01(CooldownTime / TotalCooldown);
    public bool IsReady => CooldownTime <= 0;

    /// <summary>
    /// Create a timer.<br/>
    /// Time must be manually updated using <see cref="Recharge(float)"/>.
    /// </summary>
    /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
    public BasicClockwork(float cooldown)
    {
        TotalCooldown = cooldown;
        ResetCooldown();
    }

    public void ResetCooldown() => CooldownTime = TotalCooldown;
    public bool Recharge(float deltaTime)
    {
        CooldownTime -= deltaTime;
        return IsReady;
    }

    /// <summary>
    /// Calls <see cref="Recharge(float)"/>.</summary>
    /// <param name="deltaTime">Time since last increase.</param>
    public void Update(float deltaTime) => Recharge(deltaTime);
}