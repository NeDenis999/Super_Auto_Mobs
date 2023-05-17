using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopPlatformService : MonoBehaviour
    {
        public event Action OnSelectCommandPlatform;
        public event Action OnUnselectCommandPlatform;
        
        [SerializeField]
        private List<ShopCommandMobPlatform> _commandPetPlatforms;

        [SerializeField]
        private Trajectory _trajectory;
        
        [SerializeField, ReadOnly]
        private PlatformServiceState _platformServiceState = PlatformServiceState.NoChoosePlatform;

        [SerializeField]
        private float _speedMove;
        
        private ShopPlatform _shopPlatformSelected;
        private Vector2 _offsetMouseMob;
        private ShopPlatform _shopPlatformLocalSelected;

        public void PlatformsPositionUpdate()
        {
            foreach (var commandMobPlatform in _commandPetPlatforms)
            {
                if (commandMobPlatform.IsEntity && commandMobPlatform != _shopPlatformSelected)
                {
                    commandMobPlatform.Entity.transform.position =
                        Vector3.MoveTowards(
                            commandMobPlatform.Entity.transform.position,
                            commandMobPlatform.SpawnPoint.transform.position,
                            Time.deltaTime * _speedMove);
                }
            }
        }

        public void PlatformServiceUpdate()
        {
            var shopPlatform = Raycast.GetRayColliderObject<ShopPlatform>();
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            switch (_platformServiceState)
            {
                case PlatformServiceState.NoChoosePlatform:
                    if (!shopPlatform || !shopPlatform.IsEntity)
                    {
                        break;  
                    }

                    if (shopPlatform.TrySelect())
                    {
                        shopPlatform.Entity.Select();
                        _shopPlatformSelected = shopPlatform;
                        _platformServiceState = PlatformServiceState.BeforeChoosePlatform;
                    }

                    break;
                case PlatformServiceState.BeforeChoosePlatform:
                    if (!shopPlatform)
                    {
                        _shopPlatformSelected.TryUnselect();

                        if (Input.GetMouseButton(0))
                        {
                            _platformServiceState = PlatformServiceState.MoveChoosePlatform;
                            
                            if (_shopPlatformSelected is ShopCommandMobPlatform)
                                OnSelectCommandPlatform?.Invoke();
                            
                            if (_shopPlatformSelected is ShopBuffPlatform)
                            {
                                _trajectory.Show(_shopPlatformSelected.Entity.transform.position);
                            }

                            foreach (var platform in _commandPetPlatforms)
                            {
                                if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform
                                    || platform.IsEntity)
                                {
                                    platform.TryArrowOn();
                                }
                            }
                            
                            break;
                        }
                        
                        _shopPlatformSelected.Entity.Unselect();
                        _shopPlatformSelected = null;
                        _platformServiceState = PlatformServiceState.NoChoosePlatform;
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            _offsetMouseMob = _shopPlatformSelected.transform.position - mousePosition;
                        }
                        
                        if (Input.GetMouseButtonUp(0))
                        {
                            foreach (var platform in _commandPetPlatforms)
                            {
                                if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform
                                    || platform.IsEntity)
                                {
                                    platform.TryArrowOn();
                                }
                            }
                            
                            if (_shopPlatformSelected is ShopCommandMobPlatform)
                                OnSelectCommandPlatform?.Invoke();
                            
                            _shopPlatformSelected.Entity.UpScaleSmoothly();
                            _platformServiceState = PlatformServiceState.ChoosePlatform;
                        }
                    }

                    break;
                case PlatformServiceState.ChoosePlatform:
                    if (_shopPlatformLocalSelected && _shopPlatformLocalSelected != _shopPlatformSelected)
                    {
                        _shopPlatformLocalSelected.TryUnselect();
                    }

                    if (shopPlatform && shopPlatform is ShopCommandMobPlatform)
                    {
                        if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform 
                            || shopPlatform && shopPlatform.IsEntity && _shopPlatformLocalSelected != _shopPlatformSelected)
                        {
                            _shopPlatformLocalSelected = shopPlatform;
                            _shopPlatformLocalSelected.TrySelect();
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        _shopPlatformSelected.Entity.DownScaleSmoothly();
                        
                        foreach (var platform in _commandPetPlatforms)
                        {
                            platform.TryArrowOff();
                        }

                        if (shopPlatform)
                        {
                            if (_shopPlatformSelected.TryUnselect())
                            {
                                _shopPlatformSelected.Entity.Unselect();
                            }
                            
                            if (_shopPlatformSelected is ShopCommandMobPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform)
                                {
                                    MoveMobs(shopPlatform);
                                }
                            }
                            else if (_shopPlatformSelected is ShopMobPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform)
                                {
                                    if (!shopPlatform.IsEntity && _shopPlatformSelected.IsEntity)
                                    {
                                        shopPlatform.Entity = _shopPlatformSelected.Entity;
                                        _shopPlatformSelected.Entity = null;
                                    }
                                }
                            }
                            else if (_shopPlatformSelected is ShopBuffPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform && shopPlatform.IsEntity)
                                {
                                    var _buff = (Buff)_shopPlatformSelected.Entity;
                                    StartCoroutine(_buff.ToMoveTrajectory(
                                        _trajectory.GetTrajectory(
                                            _shopPlatformSelected.Entity.transform.position, 
                                            shopPlatform.transform.position)));
                                    _shopPlatformSelected.Entity = null;
                                }
                            }

                            if (shopPlatform.IsEntity)
                                shopPlatform.ResetPosition();

                            if (_shopPlatformLocalSelected)
                            {
                                _shopPlatformLocalSelected.TryUnselect();
                                _shopPlatformLocalSelected = null;
                            }
                        }
                        
                        if (_shopPlatformSelected.IsEntity)
                        {
                            _shopPlatformSelected.ResetPosition();
                            _shopPlatformSelected.Entity.Unselect();
                        }

                        if (_shopPlatformSelected is ShopCommandMobPlatform)
                            OnUnselectCommandPlatform?.Invoke();
                        
                        _shopPlatformSelected.TryUnselect();
                        _shopPlatformSelected = null;
                        _platformServiceState = PlatformServiceState.NoChoosePlatform;
                        break;
                    }
                    
                    break;
                case PlatformServiceState.MoveChoosePlatform:
                    _shopPlatformSelected.TryMoveEntity(mousePosition + (Vector3)_offsetMouseMob);

                    if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform)
                    {
                        if (shopPlatform is ShopCommandMobPlatform && shopPlatform.IsEntity)
                        {
                            MoveMobs(shopPlatform);
                        }
                    }
                    
                    if (_shopPlatformSelected is ShopBuffPlatform)
                    {
                        if (shopPlatform && shopPlatform.IsEntity && shopPlatform is ShopCommandMobPlatform)
                        {
                            _trajectory.Move(shopPlatform.Entity.transform.position);   
                        }
                        else
                        {
                            _trajectory.Move(mousePosition);   
                        }
                    }
                    
                    if (_shopPlatformLocalSelected)
                    {
                        _shopPlatformLocalSelected.TryUnselect();
                        
                        if (_shopPlatformSelected is ShopBuffPlatform)
                        {
                            _trajectory.TryUnselect();
                        }
                    }

                    if (shopPlatform && shopPlatform is ShopCommandMobPlatform)
                    {
                        if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform 
                            || shopPlatform && shopPlatform.IsEntity)
                        {
                            _shopPlatformLocalSelected = shopPlatform;
                            _shopPlatformLocalSelected.TrySelect();
                            
                            if (_shopPlatformSelected is ShopBuffPlatform)
                            {
                                _trajectory.TrySelect();
                            }
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        _shopPlatformSelected.ResetPosition();
                        _shopPlatformSelected.TryUnselect();

                        if (shopPlatform)
                        {
                            if (_shopPlatformSelected is ShopCommandMobPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform)
                                {
                                    (_shopPlatformSelected.Entity, shopPlatform.Entity) = (shopPlatform.Entity, _shopPlatformSelected.Entity);
                                }
                            }
                            else if (_shopPlatformSelected is ShopMobPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform)
                                {
                                    if (!shopPlatform.IsEntity && _shopPlatformSelected.IsEntity)
                                    {
                                        shopPlatform.Entity = _shopPlatformSelected.Entity;
                                        _shopPlatformSelected.Entity = null;
                                    }
                                }
                            }
                            else if (_shopPlatformSelected is ShopBuffPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform && shopPlatform.IsEntity)
                                {
                                    var _buff = (Buff)_shopPlatformSelected.Entity;
                                    StartCoroutine(_buff.ToMoveTrajectory(_trajectory.GetTrajectory(
                                        _shopPlatformSelected.Entity.transform.position, shopPlatform.transform.position)));
                                    _shopPlatformSelected.Entity = null;
                                }
                            }

                            if (shopPlatform.IsEntity)
                            {
                                shopPlatform.ResetPosition();
                            }

                            if (_shopPlatformSelected.IsEntity)
                            {
                                _shopPlatformSelected.ResetPosition();
                                _shopPlatformSelected.Entity.Unselect(); 
                            }
                        }

                        foreach (var platform in _commandPetPlatforms)
                        {
                            platform.TryArrowOff();
                        }
                        
                        if (_shopPlatformSelected is ShopBuffPlatform)
                        {
                            _trajectory.Hide();
                        }
                        
                        if (_shopPlatformSelected.IsEntity)
                        {
                            _shopPlatformSelected.Entity.Unselect();
                        }
                        
                        if (_shopPlatformLocalSelected)
                        {
                            _shopPlatformLocalSelected.TryUnselect();
                            _shopPlatformLocalSelected = null;
                        }
                        
                        if (_shopPlatformSelected is ShopCommandMobPlatform)
                            OnUnselectCommandPlatform?.Invoke();
                        
                        _shopPlatformSelected = null;
                        _platformServiceState = PlatformServiceState.NoChoosePlatform;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void MoveMobs(ShopPlatform shopPlatform)
        {
            var numberSelectedPlatform =
                _commandPetPlatforms.FindIndex(platform => platform == shopPlatform);
                            
            var numberCurrentSelectedPlatform =
                _commandPetPlatforms.FindIndex(platform => platform == _shopPlatformSelected);

            if (IsMoveMobsRight(numberSelectedPlatform, numberCurrentSelectedPlatform))
                MoveMobsRight(numberSelectedPlatform, numberCurrentSelectedPlatform);
            else if (IsMoveMobsLeft(numberSelectedPlatform, numberCurrentSelectedPlatform))
                MoveMobsLeft(numberSelectedPlatform, numberCurrentSelectedPlatform);
        }

        private bool IsMoveMobsRight(int numberSelectedPlatform, int numberCurrentSelectedPlatform)
        {
            if (numberCurrentSelectedPlatform == -1)
            {
                for (var i = numberSelectedPlatform + 1; i < _commandPetPlatforms.Count; i++)
                {
                    if (!_commandPetPlatforms[i].IsEntity)
                    {
                        numberCurrentSelectedPlatform = i;
                        break;
                    }
                }
                
                if (numberCurrentSelectedPlatform == -1)
                    return false;
            }
            
            if (numberCurrentSelectedPlatform <= numberSelectedPlatform)
                return false;
            
            return true;
        }

        private bool IsMoveMobsLeft(int numberSelectedPlatform, int numberCurrentSelectedPlatform)
        {
            if (numberCurrentSelectedPlatform == -1)
            {
                for (var i = numberSelectedPlatform; i >= 0; i--)
                {
                    if (!_commandPetPlatforms[i].IsEntity)
                    {
                        numberCurrentSelectedPlatform = i;
                        break;
                    }
                }

                if (numberCurrentSelectedPlatform == -1)
                    return false;
            }
            
            if (numberCurrentSelectedPlatform >= numberSelectedPlatform)
                return false;

            return true;
        }

        private void MoveMobsRight(int numberSelectedPlatform, int numberCurrentSelectedPlatform)
        {
            for (var i = numberCurrentSelectedPlatform; i > numberSelectedPlatform; i--)
            {
                (_commandPetPlatforms[i].Mob, _commandPetPlatforms[i-1].Mob) = (_commandPetPlatforms[i-1].Mob, _commandPetPlatforms[i].Mob);
            }

            if (_shopPlatformSelected is ShopCommandMobPlatform)
                _shopPlatformSelected = _commandPetPlatforms[numberSelectedPlatform];
        }
        
        private void MoveMobsLeft(int numberSelectedPlatform, int numberCurrentSelectedPlatform)
        {
            for (var i = numberCurrentSelectedPlatform; i < numberSelectedPlatform; i++)
            {
                (_commandPetPlatforms[i].Mob, _commandPetPlatforms[i+1].Mob) = (_commandPetPlatforms[i+1].Mob, _commandPetPlatforms[i].Mob);
            }

            if (_shopPlatformSelected is ShopCommandMobPlatform)
                _shopPlatformSelected = _commandPetPlatforms[numberSelectedPlatform];
        }

        public void DestroySelectEntity()
        {
            Destroy(_shopPlatformSelected.Entity.gameObject);
            _shopPlatformSelected.Entity = null;
            _shopPlatformSelected.TryUnselect();
            _shopPlatformSelected = null;
            _platformServiceState = PlatformServiceState.NoChoosePlatform;
            OnUnselectCommandPlatform?.Invoke();
            
            foreach (var platform in _commandPetPlatforms)
            {
                platform.TryArrowOff();
            }
        }
    }
}