﻿using System.Collections.Generic;
using UnityEngine;

namespace YooE.Diploma
{
    public sealed class PlayerShooting : Listeners.IUpdateListener, Listeners.IStartListener
    {
        private readonly TargetPicker _targetPicker;
        private readonly WeaponView[] _weaponsView;
        private readonly BulletsSystem _bulletsSystem;
        private readonly ShootingConfig _shootingConfig;
        private readonly UpdateTimer _timer;

        private float _nextShotTime;
        private bool _canShoot;

        public PlayerShooting(BulletsSystem bulletsSystem, WeaponView[] weaponsView, TargetPicker targetPicker,
            ShootingConfig shootingConfig, UpdateTimer timer)
        {
            _shootingConfig = shootingConfig;
            _targetPicker = targetPicker;
            _weaponsView = weaponsView;
            _timer = timer;
            _bulletsSystem = bulletsSystem;
            _bulletsSystem.OnInit();

            _canShoot = false;
        }

        //TODO: change hiding weapons to rotate them forward when stop shooting
        //TODO: remove change visibility from update
        public void OnStart()
        {
            _canShoot = true;
            _nextShotTime = 0;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_canShoot)
            {
                _nextShotTime += Time.deltaTime;
                return;
            }

            if (_targetPicker.TryGetNClosestTargets(_weaponsView.Length, out var targets))
            {
                SetTargetsOnWeapons(targets);

                if (_timer.CurrentTime < _nextShotTime) return;
                _nextShotTime += _shootingConfig.ShootingDelay;

                for (var i = 0; i < _weaponsView.Length; i++)
                {
                    Shoot(_weaponsView[i]);
                }
            }
            else
            {
                _nextShotTime += Time.deltaTime;
                HideWeapons();
            }
        }

        public void DisableShooting()
        {
            _canShoot = false;
            HideWeapons();
        }

        private void SetTargetsOnWeapons(List<Collider> targets)
        {
            for (var i = 0; i < _weaponsView.Length; i++)
            {
                var col = targets.Find(c => c == _weaponsView[i].CurrentTarget);
                if (col)
                {
                    targets.Remove(col);
                }
                else
                {
                    _weaponsView[i].CurrentTarget = null;
                }
            }

            for (var i = 0; i < _weaponsView.Length; i++)
            {
                if (_weaponsView[i].CurrentTarget is null)
                {
                    _weaponsView[i].CurrentTarget = targets[0];
                    targets.Remove(targets[0]);
                }

                _weaponsView[i].RotateWeapon();
            }
        }

        private void HideWeapons()
        {
            for (var i = 0; i < _weaponsView.Length; i++)
            {
                _weaponsView[i].SetWeaponVisibility(false);
            }
        }

        private void Shoot(WeaponView weaponView)
        {
            weaponView.SetWeaponVisibility(true);

            var velocity = CalculateBulletVelocity(
                GetShotDirection(weaponView.ShootingPointPosition, weaponView.CurrentTarget.transform.position),
                _shootingConfig.BulletSpeed);
            _bulletsSystem.FlyBullet(weaponView.ShootingPointPosition, _shootingConfig.Damage, velocity);
        }

        private Vector3 GetShotDirection(Vector3 startShootingPosition, Vector3 targetPosition)
        {
            var direction = targetPosition - startShootingPosition;
            return direction.normalized;
        }

        private Vector3 CalculateBulletVelocity(Vector3 direction, float speed)
        {
            var velocity = direction * speed;
            return velocity;
        }
    }
}