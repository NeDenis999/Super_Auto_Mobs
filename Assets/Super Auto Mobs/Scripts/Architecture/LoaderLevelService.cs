using UnityEngine;
using UnityEngine.SceneManagement;

namespace Super_Auto_Mobs
{
    public class LoaderLevelService : MonoBehaviour
    {
        [SerializeField]
        private SessionProgressService _sessionProgressService;

        public void LoadSceneGameplayStory()
        {
            SceneManager.LoadScene(1);
        }
        
        public void LoadSceneGameplayPVP()
        {
            SceneManager.LoadScene(1);
        }
    }
}