using System;
using AdditionalComponents;
using Navigation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

public class CreatureSpawn : MonoBehaviour
{
#pragma warning disable CS0649
    [Header("Configuration")]
    [SerializeField, Tooltip("Creatures to spawn.")]
    private CreatureSpawnToken[] creatures;
    [Header("Setup")]
    [SerializeField, Tooltip("Navigation graph used by enemies to move.")]
    private NavigationGraph navigationGraph;
#pragma warning restore CS0649

    private void Start() => Array.ForEach(creatures, e => e.Initialize(navigationGraph));
    private void Update() => Array.ForEach(creatures, e => e.Update(Time.deltaTime));

    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Used by Unity Editor, it doesn't matter if it isn't visible.")]
    public class CreatureSpawnToken
    {
        [Header("Configuration")]
#pragma warning disable CS0649, CA2235
        [SerializeField, Tooltip("Prefab to spawn.")]
        private GameObject prefab;
        [SerializeField, Tooltip("Spawning point.")]
        private Transform spawningPoint;
#pragma warning restore CA2235
        [SerializeField, Tooltip("Respawning time in seconds.")]
        private float respawningTime;
        [SerializeField, Tooltip("Should be instantly spawn on Start.")]
        private bool spawnOnStart;
        [SerializeField, Tooltip("Action executed when spawn prefab. The parameter is the spawned game object.")]
        private UnityEventGameObject action;
        [Header("Setup")]
        [SerializeField, Tooltip("Image used to show respawning time percent.")]
        private Image image;
#pragma warning restore CS0649

        [NonSerialized]
        private Clockwork timer;
        [NonSerialized]
        private NavigationGraph navigationGraph;

        private bool enableFill;

        public void Initialize(NavigationGraph navigationGraph)
        {
            this.navigationGraph = navigationGraph;
            timer = new Clockwork(respawningTime, SpawnInstantly, true, 0);
            if (spawnOnStart)
                SpawnInstantly();
        }

        public void SpawnInstantly()
        {
            GameObject gameObject = Instantiate(prefab, spawningPoint.position, spawningPoint.rotation);
            gameObject.AddComponent<DestroyNotifier>().AddCallback(SpawnWithDelay);
            NavigationAgent.InjectNavigationGraph(gameObject, navigationGraph);
            action.Invoke(gameObject);
            enableFill = false;
        }

        public void SpawnWithDelay()
        {
            timer.ResetCycles(1);
            enableFill = true;
        }

        public void Update(float deltaTime)
        {
            timer.UpdateBehaviour(deltaTime);
            image.fillAmount = enableFill ? 1 - timer.CooldownPercent : 1;
        }

        [Serializable]
        public class UnityEventGameObject : UnityEvent<GameObject> { }
    }
}