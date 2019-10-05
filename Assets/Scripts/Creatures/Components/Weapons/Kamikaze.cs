namespace CreaturesAddons.Weapons
{
    public class Kamikaze : PassiveMelee
    {
        public override void ProduceDamage(object victim)
        {
            base.ProduceDamage(victim);
            Destroy(thisTransform.gameObject);
        }
    }
}