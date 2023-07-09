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
                SpawnMob(mobData, false);
            }
            
            foreach (var mobData in _sessionProgressService.EnemyCommandMobsData)
            {
                SpawnMob(mobData, true);
            }
        }

        public override void SpawnMob(MobData mobData, bool isEnemy)
        {
            var spawnTransform = isEnemy ? _enemyCommandSpawnTransform : _myCommandSpawnTransform;
            var list = isEnemy ? _enemyCommandMobs : _myCommandMobs;

            var mob = Instantiate(_assetProviderService.MobPrefab(mobData.MobEnum), spawnTransform);
            _diContainer.Inject(mob);
            _diContainer.Inject(mob.GetComponent<Perk>());
            mob.Init(mobData, isEnemy);
            list.Add(mob);
        }
        
        public override IEnumerator AwaitIntro()
        {
            if (_isSkipIntro)
            {
                SkipIntro();
                yield break;
            }

            print("AwaitIntro");
            //_animeSpeedEffect.SetActive(true);
            
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
            print("AwaitProcessBattle");
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
                    mob.Perk.Activate();
                    yield return new WaitForSeconds(1);
                }
            }
            
            foreach (var mob in _enemyCommandMobs)
            {
                if (mob.Perk.TriggeringSituation == TriggeringSituation.StartBattle)
                {
                    mob.Perk.Activate();
                    yield return new WaitForSeconds(1);
                }
            }
            
            while (MyActiveMob() && EnemyActiveMob())
            {
                StartCoroutine(Attack(false));
                yield return StartCoroutine(Attack(true));

                /*
                var myActiveMob = MyActiveMob();
                var enemyActiveMob = EnemyActiveMob();
                
                StartCoroutine(AwaitPreparingAttackAnimation(myActiveMob));
                yield return AwaitPreparingAttackAnimation(enemyActiveMob);
                
                StartCoroutine(AwaitApproachAnimation(myActiveMob, 0.3f));
                yield return AwaitApproachAnimation(enemyActiveMob, 0.3f);

                StartCoroutine(AwaitUsePerk());
                yield return AwaitUsePerk();
                
                StartCoroutine(AwaitTakeDamageAnimation(myActiveMob.CurrentAttack, _myDamageText));
                StartCoroutine(AwaitTakeDamageAnimation(enemyActiveMob.CurrentAttack, _enemyDamageText));

                myActiveMob.TakeDamage(enemyActiveMob.CurrentAttack);
                enemyActiveMob.TakeDamage(myActiveMob.CurrentAttack);
                
                myActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.DamageMaterial;
                enemyActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.DamageMaterial;

                if (myActiveMob.Perk.TriggeringSituation == TriggeringSituation.Attack)
                {
                    myActiveMob.Perk.Activate();
                    yield return new WaitForSeconds(_attackPerkDelay);
                }
                
                if (enemyActiveMob.Perk.TriggeringSituation == TriggeringSituation.Attack)
                {
                    enemyActiveMob.Perk.Activate();
                    yield return new WaitForSeconds(_attackPerkDelay);
                }
                
                if (myActiveMob.Perk.TriggeringSituation == TriggeringSituation.TakeDamage)
                {
                    myActiveMob.Perk.Activate();
                    yield return new WaitForSeconds(_takeDamagePerkDelay);
                }
                
                if (enemyActiveMob.Perk.TriggeringSituation == TriggeringSituation.TakeDamage)
                {
                    enemyActiveMob.Perk.Activate();
                    yield return new WaitForSeconds(_takeDamagePerkDelay);
                }
                
                yield return new WaitForSeconds(_damageDelay);
                myActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.DefaultMaterial;
                enemyActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.DefaultMaterial;
                
                if (myActiveMob.IsActive)
                    StartCoroutine(AwaitLandingAnimation(myActiveMob));
                else
                    StartCoroutine(AwaitEmergencyLanding(myActiveMob, _camera.ScreenToWorldPoint(
                        new Vector3(0, 0, 0)).x - Indent * 2));
                
                if (enemyActiveMob.IsActive)
                    StartCoroutine(AwaitLandingAnimation(enemyActiveMob));
                else
                    StartCoroutine(AwaitEmergencyLanding(enemyActiveMob, _camera.ScreenToWorldPoint(
                        new Vector3(UnityEngine.Screen.width, 0, 0)).x + Indent * 2));
                
                yield return new WaitForSeconds(_betweenAttacksDelay);

                StartCoroutine(AwaitUpdatePositionPetsAnimation(MyCommandMobsActive()));
                yield return AwaitUpdatePositionPetsAnimation(EnemyCommandMobsActive());
                */
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
            yield return AwaitUsePerk();
                
            StartCoroutine(AwaitTakeDamageAnimation(activeMob.CurrentAttack, damageText));

            oppositeActiveMob.TakeDamage(activeMob.CurrentAttack);
            oppositeActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.DamageMaterial;

            if (activeMob.Perk.TriggeringSituation == TriggeringSituation.Attack)
            { 
                activeMob.Perk.Activate();
                yield return new WaitForSeconds(_attackPerkDelay);
            }

            if (oppositeActiveMob.Perk.TriggeringSituation == TriggeringSituation.TakeDamage) 
            {
                oppositeActiveMob.Perk.Activate();
                yield return new WaitForSeconds(_takeDamagePerkDelay);
            }

            yield return new WaitForSeconds(_damageDelay);
            oppositeActiveMob.GetComponent<SpriteRenderer>().material = _assetProviderService.DefaultMaterial;

            if (activeMob.IsActive)
                StartCoroutine(AwaitLandingAnimation(activeMob));
            else
                StartCoroutine(AwaitEmergencyLanding(activeMob, _camera.ScreenToWorldPoint(
                        new Vector3(0, 0, 0)).x + Indent * 2 * (isEnemy ? 1 : -1)));

            yield return new WaitForSeconds(_betweenAttacksDelay);
            yield return AwaitUpdatePositionPetsAnimation(isEnemy ? EnemyCommandMobsActive() : MyCommandMobsActive());
        }

        private IEnumerator AwaitEndBattle()
        {
            //GameEnd
            var gameResult = EndBattleEnum.Faint;
            
            if (MyActiveMob())
            {
                //Win
                gameResult = EndBattleEnum.Won;

                foreach (var mob in MyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Win)
                    {
                        mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
                
                foreach (var mob in EnemyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Lose)
                    {
                        mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
            }
            else if (EnemyActiveMob())
            {
                //Lose
                gameResult = EndBattleEnum.Lose;
                
                foreach (var mob in EnemyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Win)
                    {
                        mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
                
                foreach (var mob in MyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Lose)
                    {
                        mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
            }
            else
            {
                //Faint
                gameResult = EndBattleEnum.Faint;
                
                foreach (var mob in MyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Faint)
                    {
                        mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
                
                foreach (var mob in EnemyCommandMobsActive())
                {
                    if (mob.Perk.TriggeringSituation == TriggeringSituation.Faint)
                    {
                        mob.Perk.Activate();
                        yield return new WaitForSeconds(_endGamePerkDelay);
                    }
                }
            }
            
            _levelCompleteScreen.Open(gameResult);
        }

        private IEnumerator AwaitApproachAnimation(Mob myActiveMob, float indent)
        {
            //LeanTween.moveLocalX(myActiveMob.gameObject, indent, 0.2f).setEaseOutCubic();;
            //LeanTween.moveLocalY(myActiveMob.gameObject, JumpHeight, 0.2f).setEaseOutCubic();;
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
            print("UsePerk");
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

        private void RemovePets()
        {
            foreach (var mob in _myCommandMobs)
            {
                Destroy(mob.gameObject);
            }

            foreach (var mob in _enemyCommandMobs)
            {
                Destroy(mob.gameObject);
            }

            _myCommandMobs.RemoveAll(IsMobRemove);
            _enemyCommandMobs.RemoveAll(IsMobRemove);
        }
        
        private static bool IsMobRemove(Mob mob)
        {
            return true;
        }
    }
}