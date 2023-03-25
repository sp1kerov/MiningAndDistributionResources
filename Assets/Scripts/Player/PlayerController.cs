using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _fixedJoystick;
    [SerializeField] private float _speedMove = 5f;

    private void FixedUpdate()
    {
      // _rigidbody.velocity = new Vector3(_fixedJoystick.Horizontal * _speedMove, _rigidbody.velocity.y, _fixedJoystick.Vertical * _speedMove);

        Vector3 moveVector = (Camera.main.transform.right * _fixedJoystick.Horizontal + Camera.main.transform.forward * _fixedJoystick.Vertical).normalized;

        transform.Translate(moveVector * _speedMove * Time.deltaTime);
      // _rigidbody.MoveRotation(Quaternion.LookRotation(moveVector));

       /* if (_fixedJoystick.Horizontal != 0 || _fixedJoystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(moveVector);
        }*/
    }
}
