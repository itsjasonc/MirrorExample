using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace io.choy.MirrorExample
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerScript : NetworkBehaviour
    {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private Camera eyeCamera;

        [SerializeField] private Transform handTransform;

        private Camera mainCamera;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                mainCamera = Camera.main;
                if (mainCamera != null)
                    mainCamera.gameObject.SetActive(false);
            }
        }

        [Client]
        private void Update()
        {
            if (!hasAuthority) return;

            float t_mouseX = Input.GetAxisRaw("Mouse Y");
            float t_mouseY = Input.GetAxisRaw("Mouse X");

            CmdSendRotation(t_mouseY);
            CmdSendCameraRotation(t_mouseX);

            float t_horizontalInput = Input.GetAxisRaw("Horizontal");
            float t_verticalInput = Input.GetAxisRaw("Vertical");
            CmdMove(t_horizontalInput, t_verticalInput);

            handTransform.rotation = eyeCamera.transform.rotation;
        }

        [Command]
        private void CmdMove(float t_horiz, float t_vert)
        {
            RpcMove(t_horiz, t_vert);
        }

        [ClientRpc]
        private void RpcMove(float t_horiz, float t_vert)
        {
            Vector3 t_horizontalVelocity = transform.right * t_horiz;
            Vector3 t_verticalVelocity = transform.forward * t_vert;
            Vector3 velocity = (t_horizontalVelocity + t_verticalVelocity).normalized * walkSpeed;
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }

        [Command]
        private void CmdSendRotation(float t_offset)
        {
            RpcRotate(t_offset);
        }

        [ClientRpc]
        private void RpcRotate(float t_offset)
        {
            Vector3 t_rotation = new Vector3(0f, t_offset, 0f) * mouseSensitivity;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(t_rotation));
        }

        [Command]
        private void CmdSendCameraRotation(float t_offset)
        {
            RpcCameraRotate(t_offset);
        }

        [ClientRpc]
        private void RpcCameraRotate(float t_offset)
        {
            Vector3 t_rotation = new Vector3(t_offset, 0f, 0f) * mouseSensitivity;
            eyeCamera.transform.Rotate(-t_rotation);
        }
    }
}