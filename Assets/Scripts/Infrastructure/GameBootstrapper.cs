using Data;
using DG.Tweening;
using UnityEngine;

namespace Infrastructure
{
    [DefaultExecutionOrder(-110), RequireComponent(typeof(SaveLoadService), typeof(SceneLoader))]
    public class GameBootstrapper : MonoBehaviour
    {
        private const string DefaultSceneName = "Level1";

        private static GameBootstrapper _instance;

        private SaveLoadService _saveLoadService;
        private SceneLoader _sceneLoader;

        public void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);

            _saveLoadService = GetComponent<SaveLoadService>();
            _sceneLoader = GetComponent<SceneLoader>();

            SetTargetFramerate();
            InitializeServices();
            LoadProgress();
            LoadDefaultScene();
        }

        private static void SetTargetFramerate() =>
            Application.targetFrameRate = 60;

        private void InitializeServices()
        {
            DOTween.Init();
        }

        private void LoadProgress() =>
            _saveLoadService.LoadProgress();

        private void LoadDefaultScene() =>
            _sceneLoader.Load(DefaultSceneName);
    }
}