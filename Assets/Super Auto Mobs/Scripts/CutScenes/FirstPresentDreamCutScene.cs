using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    public class FirstPresentDreamCutScene : CutScene
    {
        [SerializeField]
        private UnityEvent _onHideStartDialog;
        
        [SerializeField]
        private UnityEvent _onEndTimeLine;
        
        [SerializeField]
        private UnityEvent _onHideEndDialog;
        
        [SerializeField]
        private PlayableDirector _playableDirector;
        
        [SerializeField]
        private List<ReplicaData> _replicasData1;
        
        [SerializeField]
        private List<ReplicaData> _replicasData2;

        private DialogService _dialogService;
        private SoundsService _soundsService;
        private ShopService _shopService;
        private Game _game;

        [Inject]
        private void Construct(DialogService dialogService, SoundsService soundsService, ShopService shopService, Game game)
        {
            _dialogService = dialogService;
            _soundsService = soundsService;
            _shopService = shopService;
            _game = game;
        }
        
        public override void Play()
        {
            base.Play();
            _game.CurrentGameState = GameState.Shop;
            _dialogService.Show(_replicasData1);
            _dialogService.OnStartHide += _soundsService.PlayDreamSpeedrun;
            _dialogService.OnHide += PlayTimeLine;
            _dialogService.OnHide += _onHideStartDialog.Invoke;
            _shopService.IsInteractive = false;
        }

        private void PlayTimeLine()
        {
            _dialogService.OnHide -= PlayTimeLine;
            _dialogService.OnHide -= _onHideStartDialog.Invoke;
            _dialogService.OnStartHide -= _soundsService.PlayDreamSpeedrun;
            _playableDirector.gameObject.SetActive(true);
            _playableDirector.Play();
            OnPlay?.Invoke();
            StartCoroutine(AwaitEnd());
        }
        
        private IEnumerator AwaitEnd()
        {
            yield return new WaitUntil(() => _playableDirector.state != PlayState.Playing);
            _dialogService.Show(_replicasData2);
            _soundsService.StopMusic();
            _onEndTimeLine?.Invoke();
            _dialogService.OnHide += _onHideEndDialog.Invoke;
            _dialogService.OnHide += OnEnd.Invoke;
            _dialogService.OnHide += () => _shopService.IsInteractive = true;
            
        }
    }
}