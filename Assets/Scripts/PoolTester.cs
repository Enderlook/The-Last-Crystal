using FloatPool;
using UnityEngine;

public class PoolTester : MonoBehaviour
{
    [SerializeField, Tooltip("FloatPool to affect.")]
    private Pool pool;

    [SerializeField, Tooltip("Key to decrease.")]
    private KeyCode decreaseKey;

    [SerializeField, Tooltip("Amount to decrease.")]
    private float decreaseAmount;

    [SerializeField, Tooltip("Key to increase.")]
    private KeyCode increaseKey;

    [SerializeField, Tooltip("Amount to increase.")]
    private float increaseAmount;

    [SerializeField, Tooltip("Initialize pool.")]
    private bool initialize;
    [SerializeField, Tooltip("Include pool to update in each frame.")]
    private bool update;

    private void Start()
    {
        if (initialize)
            pool.Initialize();
    }

    private void Update()
    {
        if (update)
            pool.UpdateBehaviour(Time.deltaTime);
        if (Input.GetKeyDown(decreaseKey))
            pool.Decrease(decreaseAmount);
        if (Input.GetKeyDown(increaseKey))
            pool.Increase(increaseAmount);
    }
}
