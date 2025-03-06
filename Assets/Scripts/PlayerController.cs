using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Animator animator;
    private new Rigidbody2D rigidbody;
    private Vector3 velocity;

    public float speed;

    void Start() {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        if (hor != 0 || ver != 0) {
            animator.SetFloat("Horizontal", hor);
            animator.SetFloat("Vertical", ver);
            animator.SetFloat("Speed", 1);

            Vector3 direction = (Vector3.up * ver + Vector3.right * hor).normalized;
            velocity = direction * speed;
        } else {
            animator.SetFloat("Speed", 0);
            velocity = Vector3.zero;
        }
    }

    void FixedUpdate() {
        rigidbody.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }
}
