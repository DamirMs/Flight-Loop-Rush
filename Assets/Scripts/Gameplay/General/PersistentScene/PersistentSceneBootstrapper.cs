using Cysharp.Threading.Tasks;
using PT.Logic.PersistentScene;
using UnityEngine;
using Zenject;

namespace Gameplay.General.PersistentScene
{
    public class PersistentSceneBootstrapper : MonoBehaviour
    {
        [Inject] private SceneLoadManager _sceneLoadManager;

        private void Start()
        {
            _sceneLoadManager.LoadScene(SceneNameEnum.Menu).Forget();
        }
    }
}