using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        LevelManager.Default.Init();
    }
}
