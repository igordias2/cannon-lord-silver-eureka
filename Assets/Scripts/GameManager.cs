using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public GameObject[] bullets;

    void Awake()
    {
        if(GM == null)
            GM = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        GetBullets();
    }
    private void GetBullets()
    {
        bullets = Resources.LoadAll<GameObject>("Bullets");
    }

    void Update()
    {
        
    }
}
