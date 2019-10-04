using System;
using UnityEngine;

public class DestroyNotifier : MonoBehaviour
{
    private Action callback;
    public void SetCallback(Action callback) => this.callback = callback;
    private void OnDestroy() => callback();
}