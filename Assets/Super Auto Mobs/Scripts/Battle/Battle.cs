using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Battle : BattleService
    {
        private const float Indent = 0.5f;
        private const float JumpHeight = 0.5f;
        private const float DistanceBetweenTeams = -1.5f;

        [SerializeField]
        private GameObject _battle;
        
        [SerializeField]
        private Transform _myCommandSpawnTransform;
        
        [SerializeField]
        private Transform _enemyCommandSpawnTransform;
        
        [SerializeField]
        private TextMeshProUGUI _myCommandText;
        
        [SerializeField]
        private TextMeshProUGUI _vsText;
        
        [SerializeField]
        private TextMeshProUGUI _enemyCommandText;

        [SerializeField]
        private TextMeshProUGUI _myDamageText;
        
        [SerializeField]
        private TextMeshProUGUI _enemyDamageText;

        [SerializeField]
        private ShakeService _shakeService;

        [SerializeField]
        private GameObject _animeSpeedEffect;

        [SerializeField]
        private LevelCompleteScreen _levelCompleteScreen;

        [SerializeField]
        private InfoMobScreen _myInfoMobScreen;
        
        [SerializeField]
        private InfoMobScreen _enemyInfoMobScreen;

        [SerializeField]
        private TextMeshProUGUI _tapText;
        
        [Header("Parameters")]
        [SerializeField]
        private bool _isSkipIntro;
        
        [SerializeField]
        private bool _isSkipBattle;
        
        [SerializeField]
        private float _emergencyLandingDelay = 0.5f;
        
        [SerializeField]
        private float _emergencyEndLandingDelay = 0.5f;
        
        [SerializeField]
        private float _updatePositionDelay = 0.5f;
                
        [SerializeField]
        private float _preparingAttackDelay = 0.5f;
                
        [SerializeField]
        private float _takeDamageDelay = 0.5f;
                
        [SerializeField]
        private float _landingDelay = 0.2f;
                
        [SerializeField]
        private float _approachDelay = 0.2f;
                
        [SerializeField]
        private float _attackPerkDelay = 1f;
                
        [SerializeField]
        private float _takeDamagePerkDelay = 1f;
        
        [SerializeField]
        private float _endGamePerkDelay = 1f;
        
        [SerializeField]
        private float _betweenAttacksDelay = 1f;
                
        [SerializeField]
        private float _damageDelay = 0.1f;
        
        private AssetProviderService _assetProviderService;
        private Camera _camera;
        private SessionProgressService _sessionProgressService;
        private DiContainer _diContainer;
        private bool _endTurn;
        private bool _autoPlay;
        
        private List<Mob> _myCommandMobs = new();
        private List<Mob> _enemyCommandMobs = new();

        public override List<Mob> MyCommandMobs => _myCommandMobs;
        public override List<Mob> EnemyCommandMobs => _enemyCommandMobs;

        [Inject]
        private void Construct(AssetProviderService assetProviderService, SessionProgressService sessionProgressService, DiContainer diContainer)
        {
            _assetProviderService = assetProviderService;
            _sessionProgressService = sessionProgressService;
            _diContainer = diContainer;
        }

        public override void Open()
        {
            _battle.SetActive(true);
        }
        
        public override void Close()
        {
            if (!_battle.activeSelf)
                return;
            
            RemovePets();
            _battle.SetActive(false);
        }
        
        private void SkipUpdatePositionPetsAnimation(List<Mob> commandMobsActive)
        {
            for (int i = 0; i < commandMobsActive.Count; i++)
            {
                commandMobsActive[i].transform.localPosition = commandMobsActive[i].transform.localPosition.SetX(DistanceBetweenTeams * i);
            }
        }

        private void SpawnMobs()
        {
            foreach (var mobData in _sessionProgressService.MyCommandMobsData)
            {
                var mob = SpawnMob(mobData, false);
                mob.OnFaint += Faint;
            }
            
            foreach (var mobData in _sessionProgressService.EnemyCommandMobsData)
            {
                var mob = SpawnMob(mobData, true);
                mob.OnFaint += Faint;
            }
        }

        public override Mob SpawnMob(MobData mobData, bool isEnemy)
        {
            var spawnTransform = isEnemy ? _enemyCommandSpawnTransform : _myCommandSpawnTransform;
            var list = isEnemy ? _enemyCommandMobs : _myCommandMobs;

            var mobInfo = _assetProviderService.GetMobInfo(mobData.MobEnum);
            var mob = Instantiate(mobInfo.Prefab, spawnTransform);
            _diContainer.Inject(mob);
            var perk = mob.GetComponent<Perk>();
            
            if (perk)
                _diContainer.Inject(perk);
            
            mob.Init(mobInfo.mobDefaultData, mobData, isEnemy);
            list.Add(mob);
            return mob;
        }
        
        public override IEnumerator AwaitIntro()
        {
            if (_isSkipIntro)
            {
                SkipIntro();
                yield break;
            }
            
            var startMyPosition = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - Indent;
            var startEnemyPosition = _camera.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, 0, 0)).x + Indent;

            _myCommandText.gameObject.SetActive(true);
            _enemyCommandText.gameObject.SetActive(true);
            _vsText.gameObject.SetActive(true);
            
            _myCommandText.transform.localScale = Vector3.one * 0;        
            _enemyCommandText.transform.localScale = Vector3.one * 0;    
            _vsText.transform.localScale = Vector3.one * 0;         

            LeanTween.scale(_myCommandText.gameObject, Vector3.one, 2).setEaseOutElastic();
            LeanTween.scale(_vsText.gameObject, Vector3.one, 2).setDelay(0.5f).setEaseOutElastic();
            LeanTween.scale(_enemyCommandText.gameObject, Vector3.one, 2).setDelay(1f).setEaseOutElastic();

            for (int i = 0; i < _myCommandMobs.Count; i++)
            {
                _myCommandMobs[i].transform.position = _myCommandMobs[i].transform.position.SetX(startMyPosition);
                _enemyCommandMobs[i].transform.position = _enemyCommandMobs[i].transform.position.SetX(startEnemyPosition);
            }

            for (int i = 0; i < _myCommandMobs.Count; i++)
            {
                LeanTween.moveLocalX(_myCommandMobs[i].gameObject, DistanceBetweenTeams * i, 1 - 0.15f * i)
                    .setOnComplete(() =>
                    {
                        LeanTween.cancel(_myCommandMobs[i].gameObject);
                        LeanTween.moveLocalY(_myCommandMobs[i].gameObject, 0, 0.3f);
                    });
                
                LeanTween.moveLocalX(_enemyCommandMobs[i].gameObject, DistanceBetweenTeams * i, 1- 0.15f * i)
                    .setOnComplete(() =>
                    {
                        LeanTween.cancel(_enemyCommandMobs[i].gameObject);
                        LeanTween.moveLocalY(_enemyCommandMobs[i].gameObject, 0, 0.3f);
                    });
                
                LeanTween.moveLocalY(_myCommandMobs[i].gameObject, JumpHeight, 0.3f).setLoopPingPong().setEaseOutCubic();
                LeanTween.moveLocalY(_enemyCommandMobs[i].gameObject, JumpHeight, 0.3f).setLoopPingPong().setEaseOutCubic();
                yield return new WaitForSeconds(1f);
            }
            
            LeanTween.scale(_myCommandText.gameObject, Vector3.zero, 0.5f).setEaseOutCubic();
            LeanTween.scale(_vsText.gameObject, Vector3.zero,  0.5f).setEaseOutCubic();
            LeanTween.scale(_enemyCommandText.gameObject, Vector3.zero,  0.5f).setEaseOutCubic();
            
            yield return new WaitForSeconds(1);
            
            _animeSpeedEffect.SetActive(false);
            _myCommandText.gameObject.SetActive(false);
            _enemyCommandText.gameObject.SetActive(false);
            _vsText.gameObject.SetActive(false);
        }
        
        public override IEnumerator AwaitProcessBattle()
        {
            _camera = Camera.main;
            SpawnMobs();
            yield return null;
            yield return AwaitIntro();

            if (_isSkipBattle)
            {
                StartCoroutine(AwaitEndBattle());
                yield break;
            }
            
            yield return null;

            foreach (var mob in _myCommandMobs)
            {
                if (mob.Perk.TriggeringSituation == TriggeringSituation.StartBattle)
                {
                    yield return mob.Perk.Activate();
                    yield return new WaitForSeconds(1);
                }
            }
            
            foreach (var mob in _enemyCommandMobs)
            {
                if (mob.Perk.TriggeringSituation == TriggeringSituation.StartBattle)
                {
                    yield return mob.Perk.Activate();
                    yield return new WaitForSeconds(1);
                }
            }

            if (!_autoPlay)
            {
                _tapText.gameObject.SetActive(true);
            }
            
            while (MyActiveMob() && EnemyActiveMob())
            {
                if (!_autoPlay)
                {
                    _myInfoMobScreen.Open(MyActiveMob(), true);
                    _enemyInfoMobScreen.Open(EnemyActiveMob(), true);
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                    _tapText.gameObject.SetActive(false);
                    _myInfoMobScreen.Close();
                    _enemyInfoMobScreen.Close();
                }
                
                StartCoroutine(Attack(false));
                yield return StartCoroutine(Attack(true));
                StartCoroutine(AwaitUpdatePositionPetsAnimation(EnemyCommandMobsActive()));
                yield return AwaitUpdatePositionPetsAnimation(MyCommandMobsActive());
            }

            StartCoroutine(AwaitEndBattle());
        }

        public override IEnumerator Attack(bool isEnemy)
        {
            var activeMob = isEnemy ? EnemyActiveMob() : MyActiveMob();
            var oppositeActiveMob = isEnemy ? MyActiveMob() : EnemyActiveMob();
            var damageText = isEnemy ? _enemyDamageText : _myDamageText;

            yield return AwaitPreparingAttackAnimation(activeMob);
            yield return AwaitApproachAnimation(activeMob, 0.3f);
            
            oppositeActiveMob.TakeDamage(activeMob.CurrentAttack);
            
            if (activeMob.Perk.TriggeringSituation == TriggeringSituation.Attack)
            { 
                yield return activeMob.Perk.Activate();
                yield return new WaitForSeconds(_attackPerkDelay);
            }

            if (oppositeActiveMob.Perk.TriggeringSituation == TriggeringSituation.TakeDamage) 
            {
                yield return oppositeActiveMob.Perk.Activate();
                yield return new WaitForSeconds(_takeDamagePerkDelay);
            }

            yield return new WaitForSeconds(_betweenAttacksDelay);
            
            if (activeMob.IsActive)
            {
                yield return new WaitForSeconds(_damageDelay);
                StartCoroutine(AwaitLandingAnimation(activeMob));
            }
            
            _endTurn = true;
            yield return null;
            _endTurn = false;
        }

        private IEnumerator AwaitEndBattle()
        {
            var gameResult = EndBattleEnum.Faint;
            
            if (MyActiveMob())
            {
                gameResult = EndBattleEnum.Won;

                foreach (var mob in MyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Win)
                    {
                        yield return mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
                
                foreach (var mob in EnemyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Lose)
                    {
                        yield return mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
            }
            else if (EnemyActiveMob())
            {
                gameResult = EndBattleEnum.Lose;
                
                foreach (var mob in EnemyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Win)
                    {
                        yield return mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
                
                foreach (var mob in MyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Lose)
                    {
                        yield return mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
            }
            else
            {
                gameResult = EndBattleEnum.Faint;
                
                foreach (var mob in MyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Faint)
                    {
                        yield return mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
                
                foreach (var mob in EnemyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Faint)
                    {
                        yield return mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
            }
            
            _levelCompleteScreen.Open(gameResult);
        }

        private IEnumerator AwaitApproachAnimation(Mob myActiveMob, float indent)
        {
            LeanTween.moveLocal(myActiveMob.gameObject, new Vector3(indent, JumpHeight), _approachDelay).setEaseOutCubic();;
            yield return new WaitForSeconds(_approachDelay);
        }
        
        private IEnumerator AwaitLandingAnimation(Mob mob)
        {
            LeanTween.moveLocal(mob.gameObject, Vector3.zero, _landingDelay).setEaseInOutSine();
            yield return new WaitForSeconds(_landingDelay);
        }

        private IEnumerator AwaitEmergencyLanding(Mob mob, float endPathPositionX)
        {
            yield return new WaitUntil(() => _endTurn);

            var pathPoints = MathfExtensions.CreateSineWave(mob.transform.position, 
                mob.transform.position.SetX(endPathPositionX), 50, 1, 0.5f);
            
            LeanTween.moveSpline(mob.gameObject, pathPoints, _emergencyLandingDelay).setEaseInSine();
            yield return new WaitForSeconds(_emergencyEndLandingDelay);
        }

        private IEnumerator AwaitTakeDamageAnimation(int damage, TextMeshProUGUI text)
        {
            text.gameObject.SetActive(true);
            text.text = damage.ToString();
            yield return new WaitForSeconds(_takeDamageDelay);
            text.gameObject.SetActive(false);
        }
        
        private void SkipApproach()
        {
            _myCommandMobs[0].transform.position = _myCommandMobs[0].transform.position.SetX(Indent);
            _myCommandMobs[0].transform.localPosition = _myCommandMobs[0].transform.position.SetY(JumpHeight);
        }
        
        private void SkipLanding()
        {
            _myCommandMobs[0].transform.localPosition = Vector3.zero;
        }

        private void SkipIntro()
        {
            for (int i = 0; i < _myCommandMobs.Count; i++)
            {
                _myCommandMobs[i].transform.localPosition = _myCommandMobs[i].transform.localPosition.SetX(DistanceBetweenTeams * i);
                _enemyCommandMobs[i].transform.localPosition = _enemyCommandMobs[i].transform.localPosition.SetX(DistanceBetweenTeams * i);
            }
        }

        private IEnumerator AwaitPreparingAttackAnimation(Mob myActiveMob)
        {
            myActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.OutlitRedMaterial;
            yield return  _shakeService.Shake(myActiveMob.gameObject, _preparingAttackDelay);
        }

        private IEnumerator AwaitUpdatePositionPetsAnimation(List<Mob> commandMobsActive)
        {
            for (int i = 0; i < commandMobsActive.Count; i++)
            {
                LeanTween.moveLocalX(commandMobsActive[i].gameObject, DistanceBetweenTeams * i, _updatePositionDelay);
            }
            
            yield return new WaitForSeconds(_updatePositionDelay);
        }
        
        private IEnumerator AwaitUsePerk()
        {
            yield break;
        }
        
        private List<Mob> MyCommandMobsActive()
        {
            var activeMobs = new List<Mob>();

            foreach (var mob in _myCommandMobs)
                if (mob.IsActive)
                    activeMobs.Add(mob);

            return activeMobs;
        }

        private List<Mob> EnemyCommandMobsActive()
        {
            var activeMobs = new List<Mob>();

            foreach (var mob in _enemyCommandMobs)
                if (mob.IsActive)
                    activeMobs.Add(mob);

            return activeMobs;
        }
        
        private Mob MyActiveMob()
        {
            var myCommandMobs = MyCommandMobsActive();
            return myCommandMobs.Count == 0 ? null : myCommandMobs[0];
        }

        private Mob EnemyActiveMob()
        {
            var enemyCommandMobs = EnemyCommandMobsActive();
            return enemyCommandMobs.Count == 0 ? null : enemyCommandMobs[0];
        }

        public override void RemovePets()
        {
            foreach (var mob in _myCommandMobs)
            {
                mob.OnFaint -= Faint;
                Destroy(mob.gameObject);
            }

            foreach (var mob in _enemyCommandMobs)
            {
                mob.OnFaint -= Faint;
                Destroy(mob.gameObject);
            }

            _myCommandMobs.RemoveAll(IsMobRemove);
            _enemyCommandMobs.RemoveAll(IsMobRemove);
        }
        
        private static bool IsMobRemove(Mob mob)
        {
            return true;
        }

        private void Faint(Mob mob)
        {
            if (!mob.IsEnemy)
                StartCoroutine(AwaitEmergencyLanding(mob, _camera.ScreenToWorldPoint(
                new Vector3(0, 0, 0)).x + Indent * 2 * -1));
            else
                StartCoroutine(AwaitEmergencyLanding(mob, _camera.ScreenToWorldPoint(
                    new Vector3(UnityEngine.Screen.width, 0, 0)).x + Indent * 2));
        }
    }
}