using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Super_Auto_Mobs
{
    public class MineshieldCutScene2 : CutScene
    {
        [SerializeField]
        private Dialogue _dialog1;

        public override IEnumerator Play()
        {
            yield return base.Play();
            yield return AwaitDialogHide(_dialog1);
        }
    }
}