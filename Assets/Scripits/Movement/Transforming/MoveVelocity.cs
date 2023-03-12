using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] float moveSpeed = 0;

    private Vector3 velocityVector;
    private new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = velocityVector * moveSpeed;
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }
    public void SetVelocityX(int velocityVectorX)
    {
        this.velocityVector.x = velocityVectorX;
    }
    public void SetVelocityY(int velocityVectorY)
    {
        this.velocityVector.y = velocityVectorY;
    }
    public void SetVelocityZ(int velocityVectorZ)
    {
        this.velocityVector.z = velocityVectorZ;
    }
    public Vector3 GetVelocity()
    {
        return velocityVector;
    }
}
