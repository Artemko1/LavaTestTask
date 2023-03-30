using System;
using System.Collections;
using Data;
using UnityEngine;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class SaveLoadService : MonoBehaviour
    {
        private const string ProgressKey = "Progress";
        [SerializeField] private PlayerProgressProvider _progressService;

        public event Action<PlayerProgress> OnSave;

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
            OnSave?.Invoke(_progressService.PlayerProgress);

            string progressJson = _progressService.PlayerProgress.ToJson();
            PlayerPrefs.SetString(ProgressKey, progressJson);
        }

        public void LoadProgress()
        {
            string progressJson = PlayerPrefs.GetString(ProgressKey); //todo move from prefs to file save
            _progressService.PlayerProgress = progressJson?.ToDeserialized<PlayerProgress>() ?? new PlayerProgress();
        }
    }

    public static class JsonExtensions
    {
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);
        public static T ToDeserialized<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
    }
}