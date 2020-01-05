using AdditionalComponents;

using CreaturesAddons;

using FloatPool.Decorators;

using HealthBarGUI;

using Master;

using UnityEngine;

public class InitializePlayers : MonoBehaviour
{
    [SerializeField, Tooltip("Warrior health bar.")]
    public HealthBar warriorHealthBar;

    [SerializeField, Tooltip("Warrior energy bar.")]
    public HealthBar warriorEnergyBar;

    [SerializeField, Tooltip("Wizard health bar.")]
    public HealthBar wizardHealthBar;

    [SerializeField, Tooltip("Wizard energy bar.")]
    public HealthBar wizardEnergyBar;

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
        BarDecorator barDecorator = creature.GetComponent<Creature>().health.GetLayer<BarDecorator>();
        barDecorator.Bar = healthBar;
        creature.GetComponent<EnergyManager>().SetEnergyBar(energyBar);
    }
}
