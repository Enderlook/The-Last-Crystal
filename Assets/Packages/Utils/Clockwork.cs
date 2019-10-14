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
    /// Reset <see cref="CooldownTime"/> time to <see cref="TotalCooldown"/>.
    /// </summary>
    void ResetCooldown();

    /// <summary>
    /// Assign a new maximum value <paramref name="newCooldownTime"/> and calls <see cref="ResetCooldown"/>.
    /// </summary>
    void ResetCooldown(float newCooldownTime);

    /// <summary>
    /// Reduce <see cref="CooldownTime"/> time and checks if the <see cref="CooldownTime"/> is over.
    /// </summary>
    /// <param name="deltaTime"><see cref="Time.deltaTime"/></param>
    /// <returns><see cref="IsReady"/>.</returns>
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

    /// <summary>
    /// Total number of times <see cref="Execute"/> can be called. -1 is unlimited.
    /// </summary>
    int TotalCycles { get; }

    /// <summary>
    /// Remaining number of times <see cref="Execute"/> can be called.
    /// </summary>
    int RemainingCycles { get; }

    /// <summary>
    /// Whenever there is no number of time <see cref="Execute"/> can be called.
    /// </summary>
    bool IsEndlessLoop { get; }

    /// <summary>
    /// Whenever the timer is working or not. If <see cref="RemainingCycles"/> is 0 the timer stop working.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Reset <see cref="RemainingCycles"/> to <see cref="TotalCycles"/>.
    /// </summary>
    void ResetCycles();

    /// <summary>
    /// Assign a new maximum value <paramref name="TotalCycles"/> and <see cref="RemainingCycles"/>.
    /// </summary>
    void ResetCycles(int newCycles);
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

    public int TotalCycles { get; private set; }
    public int RemainingCycles { get; private set; }
    public bool IsEndlessLoop => TotalCycles == -1;
    public bool IsEnabled => RemainingCycles > 0 || IsEndlessLoop;

    /// <summary>
    /// Create a timer that executes <paramref name="Callback"/> each <paramref name="cooldown"/> seconds.<br/>
    /// Time must be manually updated using <see cref="Recharge(float)"/>, <see cref="TryExecute(float)"/> or <see cref="TryExecute(ref T, float)"/> methods.
    /// </summary>
    /// <param name="cooldown">Time in seconds to execute <paramref name="Callback"/>.</param>
    /// <param name="Callback">Action to execute.</param>
    /// <param name="autoExecute">Whenever <see cref="UpdateBehaviour(float)"/> must call <see cref="Execute"/> when <see cref="CooldownTime"/> is 0.</param>
    /// <param name="cycle">Number of times <see cref="Execute"/> can be call. Use -1 for unlimited. Use <see cref="ResetCycles"/> to recover their uses. Don't use 0 or the timer will be disabled by default.</param>
    public Clockwork(float cooldown, System.Action Callback, bool autoExecute = true, int cycles = -1)
    {
        ResetCycles(cycles);
        ResetCooldown(cooldown);
        this.Callback = Callback;
        this.autoExecute = autoExecute;
    }

    public void Execute()
    {
        if (ReduceCyclesByOne())
        {
            ResetCooldown();
            Callback();
        }
    }

    private bool ReduceCyclesByOne()
    {
        if (IsEndlessLoop)
            return true;
        bool enabled = IsEnabled;
        RemainingCycles--;
        return enabled;
    }

    public bool TryExecute(float deltaTime = 0)
    {
        if (IsEnabled && Recharge(deltaTime))
        {
            Execute();
            return true;
        }
        return false;
    }

    public void ResetCooldown() => CooldownTime = TotalCooldown;
    public void ResetCooldown(float newCooldownTime)
    {
        TotalCooldown = newCooldownTime;
        ResetCooldown();
    }

    public void ResetCycles() => RemainingCycles = TotalCycles;
    public void ResetCycles(int newCycles)
    {
        TotalCycles = newCycles;
        ResetCycles();
    }

    public bool Recharge(float deltaTime)
    {
        if (IsEnabled)
        {
            CooldownTime -= deltaTime;
            return IsReady;
        }
        return false;
    }

    /// <summary>
    /// Calls <see cref="Recharge(float)"/> if <see cref="IsEnabled"/>, and calls <see cref="Execute"/> if <see cref="autoExecute"/> is <see langword="true"/> and <see cref="Recharge(float)"/> returned <see langword="true"/>.
    /// </summary>
    /// <param name="deltaTime">Time since last increase.</param>
    public void UpdateBehaviour(float deltaTime)
    {
        if (IsEnabled && Recharge(deltaTime) && autoExecute)
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
    public void ResetCooldown(float newCooldownTime)
    {
        TotalCooldown = newCooldownTime;
        ResetCooldown();
    }

    public bool Recharge(float deltaTime)
    {
        CooldownTime -= deltaTime;
        return IsReady;
    }

    /// <summary>
    /// Calls <see cref="Recharge(float)"/>.</summary>
    /// <param name="deltaTime">Time since last increase.</param>
    public void UpdateBehaviour(float deltaTime) => Recharge(deltaTime);
}