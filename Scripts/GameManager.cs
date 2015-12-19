using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playerTurn = true;

    public float turnDelay = .1f;

    private List<Enemy> enemyList;
    private bool enemiesMoving;
    private int level = 1;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;

    void Awake () {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemyList = new List<Enemy>();
        boardScript =GetComponent<BoardManager>();
        InitGame();
	}

    private void OnLevelWasLoaded(int index) {
        level++;
        InitGame();

    }

    void InitGame() {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text> ();
        levelText.text = "Day #" + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemyList.Clear();
        boardScript.SetupScene(level);

    }

    private void HideLevelImage() {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver() {

        levelText.text = "After " + level + " days, you starved ";
        levelImage.SetActive(true);
        //SoundManager.instance.efxSource.Stop();
        enabled = false;
        //Application.Quit();


    }


    void Update () {
        if (playerTurn || enemiesMoving || doingSetup)
            return;

        
        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy script) {
        enemyList.Add(script);
    }

    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemyList.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < enemyList.Count; ++i) {
            enemyList[i].MoveEnemy();
            yield return new WaitForSeconds(enemyList[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }

}

