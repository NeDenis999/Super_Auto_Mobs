using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class BattleService : MonoBehaviour
    {
        private const float Indent = 0.5f;
        
        [SerializeField]
        private List<Mob> _myCommandMobs;
        
        [SerializeField]
        private List<Mob> _enemyCommandMobs;

        [SerializeField]
        private TextMeshProUGUI _myCommandText;
        
        [SerializeField]
        private TextMeshProUGUI _vsText;
        
        [SerializeField]
        private TextMeshProUGUI _enemyCommandText;

        private Camera _camera;
        private readonly Vector2 _myApproachPoint = new(Indent, -0.6f);
        private List<Mob> _myCommandMobsActive
        {
            get
            {
                var activeMobs = new List<Mob>();

                foreach (var mob in _myCommandMobs)
                {
                    if (mob.IsActive)
                    {
                        activeMobs.Add(mob);
                    }
                }

                return activeMobs;
            }
        }

        private Mob _myActiveMob => _myCommandMobsActive[0];

        private void Start()
        {
            _camera = Camera.main;
        }

        public IEnumerator AwaitIntro()
        {
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
                LeanTween.moveLocalX(_myCommandMobs[i].gameObject, -1.5f * i, 1 - 0.15f * i)
                    .setOnComplete(() =>
                    {
                        LeanTween.cancel(_myCommandMobs[i].gameObject);
                        LeanTween.moveLocalY(_myCommandMobs[i].gameObject, 0, 0.3f);
                    });
                
                LeanTween.moveLocalX(_enemyCommandMobs[i].gameObject, -1.5f * i, 1- 0.15f * i)
                    .setOnComplete(() =>
                    {
                        LeanTween.cancel(_enemyCommandMobs[i].gameObject);
                        LeanTween.moveLocalY(_enemyCommandMobs[i].gameObject, 0, 0.3f);
                    });
                
                LeanTween.moveLocalY(_myCommandMobs[i].gameObject, 1, 0.3f).setLoopPingPong().setEaseOutCubic();
                LeanTween.moveLocalY(_enemyCommandMobs[i].gameObject, 1, 0.3f).setLoopPingPong().setEaseOutCubic();
                yield return new WaitForSeconds(1f);
            }
            
            LeanTween.scale(_myCommandText.gameObject, Vector3.zero, 0.5f).setEaseOutCubic();
            LeanTween.scale(_vsText.gameObject, Vector3.zero,  0.5f).setEaseOutCubic();
            LeanTween.scale(_enemyCommandText.gameObject, Vector3.zero,  0.5f).setEaseOutCubic();
            
            yield return new WaitForSeconds(1);
        }

        public IEnumerator AwaitApproach()
        {
            LeanTween.move(_myCommandMobs[0].gameObject, _myApproachPoint, 1).setEaseOutCubic();;
            yield return new WaitForSeconds(1f);
        }
        
        public IEnumerator AwaitLanding()
        {
            LeanTween.moveLocal(_myCommandMobs[0].gameObject, Vector3.zero, 1).setEaseOutCubic();;
            yield return new WaitForSeconds(1f);
        }
        
        public IEnumerator AwaitEmergencyLanding()
        {
            var startMyPosition = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - Indent;
            LeanTween.move(_myCommandMobs[0].gameObject, new Vector3(startMyPosition, -2), 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        public void SkipApproach()
        {
            _myCommandMobs[0].transform.position = _myApproachPoint;
        }
        
        private void SkipLanding()
        {
            _myCommandMobs[0].transform.localPosition = Vector3.zero;
        }

        public void SkipIntro()
        {
            for (int i = 0; i < _myCommandMobs.Count; i++)
            {
                _myCommandMobs[i].transform.localPosition = _myCommandMobs[i].transform.localPosition.SetX(-1.5f * i);
                _enemyCommandMobs[i].transform.localPosition = _enemyCommandMobs[i].transform.localPosition.SetX(-1.5f * i);
            }
        }
    }
}