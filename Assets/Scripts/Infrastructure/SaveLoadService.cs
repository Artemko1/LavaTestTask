using System;
using Data;
using UnityEngine;

namespace Infrastructure
{
    public class SaveLoadService : MonoBehaviour
    {
        private const string ProgressKey = "Progress";
        [SerializeField] private PlayerProgressProvider _progressService;

        public event Action<PlayerProgress> OnSave;

        public void SaveProgress()
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
        public static string ToJson(this object obj) => JsonUtility.ToJson(obj);
        public static T ToDeserialized<T>(this string json) => JsonUtility.FromJson<T>(json);
    }
}