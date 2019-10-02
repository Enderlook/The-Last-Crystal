using CreaturesAddons;
using FloatPool.Decorators;
using UnityEngine;

public class InitializePlayers : MonoBehaviour
{
    [SerializeField, Tooltip("Warrior health bar.")]
    public HealthBar warriorHealthBar;

    [SerializeField, Tooltip("Wizard health bar.")]
    public HealthBar wizardHealthBar;

    public void InitializeWarrior(GameObject warrior) => InitializeCreature(warrior, warriorHealthBar);
    public void InitializeWizard(GameObject wizard) => InitializeCreature(wizard, wizardHealthBar);
    private void InitializeCreature(GameObject creature, HealthBar healthBar)
    {
        BarDecorator barDecorator = creature.GetComponent<Creature>().health.GetLayer<BarDecorator>();
        barDecorator.bar = healthBar;
        barDecorator.UpdateValues();
    }
}
