using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heph
{
    public class BillboardBehaviour : MonoBehaviour
    {
        public Camera mainCamera;

        private void Update()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
