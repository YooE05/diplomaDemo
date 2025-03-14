﻿using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    [SerializeField] private GameObject _cube;

    private void Awake()
    {
        DisableCube();
    }

    public void EnableCube()
    {
        _cube.SetActive(true);
    }

    public void DisableCube()
    {
        _cube.SetActive(false);
    }
}