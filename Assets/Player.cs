using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //for movement
    public CharacterController2D controller;

    //movement stats and stuff
    public float runSpeed = 0.75f;
    float horizontalMove = 0f;
    bool jumpMove = false;
    bool crouch = false;

    public Player player2;

    //public Animator animator;

    //hp of the player, if it hits 0 they lose
    public int maxHealth = 100;
    private int currentHealth;

    //Layer the opposing player is on; important
    public LayerMask Player2Layer;
    
    //There absolutely is a better way to implement this but I do not have the time nor energy to do a refractor
    //Attack Stats The format is:
    //Origin point(if it has a unique one instead of using a different one
    //Size of hitbox
    //Amount of health damage dealt on successful hit
    //How long the startup is(Amount of frames until the game starts checking for hit detection and the move can deal damage)
    //Type of attack(high, mid, low)


    //Stats for basic punch
    public Transform fistPoint;
    public Vector2 fistRange = new Vector2(1, 0.5f);
    public int punchDamage = 10;
    public int punchStartup = 10;
    public string punchType = "high";

    //Stats for basic kick
    public Transform kickPoint;
    public Vector2 kickRange = new Vector2(0.5f, 1f);
    public int kickDamage = 20;
    public int kickStartup = 15;
    public string kickType = "mid";

    //Stats for shoulder tackle
    //Uses punch origin point
    public Vector2 shoulderRange = new Vector2(1.5f, 1f);
    public int shoulderDamage = 25;
    public int shoulderStartup = 20;
    public string shoulderType = "mid";

    //Variables needed to work the attack loops
    bool isBlocking = false;
    public int attackCooldown = 0;
    public int currentAttackID = 0;
    void Start()
    {
        //At game or round start reset the player's health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Set to make sure we don't block infinitely
        isBlocking = false;
        //Check if player can block before blocking
        if(attackCooldown == 0){
            if(Input.GetKeyDown(KeyCode.Alpha3)){
                isBlocking = true;
                attackCooldown = 1;
                Debug.Log("Blocking rn");
            }
        }*/
        //Check first if player can move
        if (attackCooldown == 0) {
            //This is movement
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            jumpMove = Input.GetAxisRaw("Vertical") == 1;
            crouch = Input.GetAxisRaw("Vertical") == -1;
            controller.Move(horizontalMove, crouch, jumpMove);
        }


        //Check if no other attack is in progress
        if (attackCooldown == 0) {
            //If it isn't, check if player is pressing any attack buttons, then set the cooldown to how long the attack runs
            if(Input.GetKeyDown(KeyCode.Alpha1)&& Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Begins attack startup
                attackCooldown = shoulderStartup;
                //Variable to inform the script on how to call the Attack() function
                currentAttackID = 3;
            }
            else
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                attackCooldown = punchStartup;
                currentAttackID = 1;
            }
            else
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                attackCooldown = kickStartup;
                currentAttackID = 2;
            }
        }

        //Checks if it's time to either attack or stop the recovery
        if(attackCooldown == 1)
        {
            //Checks if it's recovery or attack, 99 being recovery
            if (currentAttackID == 99)
            {
                currentAttackID = 0; //Resets the ID so that we don't end up making the same attack by accident somehow
                Debug.Log("End of recovery");
            }else if (currentAttackID == 1)
            {
                currentAttackID = 0; //Slightly unnecessary since Attack() already sets it but better safe than sorry
                Attack(fistPoint, fistRange, punchDamage, punchStartup, punchType);
                Debug.Log("You punch!");
            }
            else if (currentAttackID == 2)
            {
                currentAttackID = 0;
                Attack(kickPoint, kickRange, kickDamage, kickStartup, kickType);
                Debug.Log("You kick!");
            }
            else if (currentAttackID == 3)
            {
                currentAttackID = 0;
                Attack(fistPoint, shoulderRange, shoulderDamage, shoulderStartup, shoulderType);
                Debug.Log("Shoulder Tackle!");
            }
        }
        //Another if is inefficient but this is needed so that the whole thing can tick down without going into negatives and messing up the whole script
        if (attackCooldown > 0)
        {
            attackCooldown--;
        }
        /*void Block()
        {

        }*/


    }

    void Attack(Transform attackPoint, Vector2 attackRange, int attackDamage, int attackStartup, string attackType) //Takes origin point, hitbox size, and damage amount of attack
    {
        //Animation
        //animator.SetTrigger("Attack");

        //Hit detection
        Collider2D[] hitPlayer2 = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, Player2Layer);



        //Hit effect
        foreach (Collider2D player2 in hitPlayer2)
        {
            if (player2.TryGetComponent(out Player player) && player != this) //Checks for hitboxes that don't belong to the player object this is for
            {
                //This is not working yet, I haven't figured out how to check for the player crouching
                //if (player2.crouch == true) { 
                //if (attackType != "high") {
                Debug.Log("Hit!");
                if (isBlocking == false)
                {
                    //Debug.Log("Damaged for " + attackDamage);
                    //player2.GetComponent<player2>.TakeDamage(attackDamage);
                }
                currentAttackID = 99; //ID of recovery
                attackCooldown = 1; //Placeholder number, needs to be changed to recovery variable for the attack
                /*} else {
                    Debug.Log("Crushed!");
                }
            } else {
                Debug.Log("Hit!");
                if (isBlocking == false) {
                    //Debug.Log("Damaged for " + attackDamage);
                    //player2.TakeDamage(attackDamage);
                }
            }*/
                break;
            }

        }
    }

    //It's in the name, function for taking damage. Not fully working yet, awaiting implementation. We need block first
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //interrupt and play hurt animation
        if(currentHealth <= 0)
        {
            LoseRound();
        }
    }

    //Function to make the player lose the round, currently does nothing
    void LoseRound()
    {
        Debug.Log("You lost you doodoohead");
    }

    //This is so that the hitboxes of attacks will be displayed in the editor
    //Refractoring might be a good idea later down the line; Size will bloat as moves are added
    void OnDrawGizmosSelected()
    {
        if (fistPoint == null)
            return;
        if (kickPoint == null)
            return;

        Gizmos.DrawWireCube(fistPoint.position, fistRange);
        Gizmos.DrawWireCube(kickPoint.position, kickRange);
    }
}
