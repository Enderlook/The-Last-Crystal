using System;
using CreaturesAddons;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Health.")]
    public HealthPoints healthPoints;

    [Header("Setup")]
    [Tooltip("FloatingTextController Script")]
    public FloatingTextController floatingTextController;

    [Tooltip("StoppableRigidbody Script")]
    public StoppableRigidbody stoppableRigidbody;

    private IInitialize[] initializes;
    private IDie[] dies;
    private IUpdate[] updates;
    private IMove move;

    private bool hasBeenBuilded = false;

    protected bool isDead = false;

    public float SpeedMultiplier {
        get => stoppableRigidbody.SpeedMultiplier;
        set => stoppableRigidbody.SpeedMultiplier = value;
    }

    private void Build()
    /* We could have used Awake,
     * but in order to use that we would need to make Initialize public and call it from EnemySpawner through GetComponent.
     * That is because OnEnable is called before Awake.
     */
    {
        Array.ForEach(gameObject.GetComponents<IBuild>(), e => e.Build(this));
        LoadComponents();
        healthPoints.SetDie(Die);
    }

    private void LoadComponents()
    {
        initializes = gameObject.GetComponentsInChildren<IInitialize>();
        dies = gameObject.GetComponentsInChildren<IDie>();
        updates = gameObject.GetComponentsInChildren<IUpdate>();
        move = gameObject.GetComponentInChildren<IMove>();
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            gameObject.SetActive(false);
            return;
        }
        healthPoints.Update(Time.deltaTime);
        move?.Move(Time.deltaTime, SpeedMultiplier);
        Array.ForEach(updates, e => e.Update(Time.deltaTime));
    }

    /// <summary>
    /// Initializes some values that are reused during the enemies recycling.
    /// </summary>
    protected virtual void Initialize()
    {
        healthPoints.Initialize();
        Array.ForEach(initializes, e => e.Initialize());
        isDead = false;
    }

    private void OnEnable()
    {
        if (!hasBeenBuilded)
        {
            hasBeenBuilded = true;
            Build();
        }
        Initialize();
    }

    /// <summary>
    /// Takes healing increasing its <see cref="Health"/>.
    /// </summary>
    /// <param name="amount">Amount of <see cref="Health"/> recovered. Must be positive.</param>
    public void TakeHealing(float amount)
    {
        if (isDead) return;
        healthPoints.TakeHealing(amount);
    }

    /// <summary>
    /// Take damage reducing its <see cref="Health"/>.
    /// </summary>
    /// <param name="amount">Amount of <see cref="Health"/> lost. Must be positive.</param>
    /// <param name="displayText">Whenever the damage taken must be shown in a floating text.</param>
    public virtual void TakeDamage(float amount, bool displayDamage = false)
    {
        if (isDead) return;
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
        if (isDead) return;
        isDead = true;
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