using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator animator;
    //Property for Health for enemies includes a set call to Defeated when health drops to 0
    public float Health {
        set {
            health = value;
            if(health <= 0) {
                Defeated();
            }
        }
        get {
            return health;
        }
    }
    //Total Health of each slime
    public float health = 2;

    private void Start(){
        animator = GetComponent<Animator>();
    }
    //Starts death animation
    public void Defeated(){
        animator.SetTrigger("Death");
    }
    //Removes game object from world
    public void RemoveEnemy(){
        Destroy(gameObject);
    }
}
