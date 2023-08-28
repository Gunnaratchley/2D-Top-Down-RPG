using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public enum AttackDirection {
        left, right, front, back
    }
    //Sword damage
    public float damage = 1;
    public AttackDirection attackDirection;
    public Collider2D swordCollider;
    Vector2 rightAttackOffset;
    // Start is called before the first frame update
    void Start()
    {
      swordCollider = GetComponent<Collider2D>(); 
      rightAttackOffset = transform.position;
    }
    //Attack is called to determine which direction the hitbox should face for proper collision
    public void Attack(){
        switch(attackDirection) {
            case AttackDirection.left:
                AttackLeft();
                break;
            case AttackDirection.right:
                AttackRight();
                break;
            case AttackDirection.front:
                AttackFront();
                break;
            case AttackDirection.back:
                AttackBack();
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void AttackRight() {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }
    //x axis coord fliped in order to push hitbox to left of player to cover left sword attack
    private void AttackLeft() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3((rightAttackOffset.x * -1), rightAttackOffset.y);
    }
    //resets hitbox to center of character then shifts down slightly on y axis to cover down/front sword attack
    private void AttackFront() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector2((rightAttackOffset.x * 0), (rightAttackOffset.y - .05f));
    }
    //resets hitbox to center of charachter, flips the y axis and then the hitbox is slightly shifted above player to cover up/back sword attack
    private void AttackBack() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3((rightAttackOffset.x * 0), (rightAttackOffset.y * -1) - .2f);
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    //Event that fires when sword collision intersects with another collision object
    private void OnTriggerEnter2D(Collider2D otherObject) {
        if(otherObject.tag == "Enemy"){
            //deal some damage
            Enemy enemy = otherObject.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Health -= damage;
            }
        }
    }
}
