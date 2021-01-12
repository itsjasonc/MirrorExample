using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace io.choy.MirrorExample
{
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField] Behaviour[] componentsToDisable;

        // Start is called before the first frame update
        void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
            }
            else
            {

            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void DisableComponents()
        {
            foreach (var component in componentsToDisable)
            {
                component.enabled = false;
            }
        }

        private void OnDisable()
        {

        }
    }
}