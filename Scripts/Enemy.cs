﻿using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObjects {

    public int playerDamage;

    private Transform target;
    private Animator animator;
    private bool skipMove;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    protected override void Start () {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}
	
    protected override void AttempMove<T>(int xDir, int yDir){
        if (skipMove){
            skipMove = false;
            return;
        }
        base.AttempMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy() {
        int xDir = 0;
        int yDir = 0;
        
        if (Mathf.Abs(target.position.x - transform.position.x) < Mathf.Abs(target.position.y - transform.position.y))
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttempMove<Player>(xDir, yDir);
    }

    protected override void OnCanMove<T>(T component){
        Player hitPlayer = component as Player;
        animator.SetTrigger("EnemyAttack");
        SoundManager.instance.RadomizeEfx(enemyAttack1, enemyAttack2);
        hitPlayer.LoseFood(playerDamage);
    }

}
