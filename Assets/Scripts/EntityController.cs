using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Morcego Stats")]
    float startLife; public float life = 1;
    public bool died;
    float offSet = 2f;
    float cameraBorderLimit;

    float startSpeed;
    [SerializeField] float speed;
    float cooldownMoveSide;
    private float moveSide = 1;
    private Vector3 startPos;
    float cameraSize;
    public int score = 2;
    int damage = 1;

    void Start()
    {
        SetStartStatus();
    }

    private void SetStartStatus()
    {
        cameraBorderLimit = GameManager.GM.GetBorder();
        cameraSize = GameManager.GM.cameraSize;
        startLife = life;
        startSpeed = speed;
        startPos = this.transform.position;

        if(UnityEngine.Random.Range(0, 100) > 50)
            ChangeMoveSide();
    }

    void Update()
    {
        if (life <= 0 && !died)
        {
            StartCoroutine(Die());
            return;
        }
        
        if(!died){
            Move();
        }
    }

    private void Move()
    {
        cooldownMoveSide -= Time.deltaTime;
        if(this.gameObject.transform.position.x > cameraBorderLimit - offSet || this.gameObject.transform.position.x < -cameraBorderLimit + offSet ){
            if (cooldownMoveSide <= 0f)
                ChangeMoveSide();
        }
        this.gameObject.transform.Translate((Vector2.right * moveSide) * Time.deltaTime * speed*2, Space.Self);
        this.gameObject.transform.Translate(Vector3.down * Time.deltaTime * speed, Space.Self);

        if(this.gameObject.transform.position.y < -cameraSize-2){
            StartCoroutine(Die(false,false));
        }
    }
    private void ChangeMoveSide()
    {
        cooldownMoveSide = 0.25f;
        this.moveSide *= -1f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<BulletController>()){
            life -= other.GetComponent<BulletController>().damage;
        }
        if(other.GetComponent<PlayerController>()){
            other.GetComponent<PlayerController>().ReceiveDamage(damage);
        }
    }

    IEnumerator Die(bool objectPooling = false, bool normalDeath = true){
        
        died = true;
        
        if(!normalDeath){
            Destroy(this.gameObject);
            yield return null;
        }

        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("death");
        BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        GameManager.GM.score += this.score;
        if(objectPooling)
            ResetEntity();
        else{
            GameManager.GM.RemoveEnemyFromSpawned(this.gameObject);
            yield return new WaitForEndOfFrame();
            Destroy(this.gameObject,anim.GetCurrentAnimatorStateInfo(0).length);
        }
        yield return null;
    }

    private void ResetEntity()
    {
        died = false;
        life = startLife;
        speed = startSpeed;
        this.transform.position = startPos;
    }
}
