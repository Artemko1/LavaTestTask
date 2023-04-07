using Cinemachine;
using UnityEngine;

namespace Infrastructure
{
    [DefaultExecutionOrder(-100)]
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private GameObject _heroPrefab;
        [SerializeField] private CinemachineVirtualCamera _playerFollowVirtualCameraPrefab;

        public void Start() =>
            InitGameWorld();

        private void InitGameWorld()
        {
            GameObject hero = CreateHero();
            CreateFollowCamera(hero);
        }

        private void CreateFollowCamera(GameObject hero)
        {
            CinemachineVirtualCamera virtualCamera = Instantiate(_playerFollowVirtualCameraPrefab);
            virtualCamera.Follow = hero.transform;
        }

        private GameObject CreateHero() =>
            Instantiate(_heroPrefab, _playerSpawnPoint.position, Quaternion.identity);
    }
}