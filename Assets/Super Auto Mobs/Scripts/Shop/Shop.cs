using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Shop : ShopService
    {
        public override event Action OnSelectCommandPlatform;
        public override event Action OnUnselectCommandPlatform;
        public override event Action<PlatformServiceState> OnUpdateState;
        public event Action<Buff, Mob> OnBuyBuff;
        
        private bool _isDisableBattleButton;
        
        [SerializeField]
        private GameObject _shop;
        
        [SerializeField]
        private List<ShopCommandMobPlatform> _commandPetPlatforms = new();

        [SerializeField]
        private Trajectory _trajectory;

        [SerializeField]
        private InfoMobScreen _infoMobScreen;
        
        [SerializeField]
        private Button _battleButton, _rollButton;
        
        [SerializeField]
        private float _speedMove;

        [SerializeField]
        private Transform _commandPlatformPoint;

        [SerializeField]
        private Transform _shopPlatformPoint;
        
        [SerializeField]
        private Transform _buffPlatformPoint;
        
        private PlatformServiceState _platformServiceState
        {
            get => _currentPlatformServiceState;
            set
            {
                OnUpdateState?.Invoke(value);
                _currentPlatformServiceState = value;
                BattleButtonUpdate();
            }
        }

        private PlatformServiceState _currentPlatformServiceState = PlatformServiceState.NoChoosePlatform;
        private ShopPlatform _shopPlatformSelected;
        private Vector2 _offsetMouseMob;
        private ShopPlatform _shopPlatformLocalSelected;
        private SessionProgressService _sessionProgressService;
        private AssetProviderService _assetProviderService;
        private ShopTradeService _shopTradeService;
        private MobFactoryService _mobFactoryService;

        public override ShopPlatform ShopPlatformSelected => _shopPlatformSelected;
        public override List<ShopCommandMobPlatform> CommandPlatforms => _commandPetPlatforms;
        private ShopUpdaterService _shopUpdaterService;
        private Game _game;
        private bool _isOpen;
        private BackgroundService _backgroundService;
        private List<Discount> _discounts = new();
        private SoundsService _soundsService;
        public GameObject ShopScreen => _shop;

        [Inject]
        private void Construct(SessionProgressService sessionProgressService, MobFactoryService mobFactoryService, 
            AssetProviderService assetProviderService, ShopTradeService shopTradeService, ShopUpdaterService shopUpdaterService,
            Game game, SoundsService soundsService, BattleService battleService, BackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
            _mobFactoryService = mobFactoryService;
            _sessionProgressService = sessionProgressService;
            _assetProviderService = assetProviderService;
            _shopTradeService = shopTradeService;
            _shopUpdaterService = shopUpdaterService;
            _game = game;
            _soundsService = soundsService;
        }

        private void Update()
        {
            if (_game.CurrentGameState != GameState.Shop || !_shop.activeSelf)
                return;

            PlatformServiceUpdate();
            PlatformsPositionUpdate();
        }

        public override void EnableBattleButton()
        {
            _isDisableBattleButton = false;
            _battleButton.gameObject.SetActive(true);
            _sessionProgressService.IsDisableBattleButton = false;
        }
        
        private void BattleButtonUpdate()
        {
            bool isMob = false;

            if (!_isDisableBattleButton)
            {
                foreach (var commandPetPlatform in _commandPetPlatforms)
                {
                    if (commandPetPlatform.Mob)
                        isMob = true;
                }
            }

            _battleButton.gameObject.SetActive(isMob && _sessionProgressService.IsBattle);
        }

        public override void Open()
        {
            if (_isOpen)
                return;
            
            _isOpen = true;
            _shop.SetActive(true);
            _discounts = new List<Discount>();
            _shopUpdaterService.UpdateShop();
            CreatePlatformMobs();
            BattleButtonUpdate();
            _isDisableBattleButton = _sessionProgressService.CurrentWorld.IsDisableBattleButton;
            IsDisableSellButton = _sessionProgressService.CurrentWorld.IsDisableSellButton;
            _rollButton.gameObject.SetActive(!_sessionProgressService.CurrentWorld.IsDisableRollButton);
            
            _commandPlatformPoint.position = _commandPlatformPoint.position
                .SetY(_backgroundService.Location.CommandSpawnPoint.position.y);
            
            _shopPlatformPoint.position = _shopPlatformPoint.position
                .SetY(_backgroundService.Location.ShopSpawnPoint.position.y);
            
            _buffPlatformPoint.position = _buffPlatformPoint.position
                .SetY(_backgroundService.Location.ShopSpawnPoint.position.y);
            
            _soundsService.PlayShop();
        }

        public override void Close()
        {
            if (!_shop.activeSelf)
                return;

            if (_isOpen)
            {
                _isOpen = false;
                _sessionProgressService.MyCommandMobsData = new List<MobData>();

                foreach (var command in _commandPetPlatforms)
                {
                    if (command.IsEntity)
                    {
                        _sessionProgressService.MyCommandMobsData.Add(command.Mob.MobData);
                        print(command.Mob.MobData.MobEnum);
                    }
                }
            }

            _shop.SetActive(false);
            DestroyPlatformMobs();
        }

        public override Mob SpawnMob(MobData mobData)
        {
            throw new NotImplementedException();
        }

        public override Buff SpawnBuff(BuffData buffData)
        {
            throw new NotImplementedException();
        }

        private void CreatePlatformMobs()
        {
            for (int i = 0; i < _sessionProgressService.MyCommandMobsData.Count; i++)
            {
                var mobData = _assetProviderService
                    .GetMobInfo(_sessionProgressService.MyCommandMobsData[i].MobEnum);
                
                _mobFactoryService.CreateMobInPlatform(mobData.Prefab, _commandPetPlatforms[i], mobData.mobDefaultData,
                    _sessionProgressService.MyCommandMobsData[i]);
            }
        }

        public override void DestroyPlatformMobs()
        {
            for (int i = 0; i < _commandPetPlatforms.Count; i++)
            {
                if (_commandPetPlatforms[i].IsEntity)
                    Destroy(_commandPetPlatforms[i].Mob.gameObject);
            }
        }

        public override void AddDiscount(Discount discount)
        {
            _discounts.Add(discount);
        }

        private void UpdateDiscounts()
        {
            
        }
        
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

        public override void DestroySelectEntity()
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

        public void PlatformServiceUpdate()
        {
            if (!IsInteractive)
                return;

            var shopPlatform = Raycast.GetRayColliderObject<ShopPlatform>();
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            switch (_platformServiceState)
            {
                case PlatformServiceState.NoChoosePlatform:
                    if (!shopPlatform || !shopPlatform.IsEntity)
                    {
                        _infoMobScreen.Close();
                        break;  
                    }

                    if (shopPlatform.TrySelect())
                    {
                        _infoMobScreen.Open(shopPlatform.Entity);
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
                            _infoMobScreen.Close();
                            
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
                        _infoMobScreen.Close();
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
                            _infoMobScreen.Close();
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
                                        if (_shopTradeService.TryBuy(PurchaseEnum.Mob))
                                        {
                                            Buy(PurchaseEnum.Mob, shopPlatform);
                                        }
                                    }
                                }
                            }
                            else if (_shopPlatformSelected is ShopBuffPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform && shopPlatform.IsEntity)
                                {
                                    if (_shopTradeService.TryBuy(PurchaseEnum.Buff))
                                    {
                                        Buy(PurchaseEnum.Buff, shopPlatform);
                                    }
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
                                        if (_shopTradeService.TryBuy(PurchaseEnum.Mob))
                                        {
                                            Buy(PurchaseEnum.Mob, shopPlatform);
                                        }
                                    }
                                }
                            }
                            else if (_shopPlatformSelected is ShopBuffPlatform)
                            {
                                if (shopPlatform is ShopCommandMobPlatform && shopPlatform.IsEntity)
                                {
                                    if (_shopTradeService.TryBuy(PurchaseEnum.Buff))
                                    {
                                        Buy(PurchaseEnum.Buff, shopPlatform);
                                    }
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
                        
                        _platformServiceState = PlatformServiceState.NoChoosePlatform;
                        _shopPlatformSelected = null;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Buy(PurchaseEnum purchaseEnum, ShopPlatform shopPlatform)
        {
            if (purchaseEnum == PurchaseEnum.Mob)
            {
                shopPlatform.Entity = _shopPlatformSelected.Entity;

                var mob = (Mob)_shopPlatformSelected.Entity;

                if (mob.MobDefaultData.IsSingle)
                {
                    _sessionProgressService.MobsUnlocked.Remove(mob.MobData.MobEnum);
                }
                
                if (mob.Perk.TriggeringSituation == TriggeringSituation.Buy)
                {
                    StartCoroutine(mob.Perk.Activate());
                }

                _shopPlatformSelected.Entity = null;
            }
            else if (purchaseEnum == PurchaseEnum.Buff)
            {
                var mob = (Mob)shopPlatform.Entity;

                var buff = (Buff)_shopPlatformSelected.Entity;
                
                if (buff.BuffData.IsSingle)
                {
                    _sessionProgressService.BuffsUnlocked.Remove(buff.BuffData.BuffEnum);
                }

                OnBuyBuff?.Invoke(buff, mob);

                Action OnEndMoveAnimation = () =>
                {
                    foreach (var platform in _commandPetPlatforms)
                    {
                        if (platform.IsEntity)
                        {
                            if (platform.Mob.Perk.TriggeringSituation == TriggeringSituation.EatOther)
                            {
                                StartCoroutine(platform.Mob.Perk.Activate());
                            }
                        }
                    }
                };

                StartCoroutine(buff.ToMoveTrajectory(
                    _trajectory.GetTrajectory(
                        _shopPlatformSelected.Entity.transform.position, 
                        shopPlatform.transform.position), mob, OnEndMoveAnimation));
                _shopPlatformSelected.Entity = null;
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
    }
}