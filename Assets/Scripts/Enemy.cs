﻿using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	public int playerDamage;

	private Animator animator;
	private Transform target; //store the position of the player. Enemies goes after the target
	private bool skipMove; //cause enemies to move every other turn.

	// Use this for initialization
	protected override void Start () {

		//get the components
		animator = GetComponent<Animator>();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();

	}

	protected override void AttemptMove<T> (int xDir, int yDir){
		if(skipMove)
		{
			skipMove = false;
			return;
		}

		base.AttemptMove<T> (xDir, yDir);

		skipMove = true;

	}

	public void MoveEnemy(){
		int xDir = 0;
		int yDir = 0;

		//check to see if the player and enemy are in the same column
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			//move up or down toward the player
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else 
			xDir = target.position.x > transform.position.x ? 1 : -1;

		AttemptMove<Player> (xDir, yDir);
	}

	protected override void OnCantMove<T>(T component)
	{
		Player hitPlayer = component as Player;
		hitPlayer.LoseFood (playerDamage);

	}

}
