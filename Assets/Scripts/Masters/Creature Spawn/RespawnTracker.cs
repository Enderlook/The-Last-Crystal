using CreaturesAddons;
using UnityEngine;
using static CreatureSpawn;

public class RespawnTracker : MonoBehaviour, IDie
{
    private CreatureSpawnToken spawner;
    public void SetSpawner(CreatureSpawnToken spawner) => this.spawner = spawner;
    public void Die(bool suicide) => spawner.SpawnWithDelay();
}