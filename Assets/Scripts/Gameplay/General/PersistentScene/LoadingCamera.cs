using PT.Logic.PersistentScene;
using UnityEngine;
using Zenject;

namespace Gameplay.General.PersistentScene
{
    public class LoadingCamera : MonoBehaviour
    {
        [SerializeField] private Camera loadingCamera;
        
        [Inject] private SceneLoadManager _sceneLoadManager;

        private void Awake()
        {
            ToggleCamera(false);
            
            _sceneLoadManager.ToggleSceneLoad += ToggleCamera;
        }
        
        private void ToggleCamera(bool isLoading)
        {
            loadingCamera.SetActive(isLoading);
        }
        
        private void OnDestroy()
        {
            _sceneLoadManager.ToggleSceneLoad -= ToggleCamera;
        }
    }
}