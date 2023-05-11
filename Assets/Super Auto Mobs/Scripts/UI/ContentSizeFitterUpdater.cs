using UnityEngine;
using UnityEngine.UI;

namespace Super_Auto_Mobs
{
    public class ContentSizeFitterUpdater : MonoBehaviour
    {
        private ContentSizeFitter _contentSizeFitter;

        private void Awake()
        {
            _contentSizeFitter = GetComponent<ContentSizeFitter>();
        }

        private void Start()
        {
            SizeUpdate();
        }

        private void SizeUpdate()
        {
            _contentSizeFitter.SetLayoutHorizontal();
        }
    }
}