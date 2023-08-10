using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Super_Auto_Mobs
{
    public class Carousel : MonoBehaviour
    {
        [SerializeField]
        private List<World> _worlds;

        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private Transform _container;
        
        [SerializeField]
        private float _horizontalSize, _distance, _speed, _speedDropRate, _maxSpeed;

        private List<GameObject> _deactivatePrefabs = new();
        private List<GameObject> _activePrefabs = new();
        private DiContainer _diContainer;
        private CoroutineRunner _coroutineRunner;
        
        private float _previousMousePositionX;
        private float _currentSpeed;
        private int _worldLeftNumber;
        private int _worldRightNumber;

        [Inject]
        private void Construct(DiContainer diContainer, CoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _diContainer = diContainer;

        }

        private void Start()
        {
            _worldLeftNumber = _worlds.Count - 1;
;           _coroutineRunner.StartCoroutine(ProcessCreate());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _previousMousePositionX = Input.mousePosition.x;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _currentSpeed = (Input.mousePosition.x - _previousMousePositionX) * _speed;
            }
            
            if (Input.GetMouseButton(0))
            {
                var moveX = Input.mousePosition.x - _previousMousePositionX;
                _container.position = _container.position.AddX(moveX);
                _previousMousePositionX = Input.mousePosition.x;
            }
            else
            {
                _container.position = _container.position.AddX(_currentSpeed * Time.deltaTime);
                _currentSpeed = Mathf.Clamp(Mathf.MoveTowards(_currentSpeed, 0, 
                        _speedDropRate), 
                    -_maxSpeed, _maxSpeed);
            }
        }
        
        private IEnumerator ProcessCreate()
        {
            PutPrefab(SideEnum.Right);
            
            while (true)
            {
                if (IsLackLeft())
                {
                    PutPrefab(SideEnum.Left);
                }
                else if (IsLackNextLeft())
                {
                    _deactivatePrefabs.Add(_activePrefabs[0]);
                    _activePrefabs[0].SetActive(false);
                    _activePrefabs.Remove(_activePrefabs[0]);
                    
                    _worldLeftNumber++;

                    if (_worldLeftNumber >= _worlds.Count)
                        _worldLeftNumber = 0;
                }
 
                if (IsLackRight())
                {
                    PutPrefab(SideEnum.Right);
                }
                else if (IsLackNextRight())
                {
                    _deactivatePrefabs.Add(_activePrefabs[_activePrefabs.Count - 1]);
                    _activePrefabs[_activePrefabs.Count - 1].SetActive(false);
                    _activePrefabs.Remove(_activePrefabs[_activePrefabs.Count - 1]);
                    
                    _worldRightNumber--;

                    if (_worldRightNumber < 0)
                        _worldRightNumber = _worlds.Count - 1;
                }
                
                yield return null;
            }
        }

        private void PutPrefab(SideEnum sideEnum)
        {
            GameObject prefab;
            var horizontalHalfSize = _horizontalSize / 2;
            var positionX = 0f;

            if (_activePrefabs.Count == 0)
            {
                positionX = 0;
            }
            else if (sideEnum == SideEnum.Left)
            {
                positionX = _activePrefabs[0].GetComponent<RectTransform>().anchoredPosition.x - (horizontalHalfSize + _distance);
            }
            else
            {
                positionX = _activePrefabs[_activePrefabs.Count - 1].GetComponent<RectTransform>().anchoredPosition.x + (horizontalHalfSize + _distance);
            }

            if (_deactivatePrefabs.Count == 0)
                prefab = Create();
            else
            {
                prefab = _deactivatePrefabs[0];
                prefab.SetActive(true);
                _deactivatePrefabs.Remove(prefab);
            }

            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero.SetX(positionX);
            
            prefab.GetComponent<WorldScreen>().World = GetWorld(sideEnum);
            
            if (sideEnum == SideEnum.Left)
                _activePrefabs.Insert(0, prefab);
            
            if (sideEnum == SideEnum.Right)
                _activePrefabs.Add(prefab);
        }

        private World GetWorld(SideEnum sideEnum)
        {
            var number = 0;
            
            if (sideEnum == SideEnum.Left)
            {
                number = _worldLeftNumber;
                _worldLeftNumber--;

                if (_worldLeftNumber < 0)
                    _worldLeftNumber = _worlds.Count - 1;
            }

            if (sideEnum == SideEnum.Right)
            {
                number = _worldRightNumber;
                _worldRightNumber++;

                if (_worldRightNumber >= _worlds.Count)
                    _worldRightNumber = 0;
            }

            return _worlds[number];
        }

        private GameObject Create()
        {
            var prefab = Instantiate(
                _prefab, Vector3.zero, 
                Quaternion.identity, _container);

            var worldScreen = prefab.GetComponent<WorldScreen>();
            _diContainer.Inject(worldScreen);

            foreach (var textTranslate in prefab.GetComponentsInChildren<TextTranslate>(true))
            {
                _diContainer.Inject(textTranslate);
            }

            foreach (var clickSound in prefab.GetComponentsInChildren<ClickSound>(true))
            {
                _diContainer.Inject(clickSound);
            }
            
            return prefab;
        }

        private bool IsLackLeft()
        {
            if (_activePrefabs.Count == 0)
                return true;
            
            return new Vector2(0, 0).x < _activePrefabs[0].transform.position.x;
        }

        private bool IsLackNextLeft()
        {
            if (_activePrefabs.Count < 2)
                return false;
            
            return new Vector2(0, 0).x > _activePrefabs[1].transform.position.x;
        }
        
        private bool IsLackRight()
        {
            if (_activePrefabs.Count == 0)
                return true;
            
            return new Vector2(UnityEngine.Screen.width, 0).x 
                   > _activePrefabs[_activePrefabs.Count - 1].transform.position.x;
        }
        
        private bool IsLackNextRight()
        {
            if (_activePrefabs.Count < 2)
                return false;
            
            return new Vector2(UnityEngine.Screen.width, 0).x 
                   < _activePrefabs[_activePrefabs.Count - 2].transform.position.x;
        }
    }
}