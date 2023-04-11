using UnityEngine;

namespace Infrastructure
{
    [CreateAssetMenu(fileName = "GameBootstrapperSettings", menuName = "Infrastructure/GameBootstrapperSettings",
        order = 0)]
    public class GameBootstrapperSettings : ScriptableObject
    {
        [field: SerializeField] public string SceneToLoadName { get; private set; } = "Level1";
    }
}