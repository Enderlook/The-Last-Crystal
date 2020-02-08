using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyWhenAnimationEnds : MonoBehaviour
{
    private void Awake() => Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
}
