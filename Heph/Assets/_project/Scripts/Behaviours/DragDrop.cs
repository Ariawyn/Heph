using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Card;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Heph.Scripts.Behaviours
{
    public class DragDrop : MonoBehaviour
    {
        private BattleSystem _battleSystemRef;
        private bool _isDragging;
        private Vector2 _startPosition;

        private bool _isOverDropZone;
        private GameObject _dropZone;
        private bool _isOverHandZone;
        private GameObject _handZone;
        private bool _hasChangedParent;

        private void Start()
        {
            _battleSystemRef = FindObjectOfType<BattleSystem>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isDragging)
            {
                // For some reason this first code snippet (commented out) didnt work and just set all the cards to the bottom left of the screen
                // SO i deleted the mouse position from player inputs, might re-add at a later date
                //transform.position = _playerInputs.Player.MousePosition.ReadValue<Vector3>();
                transform.position = Mouse.current.position.ReadValue();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // TODO: We can simplify this probably
            switch (other.gameObject.tag)
            {
                case "CardDropZone":
                    _isOverDropZone = true;
                    _dropZone = other.gameObject;
                    break;
                case "PlayerHandZone":
                    _isOverHandZone = true;
                    _handZone = other.gameObject;
                    break;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            switch (other.gameObject.tag)
            {
                case "CardDropZone":
                    _isOverDropZone = false;
                    _dropZone = null;
                    break;
                case "PlayerHandZone":
                    _isOverHandZone = false;
                    _handZone = null;
                    break;
            }
        }

        public void StartDrag()
        {
            _startPosition = transform.position;
            _isDragging = true;
        }

        public void EndDrag()
        {
            var card = gameObject.GetComponent<CardDisplay>().card;
            _isDragging = false;
            if (_isOverDropZone)
            {
                var result = _battleSystemRef.player.QueueCard(card);
                CombatEventsManager.Instance.OnPlayerQueueCardAction(card);
                if (result)
                {
                    Debug.Log("Successful queue");
                    transform.SetParent(_dropZone.transform, false);
                    _hasChangedParent = true;
                }
                else
                {
                    Debug.Log("Could not queue");
                    transform.position = _startPosition;
                }
            }
            else if(_isOverHandZone)
            {
                if (_hasChangedParent)
                {
                    transform.SetParent(_handZone.transform, false);
                    
                    _hasChangedParent = false;
                }
                transform.position = _startPosition;
            }
            else
            {
                transform.position = _startPosition;
            }
        }
    }
}
