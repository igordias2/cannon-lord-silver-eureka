using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speedToRotate;
    float rotationAngle = 1;
    GameObject player;
    float cooldownRotation;
    Transform bulletSpawnPos;
    short selectedBullet = 0;
    
    void Start()
    {
        player = this.gameObject;
        bulletSpawnPos = gameObject.transform.GetChild(0);
    }

    void Update()
    {
        RotatePlayer();
        CheckShoot();
        ChangeWeapon();
    }
    void SelectWeapon(int n){
        this.selectedBullet += (short)n;
        if(selectedBullet > GameManager.GM.bullets.Length){
            this.selectedBullet = 0;
        }
        if(selectedBullet < 0){
            this.selectedBullet = (short)GameManager.GM.bullets.Length;
        }
    }
    private void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            SelectWeapon(1);
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            SelectWeapon(-1);
        }
    }

    private void CheckShoot()
    {
        if(Input.GetKeyDown(KeyCode.Space)){

            GameObject bullet = Instantiate(GameManager.GM.bullets[selectedBullet],bulletSpawnPos.position, this.transform.rotation);
        }
    }

    private void RotatePlayer()
    {
        if (player.transform.rotation.z > 0.60f || player.transform.rotation.z < -0.60f)
        {
            if (cooldownRotation <= 0f)
                ChangeAngle();
        }
        cooldownRotation -= Time.deltaTime;
        player.transform.Rotate(new Vector3(0, 0, rotationAngle) * Time.deltaTime * speedToRotate, Space.Self);
    }
    private void ChangeAngle()
    {
        cooldownRotation = 0.25f;
        this.rotationAngle *= -1f;
    }
    public void SetPlayerRotationSpeed(float velocity){
        this.speedToRotate = velocity;
    }
}
