using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerProgressProvider", menuName = "PlayerProgressProvider", order = 0)]
    public class PlayerProgressProvider : ScriptableObject
    {
        // дописать класс, добавить сюда прогресс. Линковать SO везде где нужен доступ к прогрессу.
        // Пролинковать так же в гейм бутстраппер, чтобы прокинуть до LoadProgressState и там инициализировать.

        public PlayerProgress PlayerProgress { get; set; } = new PlayerProgress(); // убрать new, когда сделаю LoadProgressState
    }
}