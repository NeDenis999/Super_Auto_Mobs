using System;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class SparkService : MonoBehaviour
    {
        private const int StartCount = 5;
        
        private Spark _sparkPrefab;
        private List<Spark> _sparks = new();

        [Inject]
        private void Construct(AssetProviderService assetProviderService)
        {
            _sparkPrefab = assetProviderService.Spark;
        }

        private void Start()
        {
            for (int i = 0; i < StartCount; i++)
            {
                SpawnSpark();
            }
        }

        public void StartAnimation(Vector2 start, Vector2 target, Color color = default, float delay = 1)
        {
            var spark = GetSpark();

            if (color == default)
            {
                color = Color.green;
            }
            
            spark.SetColor(color);
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