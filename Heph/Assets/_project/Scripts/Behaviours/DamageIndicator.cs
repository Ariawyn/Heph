using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Heph.Scripts.Behaviours
{
    public class DamageIndicator : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public float lifetime = 0.6f;
        public float minDistance = 2f;
        public float maxDistance = 3f;

        private Vector3 initialPosition;
        private Vector3 targetPosition;
        private float timer;
        
        // Start is called before the first frame update
        void Start()
        {
            var position = transform.position;
            transform.LookAt(2 * position - Camera.main.transform.position);

            var direction = Random.rotation.eulerAngles.z;
            initialPosition = position;
            var distance = Random.Range(minDistance, maxDistance);
            targetPosition = initialPosition + (Quaternion.Euler(0, 0, direction) * new Vector3(distance, distance, 0f));
            transform.localScale = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            var fraction = lifetime / 2f;
            
            if(timer > lifetime) Destroy(gameObject);
            else if (timer > fraction)
                text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));


            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, Mathf.Sin(timer / lifetime));
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
        }

        public void SetDamageText(int damage)
        {
            text.text = damage.ToString();
        }
    }
}
