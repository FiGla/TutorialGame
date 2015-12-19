using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
    public Sprite dmgSprite;
    public int hp = 4;

    public AudioClip choopSound1;
    public AudioClip choopSound2;

    private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    public void DamageWall (int loss){
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        SoundManager.instance.RadomizeEfx(choopSound1, choopSound2);
        if (hp <= 0)
            gameObject.SetActive(false);
    }

}
