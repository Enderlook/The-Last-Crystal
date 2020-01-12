﻿using Additions.Utils;
using CreaturesAddons.Weapons;

using UnityEngine;

namespace PlayerAddons
{
    public class WeaponShooter : MonoBehaviour, IUpdate
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Weapons configuration")]
        private WeaponKeyPair[] weapons;
#pragma warning restore CS0649

        void IUpdate.UpdateBehaviour(float deltaTime)
        {
            foreach (WeaponKeyPair weapon in weapons)
            {
                if (weapon.ShouldShoot)
                    weapon.weapon.TryExecute(deltaTime);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                WeaponKeyPair weapon = weapons[i];
                if (weapon.weapon == null)
                    Debug.LogWarning($"Gameobject {gameObject.name} has the {nameof(weapons)} element at index {i} without a {nameof(WeaponKeyPair.weapon)} configured on it.\nIt will raise error if it's tried to shoot in-game.");
                if (weapon.key == KeyCode.None && weapon.button == WeaponKeyPair.MouseButton.None)
                    Debug.LogWarning($"Gameobject {gameObject.name} has the {nameof(weapons)} element at index {i} without an assigned {nameof(WeaponKeyPair.key)} nor {nameof(WeaponKeyPair.button)}. It can't be shooted in-game.");
            }
        }
#endif
    }

    [System.Serializable]
    public class WeaponKeyPair
    {
        public enum MouseButton { None = -1, Left = 0, Right = 1, Middle = 2 }
#pragma warning disable CA2235
        [Tooltip("Weapon.")]
        public Weapon weapon;
#pragma warning restore CA2235

        [Tooltip("Key to shoot.")]
        public KeyCode key;

        [Tooltip("Mouse button to shoot.")]
        public MouseButton button;

        [Tooltip("Can be hold down.")]
        public bool canBeHoldDown;

        public bool ShouldShoot => canBeHoldDown
            ? Input.GetKey(key) || (button != MouseButton.None && Input.GetMouseButton((int)button))
            : Input.GetKeyDown(key) || (button != MouseButton.None && Input.GetMouseButtonDown((int)button));
    }
}