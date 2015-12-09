using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Sprite dmgSprite; //sotre a damage sprite
	public int hp = 4; //the hit point.

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent <SpriteRenderer>();
	}

	public void DamageWall(int loss){
		spriteRenderer.sprite = dmgSprite;
		hp -= loss;
		if (hp <= 0)
			gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
