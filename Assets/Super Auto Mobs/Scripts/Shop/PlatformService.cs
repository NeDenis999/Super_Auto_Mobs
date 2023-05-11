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

                            //if (!TryMoveMobsLeft(shopPlatform, numberSelectedPlatform))
                            //{
                                TryMoveMobsRight(shopPlatform, numberSelectedPlatform);
                            //}
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

        private bool TryMoveMobsRight(ShopPlatform shopPlatform, int numberSelectedPlatform)
        {
            for (int i = numberSelectedPlatform; i < _commandPetPlatforms.Count - 1; i++)
            {
                print(1);

                if (_commandPetPlatforms[i].IsEntity)
                {
                    print(2);
                    var isCanMoved = false;

                    for (int j = _commandPetPlatforms.Count - 1; j >= numberSelectedPlatform; j--)
                    {
                        if (!_commandPetPlatforms[j].IsEntity)
                        {
                            isCanMoved = true;
                            break;
                        }
                    }

                    if (!isCanMoved)
                        break;

                    for (int j = _commandPetPlatforms.Count - 2; j >= numberSelectedPlatform; j--)
                    {
                        print(3);
                        var leftPlatform = _commandPetPlatforms[j];
                        var rightPlatform = _commandPetPlatforms[j + 1];

                        if (!(leftPlatform.IsEntity && !rightPlatform.IsEntity))
                        {
                            continue;
                        }

                        print(4);
                        (leftPlatform.Entity, rightPlatform.Entity) = (rightPlatform.Entity, leftPlatform.Entity);

                        if (leftPlatform.IsEntity)
                            leftPlatform.ResetPosition();

                        if (rightPlatform.IsEntity)
                            rightPlatform.ResetPosition();

                        if (!shopPlatform.IsEntity)
                        {
                            break;
                        }

                        print(5);
                    }

                    return true;
                    break;
                }
            }

            return false;
        }
        
        private bool TryMoveMobsLeft(ShopPlatform shopPlatform, int numberSelectedPlatform)
        {
            print(0);
            
            for (int i = _commandPetPlatforms.Count - 1; i >= numberSelectedPlatform; i--)
            {
                print(1);

                if (_commandPetPlatforms[i].IsEntity)
                {
                    print(2);
                    var isCanMoved = false;

                    for (int j = numberSelectedPlatform - 1; j > 0; j--)
                    {
                        if (!_commandPetPlatforms[j].IsEntity)
                        {
                            isCanMoved = true;
                            break;
                        }
                    }

                    print("isCanMoved: " + isCanMoved);
                    
                    if (!isCanMoved)
                        break;

                    for (int j = numberSelectedPlatform; j > 1; j--)
                    {
                        print(3);
                        var leftPlatform = _commandPetPlatforms[j-1];
                        var rightPlatform = _commandPetPlatforms[j];

                        if (!(!leftPlatform.IsEntity && rightPlatform.IsEntity))
                        {
                            continue;
                        }

                        print(4);
                        (leftPlatform.Entity, rightPlatform.Entity) = (rightPlatform.Entity, leftPlatform.Entity);

                        if (leftPlatform.IsEntity)
                            leftPlatform.ResetPosition();

                        if (rightPlatform.IsEntity)
                            rightPlatform.ResetPosition();

                        if (!shopPlatform.IsEntity)
                        {
                            break;
                        }

                        print(5);
                    }

                    return true;
                    break;
                }
            }

            return false;
        }

        private bool IsCoincidencePlatform(ShopCommandMobPlatform shopPlatform)
        {
            if (shopPlatform == _shopPlatformSelected)
                return true;

            return false;
        }
    }
}