using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace io.choy.MirrorExample
{
    public class OfflineSceneManagerScript : NetworkBehaviour
    {
        // The address you will be connecting to
        [SerializeField] private GameObject m_addressInputField;

        // The notification panel
        [SerializeField] private GameObject m_notification;
        private bool m_coroutineRunning = false;

        // If we're already attempting a connection
        private bool m_isConnecting = false;

        public void SetNotification(string message, int waitSeconds = 10)
        {
            if (m_notification == null) return;

            m_notification.transform.GetChild(0).GetComponent<Text>().text = message;
            m_notification.SetActive(true);

            StartCoroutine(HideNotification(waitSeconds));
        }

        public void OnConnectButtonClick()
        {
            Debug.Log("Current network address to connect to is: " + NetworkManager.singleton.networkAddress);
            string address = (string.IsNullOrEmpty(m_addressInputField.GetComponent<Text>().text)) ? "localhost" : m_addressInputField.GetComponent<Text>().text;

            StartCoroutine(Connect(address));
        }

        public void OnHostButtonClick()
        {
            NetworkManager.singleton.StartHost();
        }

        public void OnQuitButtonClick()
        {
            Application.Quit();
        }

        IEnumerator HideNotification(int waitSeconds)
        {
            if (!m_coroutineRunning)
            {
                m_coroutineRunning = true;

                yield return new WaitForSeconds(waitSeconds);

                m_notification.SetActive(false);
                m_coroutineRunning = false;
            }
        }

        IEnumerator Connect(string address)
        {
            if (m_isConnecting) yield return null;

            m_isConnecting = true;

            NetworkManager.singleton.networkAddress = address;
            NetworkManager.singleton.StartClient();

            for (int i = 0; i < 10; i++)
            {
                string message = "Connecting to " + address;
                for (int j = 0; j < i % 3; j++)
                    message += ".";
                SetNotification(message);
                yield return new WaitForSeconds(1);
            }

            m_isConnecting = false;
            SetNotification("Failed to connect to the server.");
        }
    }
}