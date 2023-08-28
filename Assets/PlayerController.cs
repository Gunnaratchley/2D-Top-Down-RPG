using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    bool canMove = true;
    public SwordAttack swordAttack;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
  
    private void FixedUpdate() {
        if(canMove){
            //If movment input is not 0, try to move
            if(movementInput != Vector2.zero){
                bool success = TryMove(movementInput);

                if(!success){
                    success = TryMove(new Vector2(movementInput.x, movementInput.y = 0));
                }
                if(!success){
                    success = TryMove(new Vector2(movementInput.x = 0, movementInput.y));
                }
                

                animator.SetBool("isMovingXAxis", success);
            }else {
                animator.SetBool("isMovingXAxis", false);
            }

            //set direction of sprite on x axis
            //Flips so the sprite can go left all others are animated
            if(movementInput.x < 0) {
                spriteRenderer.flipX = true;
                swordAttack.attackDirection = SwordAttack.AttackDirection.left;
            } else if (movementInput.x > 0) {
                spriteRenderer.flipX = false;
                swordAttack.attackDirection = SwordAttack.AttackDirection.right;
            }
            //set animation for up and down movement
            //must set other directions false to correctly register
            if(movementInput.y < 0) {
                animator.SetBool("isMovingYNeg", true);
                animator.SetBool("isMovingYPos", false);
                animator.SetBool("isMovingXAxis", false);
                swordAttack.attackDirection = SwordAttack.AttackDirection.front;
            } else if (movementInput.y > 0) {
                animator.SetBool("isMovingYPos", true);
                animator.SetBool("isMovingYNeg", false);
                animator.SetBool("isMovingXAxis", false);
                swordAttack.attackDirection = SwordAttack.AttackDirection.back;
            }
            //If Player not moving then sit in idle animation
            //Must account for all directions to determine if not moving
            if(movementInput.x == 0 && movementInput.y == 0){
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingYNeg", false);
                animator.SetBool("isMovingYPos", false);
                animator.SetBool("isMovingXAxis", false);
            } else if (movementInput.x != 0 || movementInput.y != 0) {
                animator.SetBool("isMoving", true);
            }
        }
    }

    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero) {
            //Check for potential collisions
            //direction is x, y values between 1 and -1
            //movementFilter is the settings that determine where collisions can occur
            //castCollisions is a list of collisions to store after a cast is identified
            //Calculates the amount to cast equal to movement plus the offest
            int count = rb.Cast(direction, movementFilter, castCollisions, (moveSpeed * Time.fixedDeltaTime + collisionOffset));

            if(count == 0){
                rb.MovePosition(rb.position + (movementInput * Time.fixedDeltaTime * moveSpeed));
                return true;
            } else {
                return false;
            }
        } else {
            //Can't move if there is no direction to go
            return false;
        }
    }
    //Grabs information for player asset during movement
    void OnMove(InputValue movementValue)
    {
            movementInput = movementValue.Get<Vector2>();
    }
    //Event fires on left mouse click and initiates sword attack animation.
    void OnFire(){
        animator.SetTrigger("swordAttack");
    }
    //Event for animation to move hitbox in reference to the direction of the animation
    public void SwordAttackHitbox() {
        LockMovement();
        swordAttack.Attack();
    }
    //Event to end animation for sword attack
    public void EndSwordAttack(){
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }
}
