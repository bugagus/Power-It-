using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private Transform leftHand, rightHand;
    [SerializeField] private float rotationSpeed;
    bool _grabbedRight, _grabbedLeft;

    void Start()
    {
        _grabbedRight = false;
        _grabbedLeft = false;
    }

    void Update()
    {
        if(_grabbedRight && !_grabbedLeft)
        {
            Vector3 _relativePosition = transform.position - rightHand.position;
            transform.RotateAround(rightHand.position, -Vector3.forward, rotationSpeed);
            Debug.Log("Giro en torno a la derecha");
        }
        else if(!_grabbedRight && _grabbedLeft)
        {
            Vector3 _relativePosition = transform.position - leftHand.position;
            transform.RotateAround(leftHand.position, Vector3.forward, rotationSpeed);

            Debug.Log("Giro en torno a la izquierda");
        }
        else if(_grabbedRight && _grabbedLeft)
        {
            Debug.Log("Salto");
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
    }

    public void ReleaseLeft()
    {
        _grabbedLeft = false;
    }
}

