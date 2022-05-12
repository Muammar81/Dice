using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TossTest : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;
    private float _tossForce = 60;
    private enum Sides { ONE, TWO, THREE, FOUR, FIVE, SIX }
    private Sides side;
    private Quaternion rot;
    private float angleThresh = 0.5f;
    private Quaternion initialRot;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        initialRot = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Toss();
        }

        CheckSide();
        SmoothRotation();
    }

    private void SmoothRotation()
    {
        if (IsGrounded())
        {
                    _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
           // transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * rot, Time.deltaTime * 500);
        }
    }



    private void CheckSide()
    {
        if (transform.hasChanged)
        {
            if (Vector3.Cross(Vector3.up, transform.right).magnitude < angleThresh)
            {
                if (Vector3.Dot(Vector3.up, transform.right) > 0)
                {
                    side = Sides.FIVE;
                    rot = Quaternion.Euler(Vector3.right);
                }
                else
                {
                    side = Sides.TWO;
                    rot = Quaternion.Euler(Vector3.left);

                }
            }
            else if (Vector3.Cross(Vector3.up, transform.up).magnitude < angleThresh)
            {
                if (Vector3.Dot(Vector3.up, transform.up) > 0)
                {
                    side = Sides.THREE;
                    rot = Quaternion.Euler(Vector3.up);
                }
                else
                {
                    side = Sides.FOUR;
                    rot = Quaternion.Euler(Vector3.down);
                }
            }
            else if (Vector3.Cross(Vector3.up, transform.forward).magnitude < angleThresh)
            {
                if (Vector3.Dot(Vector3.up, transform.forward) > 0)
                {
                    side = Sides.ONE;
                    rot = Quaternion.Euler(Vector3.forward);
                }
                else
                {
                    side = Sides.SIX;
                    rot = Quaternion.Euler(Vector3.back);
                }
            }
            Debug.Log(Enum.GetName(typeof(Sides), side));
            transform.hasChanged = false;
        }
    }

    private void Toss()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.AddForce(Vector3.up * _tossForce, ForceMode.Impulse);
        var torque = GetRandVector();
        _rb.AddTorque(torque);
    }

    private Vector3 GetRandVector()
    {
        var dirX = Random.Range(0, 360);
        var dirY = Random.Range(0, 360);
        var dirZ = Random.Range(0, 360);

        return new Vector3(dirX, dirY, dirZ);
    }
    private bool IsGrounded()
    {
        var distToGround = _collider.bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
