using CreaturesAddons;
using UnityEngine;

public class KeyboardAttack : MonoBehaviour, IAttack
{
    [Tooltip("Key attack.")]
    public KeyCode keyAttack;
    [Tooltip("Animator.")]
    public Animator animator;

    // Constants
    private const string BASIC_ATTACK = "Attack";

    void IAttack.Attack(float deltaTime)
    {
        if (Input.GetKey(keyAttack)) animator.SetTrigger(BASIC_ATTACK);
    }
}
