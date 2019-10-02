using CreaturesAddons;
using FloatPool.Decorators;
using HealthBarGUI;
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
        Global.warrior = warrior.transform;
    }

    public void InitializeWizard(GameObject wizard)
    {
        InitializeCreature(wizard, wizardHealthBar);
        Global.wizard = wizard.transform;
    }

    private static void InitializeCreature(GameObject creature, HealthBar healthBar)
    {
        BarDecorator barDecorator = creature.GetComponent<Creature>().health.GetLayer<BarDecorator>();
        barDecorator.Bar = healthBar;
    }
}
