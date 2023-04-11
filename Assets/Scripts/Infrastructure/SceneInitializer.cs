using Cinemachine;
using Logic;
using UnityEngine;

namespace Infrastructure
{
    [DefaultExecutionOrder(-100)]
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _heroPrefab;
        [SerializeField] private CinemachineVirtualCamera _playerFollowVirtualCameraPrefab;

        [SerializeField] private Tutorial _tutorialPrefab;
        [SerializeField] private Tutorial.Settings _tutorialSettings;

        public void Start() =>
            InitGameWorld();

        private void InitGameWorld()
        {
            GameObject hero = CreateHero();
            CreateFollowCamera(hero);

            if (_tutorialSettings.EnableTutorial)
            {
                CreateTutorial(hero);
            }
        }

        private GameObject CreateHero() =>
            Instantiate(_heroPrefab, _playerSpawnPoint.position, Quaternion.identity);

        private void CreateFollowCamera(GameObject hero)
        {
            CinemachineVirtualCamera virtualCamera = Instantiate(_playerFollowVirtualCameraPrefab);
            virtualCamera.Follow = hero.transform;
        }

        private void CreateTutorial(GameObject hero)
        {
            Tutorial tutorial = Instantiate(_tutorialPrefab, hero.transform);
            tutorial.Init(_tutorialSettings);
        }
    }
}