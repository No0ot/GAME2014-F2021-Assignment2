using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingPlatformDirection
{
    HORIZONTAL,
    VERTICAL,
    DIAGONAL_UP,
    DIAGONAL_DOWN,
    NUM_OF_DIRECTIONS
}

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public MovingPlatformDirection direction;
    [Range(0.1f, 10.0f)]
    public float speed;
    [Range(1, 20)]
    public float distance;
    [Range(0.05f, 0.1f)]
    public float distanceOffset;
    public bool isLooping;

    private Vector2 startingPosition;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        if (isLooping)
        {
            isMoving = true;
        }
    }

    private void MovePlatform()
    {
        float pingPongValue = (isMoving) ? Mathf.PingPong(Time.time * speed, distance) : distance;

        if ((!isLooping) && (pingPongValue >= distance - distanceOffset))
        {
            isMoving = false;
        }

        switch (direction)
        {
            case MovingPlatformDirection.HORIZONTAL:
                transform.position = new Vector2(startingPosition.x + pingPongValue, transform.position.y);
                break;
            case MovingPlatformDirection.VERTICAL:
                transform.position = new Vector2(transform.position.x, startingPosition.y + pingPongValue);
                break;
            case MovingPlatformDirection.DIAGONAL_UP:
                transform.position = new Vector2(startingPosition.x + pingPongValue, startingPosition.y + pingPongValue);
                break;
            case MovingPlatformDirection.DIAGONAL_DOWN:
                transform.position = new Vector2(startingPosition.x + pingPongValue, startingPosition.y - pingPongValue);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(this.gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
