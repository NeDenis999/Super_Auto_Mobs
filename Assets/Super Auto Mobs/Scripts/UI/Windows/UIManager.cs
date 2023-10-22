using UnityEngine;

namespace Super_Auto_Mobs
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] 
        private SerializablePair<WindowType, BaseWindow>[] _windows;

        public void Show(WindowType type, object args = null, bool hideOther = true)
        {
            if (hideOther)
                HideAll();

            foreach (var pair in _windows)
            {
                if (pair.Key == type)
                {
                    pair.Value.Show();
                    pair.Value.Bind(args);
                }
            }
        }

        public void HideAll()
        {
            foreach (var pair in _windows)
                pair.Value.Hide();
        }

        public void Toggle(WindowType type, object args = null, bool hideOther = true)
        {
            foreach (var pair in _windows)
            {
                if (pair.Key == type)
                {
                    if (pair.Value.gameObject.activeSelf)
                    {
                        pair.Value.Hide(); 
                    }
                    else
                    {
                        if (hideOther)
                            HideAll();
                        
                        pair.Value.Show();
                        pair.Value.Bind(args);
                    }
                }
            }
        }
    }
}