using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{   
    /* Private variables */
    private List<Follower> followers = new List<Follower>();
    private Controls controls;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() => controls = new Controls();

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable() {
        controls.Enable();

        // Set up the actions for the controls
        controls.Player.MoveUp.performed += ctx => {
            if (Direction != Vector2.down) ChangeDirection(Vector2.up);
        };
        controls.Player.MoveRight.performed += ctx => {
            if (Direction != Vector2.left) ChangeDirection(Vector2.right);
        };
        controls.Player.MoveDown.performed += ctx => {
            if (Direction != Vector2.up) ChangeDirection(Vector2.down);
        };
        controls.Player.MoveLeft.performed += ctx => {
            if (Direction != Vector2.right) ChangeDirection(Vector2.left);
        };
    }

    private void ChangeDirection(Vector2 newDirection) => Direction = newDirection;

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable() => controls.Disable();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        bc.size = new Vector2(0.8f, 0.45f);
        previousPositions = new Queue<Vector2>();
        Direction = Vector2.up; // Start moving up
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Border":
                // GameManager.instance.GameOver();
                break;
        }
    }

    protected override void Move() => rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);

    public override void RecruitFollower(Follower newFollower)
    {
        if (followers.Count == 0)
            follower = newFollower;

        followers.Insert(followers.Count, newFollower);
    }

    /* Getters and Setters */
    public int GetFollowerCount() => followers.Count;
    public Follower GetLastFollower() => followers.Count != 0 ? followers[followers.Count - 1] : null;
}
