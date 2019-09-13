using System;
using CreaturesAddons;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Health.")]
    public HealthPoints healthPoints;

    [Tooltip("Movement speed.")]
    public float speed;

    [Header("Setup")]
    [Tooltip("FloatingTextController Script")]
    public FloatingTextController floatingTextController;

    [Tooltip("StoppableRigidbody Script")]
    public StoppableRigidbody stoppableRigidbody;

    [Tooltip("Rigidbody Component.")]
    public Rigidbody2D thisRigidbody2D;

    [Tooltip("Animator Component.")]
    public Animator animator;

    private IDie[] dies;
    private IUpdate[] updates;
    private IMove move;
    private IAttack attack;

    private const string HURT = "Hurt";
    
    public float SpeedMultiplier {
        get => stoppableRigidbody.SpeedMultiplier;
        set => stoppableRigidbody.SpeedMultiplier = value;
    }

    private void Awake()
    {
        healthPoints.SetDie(Die);
        healthPoints.Initialize();
        LoadComponents();
    }

    private void LoadComponents()
    {
        dies = gameObject.GetComponentsInChildren<IDie>();
        updates = gameObject.GetComponentsInChildren<IUpdate>();
        move = gameObject.GetComponentInChildren<IMove>();
        attack = gameObject.GetComponentInChildren<IAttack>();
        Array.ForEach(gameObject.GetComponents<IAwake>(), e => e.Awake(this));
    }

    protected virtual void Update()
    {
        healthPoints.Update(Time.deltaTime);
        move?.Move(Time.deltaTime, SpeedMultiplier * speed);
        attack?.Attack(Time.time);
        Array.ForEach(updates, e => e.Update(Time.deltaTime));
    }

    /// <summary>
    /// Takes healing increasing its <see cref="Health"/>.
    /// </summary>
    /// <param name="amount">Amount of <see cref="Health"/> recovered. Must be positive.</param>
    public void TakeHealing(float amount) => healthPoints.TakeHealing(amount);

    /// <summary>
    /// Take damage reducing its <see cref="Health"/>.
    /// </summary>
    /// <param name="amount">Amount of <see cref="Health"/> lost. Must be positive.</param>
    /// <param name="displayText">Whenever the damage taken must be shown in a floating text.</param>
    public virtual void TakeDamage(float amount, bool displayDamage = false)
    {
        animator.SetTrigger(HURT);
        healthPoints.TakeDamage(amount);
        if (displayDamage)
            SpawnFloatingText(amount, Color.Lerp(Color.red, new Color(1, .5f, 0), healthPoints.Ratio));
    }

    /// <summary>
    /// Disables <see cref="gameObject"/> and spawn an explosion prefab instance on current location.
    /// </summary>
    /// <param name="suicide"><see langword="true"/> if it was a suicide. <see langword="false"/> if it was murderer.</param>
    public virtual void Die(bool suicide = false)
    {
        Array.ForEach(dies, e => e.Die(suicide));
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Spawn a floating text above the creature.
    /// </summary>
    /// <param name="text">Text to display.</param>
    /// <param name="textColor">Color of the text.</param>
    /// <param name="checkIfPositive">Only display if the number is positive.</param>
    /// <seealso cref="SpawnFloatingText(float, Color?, bool)"/>
    protected void SpawnFloatingText(string text, Color? textColor, bool checkIfPositive = true)
    {
        if (floatingTextController != null && !checkIfPositive)
            floatingTextController.SpawnFloatingText(text, textColor);
    }
    /// <summary>
    /// Spawn a floating text above the creature.
    /// </summary>
    /// <param name="text">Text to display.</param>
    /// <param name="textColor">Color of the text.</param>
    /// <param name="checkIfPositive">Only display if the number is positive.</param>
    /// <seealso cref="SpawnFloatingText(string, Color?, bool)"/>
    protected void SpawnFloatingText(float text, Color? textColor, bool checkIfPositive = true) => SpawnFloatingText(text.ToString(), textColor, checkIfPositive);
}