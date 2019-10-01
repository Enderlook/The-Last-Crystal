using System;
using CreaturesAddons;
using FloatPool;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Health.")]
    public Pool health;

    [Tooltip("Movement speed.")]
    public float speed = 1;

    [Tooltip("Damage amount.")]
    [Range(0f, 25f)]
    public float damage;

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

    private const string ANIMATION_STATE_HURT = "Hurt";

    public float SpeedMultiplier {
        get => stoppableRigidbody.SpeedMultiplier;
        set => stoppableRigidbody.SpeedMultiplier = value;
    }

    public Transform Transform => thisRigidbody2D.transform;

    private void Awake()
    {
        health.Initialize();
        LoadComponents();
    }

    private void LoadComponents()
    {
        dies = gameObject.GetComponentsInChildren<IDie>();
        updates = gameObject.GetComponentsInChildren<IUpdate>();
        move = gameObject.GetComponentInChildren<IMove>();
        attack = gameObject.GetComponentInChildren<IAttack>();
        Array.ForEach(gameObject.GetComponents<IInit>(), e => e.Init(this));
    }

    protected virtual void Update()
    {
        health.InternalUpdate(Time.deltaTime);
        move?.Move(Time.deltaTime, SpeedMultiplier * speed);
        attack?.Attack(Time.time);
        Array.ForEach(updates, e => e.UpdateBehaviour(Time.deltaTime));
    }

    /// <summary>
    /// Takes healing increasing its <see cref="Health"/>.
    /// </summary>
    /// <param name="amount">Amount of <see cref="Health"/> recovered. Must be positive.</param>
    public void TakeHealing(float amount) => health.Increase(amount);

    /// <summary>
    /// Take damage reducing its <see cref="Health"/>.<br>
    /// Animation and floating text will only be show if their parameters are <see langword="true"/> and the effective taken damage is greater than 0.
    /// </summary>
    /// <param name="amount">Amount of <see cref="Health"/> lost. Must be positive.</param>
    /// <param name="displayText">Whenever the damage taken must be shown in a floating text.</param>
    /// <param name="displayAnimation">Whenever it should display <see cref="ANIMATION_STATE_HURT"/> animation.</param>
    public virtual void TakeDamage(float amount, bool displayText = false, bool displayAnimation = true)
    {
        (float remaining, float taken) = health.Decrease(amount);
        if (taken > 0)
        {
            if (displayAnimation)
                animator.SetTrigger(ANIMATION_STATE_HURT);
            if (displayText)
                SpawnFloatingText(amount, Color.Lerp(Color.red, new Color(1, .5f, 0), health.Ratio));
        }
    }

    /// <summary>
    /// Push creature.
    /// </summary>
    /// <param name="direction">Direction to apply force.</param>
    /// <param name="force">Amount of force to apply</param>
    public void Push(Vector2 direction, float force = 1, PushMode pushMode = PushMode.Local)
    {
        if (pushMode == PushMode.Local)
            direction = ((Vector2)Transform.position - direction).normalized;
        thisRigidbody2D.AddForce(direction * force);
    }
    public enum PushMode { Local, Global };

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

    private void OnCollisionEnter2D(Collision2D collision) => CheckInDamageCollision(collision.gameObject);

    private void OnTriggerEnter2D(Collider2D collision) => CheckInDamageCollision(collision.gameObject);

    private void CheckInDamageCollision(GameObject target)
    {
        IDamageOnTouch damageOnTouch = target.gameObject.GetComponent<IDamageOnTouch>();
        if (damageOnTouch != null)
            damageOnTouch.ProduceDamage(this);
    }
}