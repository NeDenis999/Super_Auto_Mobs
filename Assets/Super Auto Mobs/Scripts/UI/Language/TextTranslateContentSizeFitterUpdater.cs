using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Super_Auto_Mobs
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public class TextTranslateContentSizeFitterUpdater : MonoBehaviour
    {
        private TextTranslate _textTranslate;
        private ContentSizeFitter _contentSizeFitter;

        private void Awake()
        {
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
            _textTranslate = GetComponent<TextTranslate>();
        }

        private void OnEnable()
        {
            _textTranslate.OnTextUpdate += SizeUpdate;
        }
        
        private void OnDisable()
        {
            _textTranslate.OnTextUpdate -= SizeUpdate;
        }

        private void SizeUpdate()
        {
            _contentSizeFitter.SetLayoutHorizontal();
        }
    }
}