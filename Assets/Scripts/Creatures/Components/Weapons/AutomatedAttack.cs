﻿using System;
using CreaturesAddons;
using UnityEngine;

namespace EnemyAddons
{
    public class AutomatedAttack : MonoBehaviour, IInit, IUpdate
    {
        private IAutomatedAttack[] weapons;
        public void Init(Creature creature) => weapons = gameObject.GetComponents<IAutomatedAttack>();
        public void UpdateBehaviour(float deltaTime) => Array.ForEach(weapons, e => e.AttackIfIsReadyAndIfTargetInRange());
    }
}

