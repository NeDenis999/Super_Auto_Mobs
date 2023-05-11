using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Super_Auto_Mobs
{
    public class ChickenMob : Mob
    {
        private void Start()
        {
            var animator = GetComponent<Animator>();
            animator.Play("ChickenIdle", -1, Random.Range(0f, 1f));
        }
    }
}