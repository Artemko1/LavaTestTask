using DG.Tweening;
using UnityEngine;

namespace Infrastructure
{
    [DefaultExecutionOrder(-110), RequireComponent(typeof(SaveLoadService), typeof(SceneLoader))]
    public class GameBootstrapper : MonoBehaviour
    {
        private static GameBootstrapper _instance;

        [SerializeField] private GameBootstrapperSettings _settings;

        private SaveLoadService _saveLoadService;
        private SceneLoader _sceneLoader;

        public void Start()
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
            LoadDefaultScene();
        }

        private static void SetTargetFramerate() =>
            Application.targetFrameRate = 60;

        private void InitializeServices()
        {
            DOTween.Init();
            _saveLoadService.LoadProgress();
        }

        private void LoadDefaultScene() =>
            _sceneLoader.Load(_settings.SceneToLoadName);
    }
}