using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int life = 3;
    [SerializeField] float speedToRotate;
    float rotationAngle = 1;
    GameObject player;
    float cooldownRotation;
    Transform bulletSpawnPos;
    short selectedBullet = 0;
    float _coolDownToShootAgain;
    bool canShoot; 
    public bool dead = true;
    
    [Header("HUD")]
    public UnityEngine.UI.Image selectedBulletImage;
    
    void Start()
    {
        player = this.gameObject;
        bulletSpawnPos = gameObject.transform.GetChild(0);
    }
    void Update()
    {
        if(dead)
            return;
        CheckLife();
        RotatePlayer();
        CheckShoot();
        ChangeWeapon();
    }

    private void CheckLife()
    {
        if(life <= 0 && !dead){
            Die();
        }
        
    }
    public SpriteRenderer GetSelectedWeaponSprite(){
        return GameManager.GM.bullets[selectedBullet].GetComponent<SpriteRenderer>();
    }
    private void Die()
    {
        dead = true;
        this.GetComponent<Animator>().SetTrigger("death");
        GameManager.GM.GameOver();
    }
    void SelectWeapon(int n){
        this.selectedBullet += (short)n;
        if(selectedBullet >= GameManager.GM.bullets.Length){
            this.selectedBullet = 0;
        }
        if(selectedBullet < 0){
            this.selectedBullet = (short)(GameManager.GM.bullets.Length-1);
        }
        UpdateDisplayBullet();
    }
    public void UpdateDisplayBullet(){
        this.selectedBulletImage.sprite = GetSelectedWeaponSprite().sprite;
        this.selectedBulletImage.color = GetSelectedWeaponSprite().color;
    }
    public void ReceiveDamage(int damage){
        if(dead)
            return;
        life -= damage;
        this.GetComponent<Animator>().SetTrigger("Hit");
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
        _coolDownToShootAgain -= Time.deltaTime;
        canShoot = _coolDownToShootAgain <= 0;
        if(Input.GetKeyDown(KeyCode.Space) && canShoot){

            GameObject bullet = Instantiate(GameManager.GM.bullets[selectedBullet],bulletSpawnPos.position, this.transform.rotation);
            _coolDownToShootAgain = bullet.GetComponent<BulletController>().coolDownToNextShot;
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
