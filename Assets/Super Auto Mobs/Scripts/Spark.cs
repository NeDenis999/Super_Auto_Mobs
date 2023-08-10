using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Spark : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sprite;
        
        public void SetColor(Color color)
        {
            _sprite.color = color;
        }

        public void SetSprite(Sprite sprite)
        {
            _sprite.sprite = sprite;
        }
    }
}