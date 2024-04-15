using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private Transform leftHand, rightHand;
    [SerializeField] private float rotationSpeed, jumpForce, extraImpulseGrowth;
    private float _baseJumpForce;
    private Rigidbody2D _rb;
    bool _grabbedRight, _grabbedLeft;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _grabbedRight = false;
        _grabbedLeft = false;
        _baseJumpForce = jumpForce;
    }

    void Update()
    {
        if(_grabbedRight && !_grabbedLeft)
        {
            _rb.velocity = new Vector3(0f, 0f, 0f);
            _rb.gravityScale = 0f;
            Vector3 _relativePosition = transform.position - rightHand.position;
            transform.RotateAround(rightHand.position, -Vector3.forward, rotationSpeed);
            Debug.Log("Giro en torno a la derecha");
        }
        else if(!_grabbedRight && _grabbedLeft)
        {
            _rb.velocity = new Vector3(0f, 0f, 0f);
            _rb.gravityScale = 0f;
            Vector3 _relativePosition = transform.position - leftHand.position;
            transform.RotateAround(leftHand.position, Vector3.forward, rotationSpeed);
            Debug.Log("Giro en torno a la izquierda");
        }
        else if(_grabbedRight && _grabbedLeft)
        {
            if(jumpForce < 10) jumpForce = jumpForce + extraImpulseGrowth * Time.deltaTime;
        }
        else if(!_grabbedRight && !_grabbedLeft)
        {
            _rb.gravityScale = 1f;
        }
    }

    public void GrabRight()
    {
        _grabbedRight = true;
    }

    public void GrabLeft()
    {
        _grabbedLeft = true;
    }

    public void ReleaseRight()
    {
        _grabbedRight = false;
        if(!_grabbedLeft) 
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            jumpForce = _baseJumpForce;
        }
    }

    public void ReleaseLeft()
    {
        _grabbedLeft = false;
        if(!_grabbedRight) 
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            jumpForce = _baseJumpForce;
        }
    }
}

