using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class LevelCompleteScreen : Screen
    {
        public event Action OnClose;
        
        public void Open(EndBattleEnum endBattleEnum)
        {
            Open();

            switch (endBattleEnum)
            {
                case EndBattleEnum.Won:
                    break;
                case EndBattleEnum.Lose:
                    break;
                case EndBattleEnum.Faint:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(endBattleEnum), endBattleEnum, null);
            }
        }

        public override void Close()
        {
            OnClose?.Invoke();
            base.Close();
        }
    }
}