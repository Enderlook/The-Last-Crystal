using Additions.Components;
using Additions.Components.FloatPool.Decorators;
using Additions.Prefabs.HealthBarGUI;

using Creatures;

using Master;

using UnityEngine;

public class InitializePlayers : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField, Tooltip("Warrior health bar.")]
    private HealthBar warriorHealthBar;

    [SerializeField, Tooltip("Warrior energy bar.")]
    private HealthBar warriorEnergyBar;

    [SerializeField, Tooltip("Wizard health bar.")]
    private HealthBar wizardHealthBar;

    [SerializeField, Tooltip("Wizard energy bar.")]
    private HealthBar wizardEnergyBar;
#pragma warning restore CS0649

    public void InitializeWarrior(GameObject warrior)
    {
        InitializeCreature(warrior, warriorHealthBar, warriorEnergyBar);
        Global.TransformCreature.Warrior.SetTransform(warrior.transform);
        DestroyNotifier.ExecuteOnDeath(warrior, () => Global.TransformCreature.Warrior.ResetTransform());
    }

    public void InitializeWizard(GameObject wizard)
    {
        InitializeCreature(wizard, wizardHealthBar, wizardEnergyBar);
        Global.TransformCreature.Wizard.SetTransform(wizard.transform);
        DestroyNotifier.ExecuteOnDeath(wizard, () => Global.TransformCreature.Wizard.ResetTransform());
    }

    private static void InitializeCreature(GameObject creature, HealthBar healthBar, HealthBar energyBar)
    {
        CreatureWithEnergy creatureWithEnergy = creature.GetComponent<CreatureWithEnergy>();
        creatureWithEnergy.SetHealthBar(healthBar);
        creatureWithEnergy.SetEnergyBar(energyBar);
    }
}
