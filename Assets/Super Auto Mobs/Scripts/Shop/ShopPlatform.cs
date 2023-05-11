using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopPlatform : MonoBehaviour
    {
        public event Action OnEntityUpdated;

        protected Entity _entity;
        
        private SpriteRenderer _arrow;
        private GameObject _selectionFrame;
        private Transform _spawnPoint;
        private bool _isSelected;
        private bool _isArrow;
        private bool _isMove;

        public bool IsEntity => _entity != null;

        public Entity Entity 
        { 
            get => _entity;
            set
            {
                _entity = value;
                OnEntityUpdated?.Invoke();
            }
        }

        private void Awake()
        {
            _arrow = GetComponentInChildren<Arrow>(true).GetComponentInChildren<SpriteRenderer>(true);
            _selectionFrame = GetComponentInChildren<SelectionFrame>(true).gameObject;
            _spawnPoint = GetComponentInChildren<SpawnPoint>().transform;

            _entity = _spawnPoint.GetComponentInChildren<Entity>(false);
        }

        [ContextMenu("TrySelect")]
        public bool TrySelect()
        {
            if (!_isSelected)
            {
                Select();
                return true;
            }

            return false;
        }

        [ContextMenu("TryUnselect")]
        public bool TryUnselect()
        {
            if (_isSelected)
            {
                Unselect();
                return true;
            }

            return false;
        }

        protected virtual void Select()
        {
            _isSelected = true;
            _arrow.color = Color.black;
            _selectionFrame.SetActive(true);
        }

        protected virtual void Unselect()
        {
            _isSelected = false;
            _arrow.color = Color.white;
            _selectionFrame.SetActive(false);
        }

        public bool TryArrowOn()
        {
            if (!_isArrow)
            {
                ArrowOn();
                return true;
            }
            
            return false;
        }

        public bool TryArrowOff()
        {
            if (_isArrow)
            {
                ArrowOff();
                return true;
            }
            
            return false;
        }

        protected virtual void ArrowOn()
        {
            _isArrow = true;
            _arrow.gameObject.SetActive(true);
        }
        
        protected virtual void ArrowOff()
        {
            _isArrow = false;
            _arrow.gameObject.SetActive(false);
        }

        protected virtual void StartMoveEntity()
        {

        }

        public virtual void MoveEntity(Vector2 target)
        {
            if (!_isMove)
            {
                _isMove = true;
                StartMoveEntity();
            }
            
            _entity.transform.position = _entity.transform.position.SetX(target.x).SetY(target.y);
        }

        public virtual void ResetPosition()
        {
            _isMove = false;
            _entity.transform.position = _entity.transform.position.SetX(_spawnPoint.position.x).SetY(_spawnPoint.position.y);
        }
    }
}