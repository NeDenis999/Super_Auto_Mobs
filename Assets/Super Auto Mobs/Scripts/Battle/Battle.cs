using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Battle : MonoBehaviour
    {
        private BattleService _battleService;

        [Inject]
        private void Construct(BattleService battleService)
        {
            _battleService = battleService;
        }
        
        private void Start()
        {
            StartCoroutine(ProcessBattle());
        }

        private IEnumerator ProcessBattle()
        {
            _battleService.SkipIntro();
            //_battleService.SkipApproach();
            //yield return StartCoroutine(_battleService.AwaitIntro());
            yield return StartCoroutine(_battleService.AwaitApproach());
            yield return StartCoroutine(_battleService.AwaitLanding());
            
            yield return StartCoroutine(_battleService.AwaitApproach());
            yield return StartCoroutine(_battleService.AwaitLanding());
            //yield return StartCoroutine(_battleService.AwaitEmergencyLanding());
        }
    }
}