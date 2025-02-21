﻿using System;
using UnityEngine;

namespace YooE.Diploma
{
    public sealed class DeathObserver
    {
        public event Action OnDeathStart;
        public event Action OnDeathEnd;

        private readonly HitPointsComponent _hitPointsComponent;
        private readonly EnemyView _enemyView;
        private readonly EnemyAnimationEvents _animationEvents;

        public DeathObserver(HitPointsComponent hitPointsComponent, EnemyView enemyView, EnemyAnimationEvents enemyAnimationEvents)
        {
            _hitPointsComponent = hitPointsComponent;
            _enemyView = enemyView;
            _animationEvents = enemyAnimationEvents;
            
            _hitPointsComponent.OnHpEmpty += StartDeathProcess;
            _animationEvents.OnDeathAnimationEnd += EnemyDeathActions;
        }

        private void StartDeathProcess(GameObject obj)
        {
            _hitPointsComponent.OnHpEmpty -= StartDeathProcess;
            _enemyView.SetAnimatorTrigger("IsDead");
            OnDeathStart?.Invoke();
        }

        private void EnemyDeathActions()
        {
            _animationEvents.OnDeathAnimationEnd -= EnemyDeathActions;
            _enemyView.DisableEnemy();
            //Death particles
            //Add points to player
            OnDeathEnd?.Invoke();
        }
    }
    
    
}