using System;
using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class ContentSizeFittersUpdater : MonoBehaviour
    {
        private ContentSizeFitter[] _contentSizeFitters;

        private void Awake()
        {
            _contentSizeFitters = GetComponentsInChildren<ContentSizeFitter>();
        }

        private void Start()
        {
            SizeUpdate();
        }

        private void SizeUpdate()
        {
            foreach (var contentSizeFitter in _contentSizeFitters)
            {
                contentSizeFitter.SetLayoutHorizontal();
                print(1);
            }
        }
    }
}