using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class SparkService : MonoBehaviour
    {
        private const int StartCount = 5;
        
        private Spark _sparkPrefab;
        private AssetProviderService _assetProviderService;
        private List<Spark> _sparks = new();

        [Inject]
        private void Construct(AssetProviderService assetProviderService)
        {
            _sparkPrefab = assetProviderService.Spark;
            _assetProviderService = assetProviderService;
        }

        private void Start()
        {
            for (int i = 0; i < StartCount; i++)
            {
                SpawnSpark();
            }
        }

        public void StartAnimation(Vector2 start, Vector2 target, SparkEnum sparkEnum, Color color = default, 
            float delay = Constants.DelayPerk)
        {
            var spark = GetSpark();
            spark.SetColor(color);

            switch (sparkEnum)
            {
                case SparkEnum.Attack:
                    spark.SetSprite(_assetProviderService.AttackSprite);
                    break;
                case SparkEnum.Heart:
                    spark.SetSprite(_assetProviderService.HeartSprite);
                    break;
                case  SparkEnum.Damage:
                    spark.SetSprite(_assetProviderService.DamageSprite);
                    break;
                case  SparkEnum.ActivatePerk:
                    spark.SetSprite(_assetProviderService.ActivatePerkSprite);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sparkEnum), sparkEnum, null);
            }

            spark.transform.position = start;
            
            var pathPoints = MathfExtensions.CreateSineWave(start, 
                target, 50, 1, 0.5f);
            
            LeanTween.moveSpline(spark.gameObject, pathPoints, delay)
                .setEaseInSine()
                .setOnComplete(() => BackSpark(spark));
        }
        
        public Spark GetSpark()
        {
            foreach (var spark in _sparks)
            {
                if (!spark.gameObject.activeSelf)
                {
                    spark.gameObject.SetActive(true);
                    return spark;
                }
            }

            return SpawnSpark();
        }

        public void BackSpark(Spark spark)
        {
            spark.gameObject.SetActive(false);
        }

        private Spark SpawnSpark()
        {
            var spark = Instantiate(_sparkPrefab, transform.position, Quaternion.identity, transform);
            spark.gameObject.SetActive(false);
            _sparks.Add(spark);
            return spark;
        }
    }
}