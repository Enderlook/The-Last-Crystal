using CreaturesAddons;
using FloatPool.Decorators;
using HealthBarGUI;
using Master;
using UnityEngine;

public class InitializePlayers : MonoBehaviour
{
    [SerializeField, Tooltip("Warrior health bar.")]
    public HealthBar warriorHealthBar;

    [SerializeField, Tooltip("Wizard health bar.")]
    public HealthBar wizardHealthBar;

    public void InitializeWarrior(GameObject warrior)
    {
        InitializeCreature(warrior, warriorHealthBar);
        Global.TransformCreature.Warrior.SetTransform(warrior.transform);
        DestroyNotifier.ExecuteOnDeath(warrior, () => Global.TransformCreature.Warrior.ResetTransform());
    }

    public void InitializeWizard(GameObject wizard)
    {
        InitializeCreature(wizard, wizardHealthBar);
        Global.TransformCreature.Wizard.SetTransform(wizard.transform);
        DestroyNotifier.ExecuteOnDeath(wizard, () => Global.TransformCreature.Wizard.ResetTransform());
    }

    private static void InitializeCreature(GameObject creature, HealthBar healthBar)
    {
        BarDecorator barDecorator = creature.GetComponent<Creature>().health.GetLayer<BarDecorator>();
        barDecorator.Bar = healthBar;
    }
}
