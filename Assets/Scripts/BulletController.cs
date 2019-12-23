using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 10;
    public float coolDownToNextShot = 0.25f;

    void Start()
    {
        Destroy(this.gameObject, 5f);
    }
    void Update()
    {
        this.transform.Translate(Vector2.up * Time.deltaTime * speed, Space.Self);
    }
    public void SetDamage(float damage){
        this.damage = damage;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("scenarioC")){
            Destroy(this.gameObject);
        }    
    }
    public void SetRotation(float z){
        this.gameObject.transform.SetPositionAndRotation(this.gameObject.transform.position, Quaternion.Euler(0,0,z));
    }
}
