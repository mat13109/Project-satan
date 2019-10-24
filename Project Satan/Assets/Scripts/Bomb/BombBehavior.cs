﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    [SerializeField] float explosionRadius = 10;
    [SerializeField] float radius = 10;
    [SerializeField] float power = 50;
    [SerializeField] float speed = .5f;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject puddle = null;
 //   [SerializeField] float stuckDuration = 1.0f;
    private GameObject Players;
       
       

    public Rigidbody2D bombRB;
    public string type;

<<<<<<< Updated upstream

=======
    private void Update()
    {
        if (Time.time - starttime > 2.5f) { 
            alpha += 20;
            gameObject.GetComponentInChildren<Transform>().localScale += new Vector3(.03f, .03f, 0f);

        //    gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
        }
    }
>>>>>>> Stashed changes


    private void Start()
    {
        Cursor.visible = false;

        switch (type)
        {
            case "explosion":
                Invoke("Explode", 3); 
                break;
            case "siphon":
                Invoke("Implode", 3);
                break;
            case "humiliator":
                Invoke("Humiliation", 3);
                break;
            case "glue":
                Invoke("Glued", 3);
                break;
        }
        
        bombRB.AddForce(transform.right * speed);
        
    }

    private void Explode()
    {
        Vector3 explosionPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, radius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null && hit.gameObject != gameObject)
            {
                rb.velocity = Vector2.zero;
                try
                {
                    hit.GetComponent<PlayerBehavior>().GetStunnned(1);
                } catch
                {
                    //it's not a player
                }
<<<<<<< Updated upstream
                rb.AddExplosionForce(power, explosionPosition, radius);

            }
                
=======
                if(hit.CompareTag("Bomb")) 
                    rb.AddExplosionForce(power/4, explosionPosition, radius);
                else 
                    rb.AddExplosionForce(power, explosionPosition, radius);
                }
>>>>>>> Stashed changes
        }
        GameObject temp = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(temp, 1);
        Destroy(gameObject);
    }

    private void Glued()
    {
        Vector3 explosionPosition = transform.position;
        Instantiate(puddle, explosionPosition, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Humiliation()
    {
        Vector3 explosionPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, radius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null && hit.gameObject != gameObject)
            {
                rb.velocity = Vector2.zero;
                try
                {
                    hit.GetComponent<PlayerBehavior>().GetStunnned(1);
                }
                catch
                {
                    //it's not a player
                }
                rb.AddExplosionForce(power, explosionPosition, radius);

            }

        }
        GameObject temp = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(temp, 1);
        Destroy(gameObject);
    }

    private void Implode()
    {
        Vector3 explosionPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, radius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null && hit.gameObject != gameObject)
            {
                rb.velocity = Vector2.zero;
                try
                {
                    hit.GetComponent<PlayerBehavior>().GetStunnned(1);
                }
                catch
                {
                    //it's not a player
                }
                rb.AddExplosionForce(-power, explosionPosition, radius);
            }
        }
        GameObject temp = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(temp, 1);
        Destroy(gameObject);
    }

}
