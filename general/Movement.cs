using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;

    [SerializeField]
    int speed = 5;

    float inputX, inputY;

    //animation states
    string currentAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 dir = new Vector2(inputX, inputY).normalized;
        rb.velocity = dir * speed;

        Walk();

        if (inputY == 0 && inputX == 0)
        {
            anim.Play(currentAnim, 0, 0f); //reset animation 
        }
    }

    void ChangeAnimState(string newstate)
    {
        if (currentAnim == newstate) return;

        anim.Play(newstate);

        currentAnim = newstate;
    }

    void Walk()
    {
        if (inputX != 0 && inputY == 0)
        {
            ChangeAnimState("walk_left");
        }

        if (inputX < 0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        else if (inputX > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (inputY < 0)
        {
            ChangeAnimState("walk_down");
            transform.localScale = new Vector2(1, 1);
        }
        else if (inputY > 0)
        {
            ChangeAnimState("walk_up");
            transform.localScale = new Vector2(1, 1);
        }

    }
}
