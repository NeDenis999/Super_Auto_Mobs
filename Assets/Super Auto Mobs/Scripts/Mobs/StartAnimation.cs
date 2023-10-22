using UnityEngine;

namespace Super_Auto_Mobs
{
    public class StartAnimation : MonoBehaviour
    {
        [SerializeField]
        private string _nameAnimation;

        [SerializeField]
        private float _startTime = 1f;
        
        private void Start()
        {
            var animator = GetComponent<Animator>();
            animator.Play(_nameAnimation, -1, Random.Range(0f, _startTime));
        }
    }
}