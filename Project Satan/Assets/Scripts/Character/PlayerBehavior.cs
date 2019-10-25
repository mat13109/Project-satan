﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    bool stunned;
    bool dead = false;

    GameManager gm;
    Animator pauseUIAnimator;
    [SerializeField] Animator playerAnimator;

    // The speed of the character movement
    [SerializeField] float movSpeed;
    [SerializeField] int team;
    [SerializeField] GameObject deathParticles;
    [SerializeField] float shootStrength;
    [SerializeField] float Stuck = 1.0f;


    // Will contain the rigidbody of the character
    Rigidbody2D rb;
    // Will contain the WASD/left-stick axis' values
    Vector2 movementValues;
    // Last orientation
    Vector2 lastMovementValues = new Vector2(0, 0);


    // Once at scene load
    private void Start()
    {
        // gets the rigidbody
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseUIAnimator = GameObject.Find("PauseUI").GetComponent<Animator>();
    }

    public void SetStuck(float force)
    {
        Stuck = force;
        StartCoroutine(Unstuck(1.5f));
    }
    
    IEnumerator Unstuck(float duration)
    {
        yield return new WaitForSeconds(duration);
        if(Stuck > 1)
            SetStuck(1);
    }

    // Triggered when E/gamepad-south is pressed
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed == true) // only on keydown
        {
            playerAnimator.SetTrigger("kick");
            Collider2D[] colliderShoot = Physics2D.OverlapCircleAll(transform.position, 1f);
            for (int i = 0; i < colliderShoot.Length; i++)
            {
                if (colliderShoot[i].CompareTag("Bomb"))
                {
                    colliderShoot[i].GetComponent<Rigidbody2D>().AddForce(lastMovementValues.normalized * shootStrength);
                    Debug.Log((lastMovementValues * shootStrength));
                }
                   
            }
        }
    }

    // Triggered when WASD/left-stick is used
    public void GetMovementValues(InputAction.CallbackContext context)
    {
        if (!stunned)
        {
            movementValues = context.ReadValue<Vector2>(); // store the value of the WASD/left-stick
            
            
        }
            
        else
        {
            movementValues = Vector2.zero;
            

        }
            
    }

    // Once per frame
    private void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            playerAnimator.SetFloat("h", rb.velocity.x);
            playerAnimator.SetFloat("v", rb.velocity.y);
            playerAnimator.SetFloat("ismoving", 1f);
        }
        else
        {
            playerAnimator.SetFloat("ismoving", 0f);
        }
        
                // moves the character with the rigidbody and prevents frame drops
                switch (stunned)
                {
                    case true:
                        //Player stunned => no control 
                        break;
                    case false:
                        rb.velocity = movementValues * movSpeed * Time.deltaTime / Stuck;
                        break;
                }

        // Moves on ice
        //rb.GetComponent<ConstantForce2D>().force = new Vector2(movementValues.x, movementValues.y) * Time.deltaTime * movSpeed;


        //save the movement
        if (movementValues != new Vector2(0, 0))
            lastMovementValues = movementValues;
        else
            if(team == 2)
                lastMovementValues = new Vector2(-1, 0);
            else
                lastMovementValues = new Vector2(1, 0);


        switch (team)
        {
            case 1:
                if (transform.position.x > 0)
                {
                    transform.position = new Vector2(0, transform.position.y);
                }
                if (stunned == false)
                {
                    if (transform.position.x < -8)
                    {
                        transform.position = new Vector2(-8, transform.position.y);
                    }
                    if (transform.position.y > 4)
                    {
                        transform.position = new Vector2(transform.position.x, 4);
                    }
                    if (transform.position.y < -4)
                    {
                        transform.position = new Vector2(transform.position.x, -4);
                    }
                }
                break;
            case 2:
                if (transform.position.x < 0)
                {
                    transform.position = new Vector2(0, transform.position.y);
                }
                if (!stunned)
                {
                    if (transform.position.x > 8)
                    {
                        transform.position = new Vector2(8, transform.position.y);
                    }
                    if (transform.position.y > 4)
                    {
                        transform.position = new Vector2(transform.position.x, 4);
                    }
                    if (transform.position.y < -4)
                    {
                        transform.position = new Vector2(transform.position.x, -4);
                    }
                }

                break;
        }

    }

    public void KillYourself()
    {
        GameObject temp = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(temp, 5);
        StopCoroutine("CooldownToGetUnstunned");
        stunned = true;
        dead = true;
        Invoke("LoadNewScene", 3);
        if (!gm.oneisdead)
        {
            gm.OneIsDead();
            gm.Shake();
            ScoreManager.RemoveOneLifeTo(team);
        }
        
        
        //Destroy(gameObject);
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Glue")
        {
            SetStuck(4.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Glue")
        {
            SetStuck(1.0f);
        }
    }

    public void GetBackToStartMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            pauseUIAnimator.SetBool("paused", !pauseUIAnimator.GetBool("paused"));
        
    }

    public void QuitMenu()
    {
        if (pauseUIAnimator.GetBool("paused") == true)
            pauseUIAnimator.SetBool("paused", false);

    }

    public void GetStunnned(float i)
    {
        stunned = true;
        StartCoroutine(CooldownToGetUnstunned(i));
    }

    IEnumerator CooldownToGetUnstunned(float i)
    {
        StopCoroutine("CooldownToGetUnstunned");
        yield return new WaitForSeconds(i);
        if(!dead)
            stunned = false;
        GetComponent<ConstantForce2D>().force = Vector2.zero;
    }

    void LoadNewScene()
    {
        SceneManager.LoadScene("FinalGameplay");
    }

    public void ExitTheGame(InputAction.CallbackContext context)
    {
        if (context.performed && GameObject.Find("PauseUI").GetComponent<Animator>().GetBool("paused"))
            SceneManager.LoadScene("Menu");
    }
}
