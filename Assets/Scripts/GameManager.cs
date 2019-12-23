using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    [Header("GameConfs")]
    public GameObject[] bullets;
    public GameObject[] enemies;
    public List<GameObject> spawnedEnemies;
    [SerializeField] ushort maxEnemiesSpawnedsOnGame = 10;
    [SerializeField] float cooldownToSpawn = 1.75f;

    [Header("Menu")]
    [SerializeField] TMPro.TMP_Text scoreTXT;
    [SerializeField] GameObject canvasMenu;
    float borderLimit;
    [HideInInspector] public float spawnPointY, offSetSpawn = 2;
    float _cooldownToSpawn;
    [HideInInspector] public float cameraSize;
    bool canSpawnEnemies = false;
    float _score; public float score { get{ return _score; } set{_score = value; if(scoreTXT != null) scoreTXT.SetText("Score: " + _score);}}
    

    void Awake()
    {
        if(GM == null)
            GM = this;
        else
            Destroy(this.gameObject);

        cameraSize = FindObjectOfType<Camera>().orthographicSize;
        borderLimit = cameraSize * Screen.width/Screen.height;
    }
    void Start()
    {
        spawnPointY = FindObjectOfType<Camera>().orthographicSize;
        _cooldownToSpawn = cooldownToSpawn;
        GetBullets();
        GetEnemies();
    }
    void Update()
    {
        if(canSpawnEnemies)
            EnemyManager();
    }
    public void StartGame(){
        StartCoroutine(StartGameCoroutine());
    }
    System.Collections.IEnumerator StartGameCoroutine(){
        canvasMenu.GetComponent<Animator>().SetTrigger("start");    
        yield return new WaitForEndOfFrame();
        Invoke("disableMenu" ,canvasMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        scoreTXT.transform.parent.gameObject.SetActive(true);
        canSpawnEnemies = true;
        PlayerController player =  FindObjectOfType<PlayerController>();
        player.dead = false;
        player.UpdateDisplayBullet();
        yield return null;
    }
    void disableMenu(){
        canvasMenu.SetActive(false);
    }
    void EnemyManager(){
        _cooldownToSpawn -= Time.deltaTime;
        if(spawnedEnemies.Count < maxEnemiesSpawnedsOnGame && _cooldownToSpawn < 0){
            SpawnEnemy(enemies[UnityEngine.Random.Range(0,enemies.Length)]);
            _cooldownToSpawn = cooldownToSpawn;
        }
    }

    public void GameOver()
    {
        canSpawnEnemies = false;
        ShowGameOverMenu();
    }
    void ShowGameOverMenu(){
        canvasMenu.SetActive(true);
        for (int i = 0; i < canvasMenu.transform.childCount; i++)
        {            
            canvasMenu.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < canvasMenu.transform.childCount; i++)
        {
            if(canvasMenu.transform.GetChild(i).name == "Restart_BTN")
                canvasMenu.transform.GetChild(i).gameObject.SetActive(true);
            if(canvasMenu.transform.GetChild(i).name == "Score_TXT"){
                canvasMenu.transform.GetChild(i).gameObject.SetActive(true);
                canvasMenu.transform.GetChild(i).GetComponent<TMPro.TMP_Text>().SetText("Score: " + score);
            }
        }
    }
    public void RestartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    private void GetEnemies()
    {
        enemies = Resources.LoadAll<GameObject>("Enemies");
    }
    private void GetBullets()
    {
        bullets = Resources.LoadAll<GameObject>("Bullets");
    }
    public float GetBorder()
    {
        return borderLimit;  
    }

    public GameObject SpawnEnemy(GameObject enemyToSpawn, bool objectPooling = false){
        if(!objectPooling){
            GameObject enemy = Instantiate(enemyToSpawn, new Vector3(UnityEngine.Random.Range(-borderLimit+3f,borderLimit-3f),spawnPointY + offSetSpawn,0),Quaternion.identity);
            spawnedEnemies.Add(enemy);
            return enemy;
        }
        else
            for (int i = 0; i < spawnedEnemies.Count; i++)
                if(spawnedEnemies[i].GetComponent<EntityController>().died)
                    return spawnedEnemies[i];

        return null;
    }
    public void RemoveEnemyFromSpawned(GameObject enemyToRemove){
        if(spawnedEnemies.Contains(enemyToRemove))
            spawnedEnemies.Remove(enemyToRemove);
    }
}
