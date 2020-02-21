using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyTextAnimation : MonoBehaviour
{
    public void InitializeEvent()
    {
        Spawner.Instance.InitializeWave();
    }
}
