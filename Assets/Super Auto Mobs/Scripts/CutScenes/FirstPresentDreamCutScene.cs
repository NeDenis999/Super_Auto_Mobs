using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
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
        private Dialogue _dialogue1;
        
        [SerializeField]
        private Dialogue _dialogue2;

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
            _dialogService.Show(_dialogue1);
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
            _dialogService.Show(_dialogue2);
            _soundsService.StopMusic();
            _onEndTimeLine?.Invoke();
            _dialogService.OnHide += EndDialog;
        }

        private void EndDialog()
        {
            _dialogService.OnHide -= EndDialog;
            _onHideEndDialog.Invoke();
            OnEnd.Invoke();
            _shopService.IsInteractive = true;
        }
    }
}