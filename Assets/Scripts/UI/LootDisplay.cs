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

            InitAllRows();
        }

        private void InitAllRows()
        {
            foreach (KeyValuePair<LootType, int> kvp in _lootData.Loot)
            {
                if (kvp.Value <= 0) return;

                LootRowView lootRow = CreateLootRowView(kvp.Key);
                lootRow.SetText(kvp.Value.ToString());
            }
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
                LootRowView lootRow = CreateLootRowView(args.Type);

                lootRow.SetText(args.TotalAmount.ToString());
                lootRow.PlayIncreaseAnimation();
            }
        }

        private LootRowView CreateLootRowView(LootType lootType)
        {
            LootRowView newRow = Instantiate(_lootRowViewPrefab, _lootRowViewParent);
            newRow.SetSprite(GetSpriteForLootType(lootType));
            _lootRowsForTypes[lootType] = newRow;
            return newRow;
        }

        private Sprite GetSpriteForLootType(LootType lootType) =>
            _lootSpritesByType.GetForType(lootType);
    }
}