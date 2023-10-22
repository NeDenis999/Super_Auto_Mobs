using System.Collections;
using UnityEngine;

namespace Super_Auto_Mobs
{
    public class ShopCommandMobPlatform : ShopPlatform
    {
        public Mob Mob 
        { 
            get => (Mob)_entity;
            set
            {
                _entity = value;
                _isMove = false;
                
                if (_entity)
                    _entity.transform.SetParent(_spawnPoint);
            }
        }

        public IEnumerator ResetPositionXSmoothly()
        {
            _isMove = false;
            LeanTween.cancel(gameObject);
            
            yield return LeanTween.move(_entity.gameObject, _spawnPoint.transform.position, 0.5f)
                .setEaseOutBack();
        }

        public void ResetPositionY()
        {
            _entity.transform.position = _entity.transform.position.SetY(_spawnPoint.position.y);
        }
    }
}