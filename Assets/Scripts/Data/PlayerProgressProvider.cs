using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerProgressProvider", menuName = "PlayerProgressProvider", order = 0)]
    public class PlayerProgressProvider : ScriptableObject
    {
        public PlayerProgress PlayerProgress { get; set; }
    }
}