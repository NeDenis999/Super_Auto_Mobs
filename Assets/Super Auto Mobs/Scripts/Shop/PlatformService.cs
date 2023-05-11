using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Super_Auto_Mobs
{
    public class PlatformService : MonoBehaviour
    {
        [SerializeField]
        private List<ShopCommandMobPlatform> _commandPetPlatforms;

        [SerializeField]
        private Trajectory _trajectory;
        
        [SerializeField, ReadOnly]
        private PlatformServiceState _platformServiceState = PlatformServiceState.NoChoosePlatform;

        private ShopPlatform _shopPlatformSelected;
        private Vector2 _offsetMouseMob;
        private ShopPlatform _shopPlatformLocalSelected;

        private ShopPlatform GetRayPlatformPet()
        {
            var pointer = Raycast.PointerRaycast(Input.mousePosition); 
            
            if (pointer == null)
                return null;

            pointer.TryGetComponent(out ShopPlatform platformPet);
            return platformPet;
        }

        private void Update()
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
                        _shopPlatformSelected.Entity.Unselect();
                        
                        if (Input.GetMouseButton(0))
                        {
                            _platformServiceState = PlatformServiceState.MoveChoosePlatform;

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
                            //_shopPlatformSelected.TryUnselect();
                            
                            foreach (var platform in _commandPetPlatforms)
                            {
                                if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform
                                    || platform.IsEntity)
                                {
                                    platform.TryArrowOn();
                                }
                            }
                            
                            _platformServiceState = PlatformServiceState.ChoosePlatform;   
                        }
                    }

                    break;
                case PlatformServiceState.ChoosePlatform:
                    if (_shopPlatformLocalSelected)
                    {
                        _shopPlatformLocalSelected.TryUnselect();
                    }

                    if (shopPlatform && shopPlatform is ShopCommandMobPlatform)
                    {
                        if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform 
                            || shopPlatform && shopPlatform.IsEntity)
                        {
                            _shopPlatformLocalSelected = shopPlatform;
                            _shopPlatformLocalSelected.TrySelect();
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
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
                                    Destroy(_shopPlatformSelected.Entity.gameObject);
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

                        _shopPlatformSelected = null;
                        _platformServiceState = PlatformServiceState.NoChoosePlatform;
                        break;
                    }
                    
                    break;
                case PlatformServiceState.MoveChoosePlatform:
                    _shopPlatformSelected.MoveEntity(mousePosition + (Vector3)_offsetMouseMob);

                    if (_shopPlatformSelected is ShopCommandMobPlatform or ShopMobPlatform)
                    {
                        if (shopPlatform is ShopCommandMobPlatform && shopPlatform.IsEntity)
                        {
                            var numberSelectedPlatform =
                                _commandPetPlatforms.FindIndex(platform => platform == shopPlatform);
                            
                            var numberCurrentSelectedPlatform =
                                _commandPetPlatforms.FindIndex(platform => platform == _shopPlatformSelected);

                            if (numberSelectedPlatform > numberCurrentSelectedPlatform)
                            {
                                if (!TryMoveMobsRight(numberSelectedPlatform, numberCurrentSelectedPlatform))
                                    TryMoveMobsLeft(numberSelectedPlatform, numberCurrentSelectedPlatform);
                            }
                            else
                            {
                                if (!TryMoveMobsLeft(numberSelectedPlatform, numberCurrentSelectedPlatform))
                                    TryMoveMobsRight(numberSelectedPlatform, numberCurrentSelectedPlatform);
                            }
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
                                    Destroy(_shopPlatformSelected.Entity.gameObject);
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
                        
                        _shopPlatformSelected = null;
                        _platformServiceState = PlatformServiceState.NoChoosePlatform;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryMoveMobsRight(int numberSelectedPlatform, int numberCurrentSelectedPlatform)
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

            for (var i = numberCurrentSelectedPlatform; i > numberSelectedPlatform; i--)
            {
                (_commandPetPlatforms[i].Mob, _commandPetPlatforms[i-1].Mob) = (_commandPetPlatforms[i-1].Mob, _commandPetPlatforms[i].Mob);
                
                if (_commandPetPlatforms[i].IsEntity)
                    _commandPetPlatforms[i].ResetPosition();
            }

            if (_shopPlatformSelected is ShopCommandMobPlatform)
                _shopPlatformSelected = _commandPetPlatforms[numberSelectedPlatform];
            
            return true;
        }

        private bool TryMoveMobsLeft(int numberSelectedPlatform, int numberCurrentSelectedPlatform)
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

            for (var i = numberCurrentSelectedPlatform; i < numberSelectedPlatform; i++)
            {
                (_commandPetPlatforms[i].Mob, _commandPetPlatforms[i+1].Mob) = (_commandPetPlatforms[i+1].Mob, _commandPetPlatforms[i].Mob);
                
                if (_commandPetPlatforms[i].IsEntity)
                    _commandPetPlatforms[i].ResetPosition();
            }

            if (_shopPlatformSelected is ShopCommandMobPlatform)
                _shopPlatformSelected = _commandPetPlatforms[numberSelectedPlatform];
            
            return true;
        }

        private bool IsCoincidencePlatform(ShopCommandMobPlatform shopPlatform)
        {
            if (shopPlatform == _shopPlatformSelected)
                return true;

            return false;
        }
    }
}