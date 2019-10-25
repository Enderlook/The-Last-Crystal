using System.Collections.Generic;
using AdditionalExtensions;
using Master;
using Navigation;
using Serializables;
using UnityEngine;

namespace CreaturesAddons.Movement.NodeMovement
{
    [RequireComponent(typeof(NavigationAgent))]
    public class TargetAndPathGetter : MonoBehaviour, IInit
    {
#pragma warning disable CS0649
        [Header("Configuration")]
        [SerializeField, Tooltip("Cost calculator to follow crystal based on distance.")]
        private Calculator crystalCalculator;
        [SerializeField, Tooltip("Cost calculator to follow players based on distance.")]
        private Calculator playerCalculator;

        private NavigationAgent navigationAgent;
#pragma warning restore CS0649

        void IInit.Init(Creature creature) => navigationAgent = GetComponent<NavigationAgent>();

        private readonly List<(Transform target, float distancePriority, List<Connection> path)> targets = new List<(Transform target, float distancePriority, List<Connection> path)>();

        public List<Connection> GetPath(out Transform target)
        {
            targets.Clear();
            AddTargetIfNotNull(Global.TransformCreature.Crystal.GetTranform(), crystalCalculator);
            AddTargetIfNotNull(Global.TransformCreature.Warrior.GetTranform(), playerCalculator);
            AddTargetIfNotNull(Global.TransformCreature.Wizard.GetTranform(), playerCalculator);
            
            // Technically, this should never happen, but to be sure...
            if (targets.Count == 0)
            {
                target = null;
                return null;
            }
            
            (Transform t, float distancePriority, List<Connection> path) = targets.MinBy(e => e.distancePriority);
            target = t;
            return path;
        }

        private void AddTargetIfNotNull(Transform transform, Calculator calculator)
        {
            if (transform != null)
                targets.Add((transform, CalculateDistancePriorityTo(transform, out List<Connection> connections, calculator), connections));
        }

        private float CalculateDistancePriorityTo(Transform transform, out List<Connection> connections, Calculator calculator)
        {
            if (transform)
            {
                connections = navigationAgent.FindPathTo(transform, out float distance);
                return calculator == null ? distance : calculator.Calculate(distance);
            }
            connections = default;
            return -1;
        }
    }
}