using CreaturesAddons.Weapons;

using Master;

using SoundSystem;

using System;

using UnityEngine;
using UnityEngine.UI;

public class WeaponCooldownDisplay : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Background sprite of weapon.")]
    public Image image;
    [Tooltip("Reverse fill mode.")]
    public bool reverseFill;
    [Tooltip("Configuration by percent.")]
    public ModePercent[] modes;

    [Header("Setup")]
    [Tooltip("Weapon to track cooldown")]
    public Weapon weapon;
    [Tooltip("Audio Source to play sounds.")]
    public AudioSource audioSource;

    private Sound lastSound;

    private void Update()
    {
        image.fillAmount = GetCharge();
        if (modes.Length > 0)
        {
            ModePercent mode = GetMode(1 - weapon.CooldownPercent);
            image.color = mode.color;

            if (audioSource != null)
            {
                if (mode.sound != lastSound && lastSound != null && mode.sound != null && Settings.IsSoundActive)
                    mode.sound.Play(audioSource);
            }
            else
                Debug.LogWarning($"{nameof(WeaponCooldownDisplay)} in {gameObject.name} gameobject doesn't have an {nameof(audioSource)}.");

            lastSound = mode.sound;
        }
    }

    private float GetCharge() => reverseFill ? 1 - weapon.CooldownPercent : weapon.CooldownPercent;

    private ModePercent GetMode(float percent)
    {
        for (int i = modes.Length - 1; i >= 0; i--)
        {
            if (modes[i].percent <= percent)
                return modes[i];
        }
        throw new Exception("No matching mode found. This shouldn't be happening.");
    }

#if UNITY_EDITOR
    private readonly string INITIAL_MESSAGE = $"{nameof(WeaponCooldownDisplay)} in gameobject";

    private void OnValidate()
    {
        if (modes.Length > 0)
        {
            if (modes[0].percent != 0)
            {
                Debug.LogWarning($"{INITIAL_MESSAGE} {gameObject.name} has a the element on index 1 from {nameof(modes)} with a {nameof(ModePercent.percent)} non equal to 0 ({modes[0].percent}). It was fixed.");
                modes[0].percent = 0;
            }
            for (int i = 0; i < modes.Length - 1; i++)
            {
                int next = i + 1;
                if (modes[i].percent >= modes[next].percent)
                {
                    Debug.LogWarning($"{INITIAL_MESSAGE} {gameObject.name} has a the element on index {next} from {nameof(modes)} array with a {nameof(ModePercent.percent)} ({modes[next].percent}) lower or equal than its predecessor ({modes[i].percent}).\nIt should be greater.");
                    modes[next].percent = modes[i].percent;
                }
            }
        }
        if (audioSource == null)
            Debug.LogWarning($"{INITIAL_MESSAGE} {gameObject.name} doesn't have an {nameof(audioSource)}.");
    }
#endif

    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It's only public for Unity Editor, so there is no problem in having it nested.")]
    public class ModePercent
    {
        [Tooltip("Color to show.")]
#pragma warning disable CA2235
        public Color color;
#pragma warning restore CA2235

        [Tooltip("Sound to play")]
        public Sound sound;

        [Tooltip("Minimal fill amount to show it"), Range(0, 1)]
        public float percent;
    }
}