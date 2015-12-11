using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

	public int wallDamage = 1;
	public int pointsPerFood = 10; //add 10 points when picking up
	public int pointsPerSoda = 20; //add 20 points when picking up
	public float restartLevelDelay = 1f; //wait for a second to restart a level
	public Text foodText;

	private Animator animator; //store animaiton
	private int food; //food points for player to stay alive



	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator>();
		food = GameManager.instance.playerFoodPoints;

		foodText.text = "Food: " + food;

		base.Start ();
	}

	private void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn)
			return;

		//varibables for x and y cordinate.
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		//prevent the player from moving in diagnal direction.
		if(horizontal != 0)
			vertical = 0;

		//if either horizontal or vertical is not zero, then we attemp to move
		if (horizontal != 0 || vertical != 0)
			AttemptMove<Wall> (horizontal, vertical);
	}

	protected override void AttemptMove<T>(int xDir, int yDir){
		food--;
		foodText.text = "Food: " + food;

		base.AttemptMove<T> (xDir, yDir);

		RaycastHit2D hit;

		CheckIfGameOver ();

		GameManager.instance.playersTurn = false;
	}

	protected void OnTriggerEnter2D(Collider2D other){

		Debug.Log ("I was here");
		if (other.tag == "Exit") {
			Debug.Log("I Will load another level");
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			foodText.text = "+" + pointsPerFood + " Food: "+ food;
			other.gameObject.SetActive(false);
		}else if(other.tag == "Soda"){
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerSoda + " Food: "+ food;

			other.gameObject.SetActive(false);

		}
	}

	protected override void OnCantMove<T>(T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}

	private void Restart(){

		Application.LoadLevel (Application.loadedLevel);
		Debug.Log ("I just loaded a level");
	}

	public void LoseFood(int loss){
		animator.SetTrigger ("playerHit");
		food -= loss;
		foodText.text = "-" + loss + " Food:" + food;
		CheckIfGameOver ();
	}

	private void CheckIfGameOver(){
		if (food <= 0)
			GameManager.instance.GameOver ();
	}
}
