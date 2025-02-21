﻿using UnityEngine;

namespace YooE.Diploma
{
    public sealed class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public AnimationEvents AnimationEvents { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Transform Visual { get; private set; }
        [field: SerializeField] public WeaponView[] WeaponViews { get; private set; }
        [field: SerializeField] public HitPointsComponent HitPointsComponent { get; private set; }

        public void DisablePlayerCharacterController()
        {
            CharacterController.enabled = false;
        }

        public void DisablePlayerView()
        {
            Visual.gameObject.SetActive(false);
        }
    }
}