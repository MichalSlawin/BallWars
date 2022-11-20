using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private const float SUB_VAL = 0.1F;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CinemachineVirtualCamera _camera;

    private float _mouseSpeed = 2.0F;
    private float _moveForce = 2F;
    private CinemachineTransposer _cinemachineTransposer;
    private bool _moving = false;

    private void Awake()
    {
        _cinemachineTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        MoveCamera();
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(_moving)
            {
                return;
            }

            _rigidbody.AddForce(transform.forward * _moveForce, ForceMode.Impulse);
            _cinemachineTransposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            StartCoroutine(CheckMovement());
        }
    }

    private void MoveCamera()
    {
        float h = _mouseSpeed * Input.GetAxis("Mouse X");
        float v = _mouseSpeed * Input.GetAxis("Mouse Y");

        transform.Rotate(v, h, 0);
    }

    private IEnumerator CheckMovement()
    {
        _moving = true;

        while(_moving)
        {
            Vector3 startPos = transform.position;
            yield return new WaitForSeconds(1);
            Vector3 endPos = transform.position;
            Vector3 sub = endPos - startPos;

            if (Mathf.Abs(sub.x) < SUB_VAL && Mathf.Abs(sub.z) < SUB_VAL)
            {
                _moving = false;
            }
        }

        HandleMoveEnd();
    }

    private void HandleMoveEnd()
    {
        _cinemachineTransposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
    }
}
