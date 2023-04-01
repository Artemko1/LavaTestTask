using System.Collections;
using System.IO;
using Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Infrastructure
{
    public class SaveLoadService : MonoBehaviour
    {
        [SerializeField] private PlayerProgressProvider _progressService;

        private string _saveJsonFilePath;
        private string _persistentFolder;

        private void Awake()
        {
            _persistentFolder = Application.persistentDataPath + "/";
            _saveJsonFilePath = _persistentFolder + "Save.json";
        }

        private IEnumerator Start()
        {
            var waitForSeconds = new WaitForSeconds(5);
            while (true)
            {
                yield return waitForSeconds;
                SaveProgress();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveProgress();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveProgress();
            }
        }


        private void SaveProgress()
        {
            Directory.CreateDirectory(_persistentFolder);

            string progressJson = _progressService.PlayerProgress.ToJson();

            File.WriteAllText(_saveJsonFilePath, progressJson);
        }

        public void LoadProgress()
        {
            PlayerProgress loadedProgress;
            if (File.Exists(_saveJsonFilePath))
            {
                string json = File.ReadAllText(_saveJsonFilePath);
                loadedProgress = json.ToDeserialized<PlayerProgress>();
                Assert.IsNotNull(loadedProgress);
            }
            else
            {
                loadedProgress = new PlayerProgress();
            }

            _progressService.PlayerProgress = loadedProgress;
        }
    }

    public static class JsonExtensions
    {
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);
        public static T ToDeserialized<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
    }
}