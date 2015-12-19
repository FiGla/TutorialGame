using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : MovingObjects{

    public int wallDamage = 1;
    public int pointInSoda = 20;
    public int pointInFood = 10;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private Animator animator;
    private int food;
    private Vector2 touchOrign = -Vector2.one;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;


    // Use this for initialization
    protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food " + food;
        base.Start();

	}

    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update() {

        if (!GameManager.instance.playerTurn) return;

        int horizontal = 0;
        int vertical = 0;
//#if UNITY_STANDALONE || UNITY_WEBPLAYER
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        
        if (horizontal != 0)
            vertical = 0;
        /*
#else
        if (Input.touchCount > 0) {
            Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
            {
                touchOrign = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && touchOrign.x >=0) {
                Vector2 myTouchEnd = myTouch.position;
                float x = myTouchEnd.x - touchOrign.x;
                float y = myTouchEnd.y - touchOrign.y;
                touchOrign.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    horizontal = x > 0 ? 1 : -1;
                }
                else vertical = y > 0 ? 1 : -1;
            }

        }

#endif*/
        if (horizontal != 0 || vertical != 0)
            AttempMove<Wall>(horizontal, vertical);
    }


    protected override void OnCanMove<T>(T component){
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerHit");
    }


    protected override void AttempMove<T>(int xDir, int yDir){
        food--;
        foodText.text = "Food " + food;
        
        base.AttempMove<T>(xDir, yDir);
        RaycastHit2D hit;

        if(Move(xDir,yDir , out hit)){
            SoundManager.instance.RadomizeEfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();
        
        GameManager.instance.playerTurn = false;


    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointInFood;
            foodText.text = "+ " + pointInFood + " Food " + food;
            SoundManager.instance.RadomizeEfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda") {
            food += pointInSoda;
            foodText.text = "+ " + pointInSoda + " Food " + food;
            SoundManager.instance.RadomizeEfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    public void LoseFood (int loss){
        animator.SetTrigger("PlayerChoop");
        food -= loss;
        foodText.text = "- " + loss + " Food " + food;
        CheckIfGameOver();
    }


    private void Restart() {
        
        Application.LoadLevel(Application.loadedLevel);

    }


    private void CheckIfGameOver()
    {
        if (food <= 0){
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            
            GameManager.instance.GameOver();
        }

    }
}
