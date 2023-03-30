using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UI
{
    public class LootDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerProgressProvider _playerProgressProvider;
        [SerializeField] private LootSpritesByType _lootSpritesByType;

        [SerializeField] private LootRowView _lootRowViewPrefab;
        [SerializeField] private RectTransform _lootRowViewParent;

        private readonly Dictionary<LootType, LootRowView> _lootRowsForTypes = new Dictionary<LootType, LootRowView>();

        private LootData _lootData;

        private void Start()
        {
            CleanupEditorObjects();

            _lootData = _playerProgressProvider.PlayerProgress.LootData;
            _lootData.Collected += UpdateOnCollected;
        }

        private void CleanupEditorObjects()
        {
            foreach (Transform child in _lootRowViewParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void UpdateOnCollected(LootCollectedArgs args)
        {
            if (_lootRowsForTypes.ContainsKey(args.Type))
            {
                LootRowView lootRow = _lootRowsForTypes[args.Type];
                lootRow.gameObject.SetActive(true);
                lootRow.SetText(args.TotalAmount.ToString());
                lootRow.PlayIncreaseAnimation();
            }
            else
            {
                LootRowView lootRow = CreateLootRowView(args);

                lootRow.SetText(args.TotalAmount.ToString());
                lootRow.PlayIncreaseAnimation();
            }
        }

        private LootRowView CreateLootRowView(LootCollectedArgs args)
        {
            LootRowView newRow = Instantiate(_lootRowViewPrefab, _lootRowViewParent);
            newRow.SetSprite(GetSpriteForLootType(args.Type));
            _lootRowsForTypes[args.Type] = newRow;
            return newRow;
        }

        private Sprite GetSpriteForLootType(LootType lootType)
        {
            return _lootSpritesByType.GetForType(lootType);
        }
    }
}