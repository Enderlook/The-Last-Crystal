using CreaturesAddons;
using UnityEngine;

public class KeyboardAttack : MonoBehaviour, IAttack, IAwake
{
    [Tooltip("Basic attack key.")]
    public KeyCode keyAttack;
    [Tooltip("Cooldown of basic attack.")]
    public float timeBtwAttack;

    private Animator thisAnimator;
    private float nextAttack;

    // Constants
    private const string BASIC_ATTACK = "Attack";

    void IAwake.Awake(Creature creature)
    {
        thisAnimator = creature.animator;
        Debug.Log(thisAnimator.name);
    }

    void IAttack.Attack(float time)
    {
        if (Input.GetKey(keyAttack) && time >= nextAttack)
        {
            thisAnimator.SetTrigger(BASIC_ATTACK);
            nextAttack = time + timeBtwAttack;
        }
    }
}
