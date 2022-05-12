using Photon.Pun;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceController : MonoBehaviourPun
{
    [SerializeField] private Color[] _colors;
    private Rigidbody _rb;
    private float _tossForce = 5;
    private Material _mat;
    private Collider _collider;
    private DiceRotation diceRot;

    private void OnEnable()
    {
        InputManager.OnInput += InputManager_OnInput;
    }

    private void OnDisable()
    {
        InputManager.OnInput -= InputManager_OnInput;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mat = GetComponent<Renderer>().material;
        _collider = GetComponent<Collider>();
        diceRot = GetComponent<DiceRotation>();
    }


    private void xUpdate()
    {
        if (IsGrounded())
            Debug.Log(Enum.GetName(typeof(Sides),side));
    }
    private void FixedUpdate()
    {
        if (IsGrounded() && _rb.velocity.magnitude > 0.001f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 4);
        }
    }
    enum Sides { UP,DOWN,LEFT,RIGHT}
    Sides side;
    private Quaternion rot;

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (Mathf.Approximately(angle, 0))
        {
            side = Sides.DOWN;
            rot = Quaternion.Euler(Vector3.down);
        }

        if (Mathf.Approximately(angle, 180))
        {
            side = Sides.UP;
            rot = Quaternion.Euler(Vector3.up);
        }

        //Sides
        if (Mathf.Approximately(angle, 90))
        {
            Vector3 cross = Vector3.Cross(Vector3.forward, hit);
            if (cross.y > 0)
            {
                side = Sides.LEFT;
                rot = Quaternion.Euler(Vector3.left);
            }
            else
            {
                side = Sides.RIGHT;
                rot = Quaternion.Euler(Vector3.right);
            }
        }

        if (IsGrounded())
            StopRB();
    }

    private void StopRB()
    {
        if (_rb.velocity.magnitude < .01)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.angularDrag = 0;
            
        }

        //_rb.isKinematic = true;
        //diceRot.enabled = true;
    }

    private void InputManager_OnInput(InputKey key)
    {
        if (key == InputKey.Roll && IsGrounded() && photonView.IsMine)
        {
            var torque = GetRandVector();
            photonView.RPC(nameof(Roll), RpcTarget.All, torque);

            var colorIndex = Random.Range(0, _colors.Length - 1);
            photonView.RPC(nameof(ChangeColor), RpcTarget.All, colorIndex);
        }
    }

    private bool IsGrounded()
    {
        var distToGround = _collider.bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    [PunRPC]
    private void Roll(Vector3 torque)
    {
        diceRot.enabled = false;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(Vector3.up * _tossForce, ForceMode.Impulse);
        _rb.AddTorque(torque);
    }

    [PunRPC]
    private void ChangeColor(int colorIndex)
    {
        _mat.color = _colors[colorIndex];
    }
    private Vector3 GetRandVector()
    {
        var dirX = Random.Range(0, 360);
        var dirY = Random.Range(0, 360);
        var dirZ = Random.Range(0, 360);

        return new Vector3(dirX, dirY, dirZ);
    }
}
