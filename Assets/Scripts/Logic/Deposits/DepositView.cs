using System.Collections.Generic;
using Data.DataLoot;
using DG.Tweening;
using Logic.Drop;
using UnityEngine;

namespace Logic.Deposits
{
    public class DepositView : MonoBehaviour
    {
        [SerializeField] private LootDropPrefabsByType _lootDropPrefabsByType;
        [SerializeField] private GameObject _viewBase;
        [SerializeField] private Transform _dropSpawnPoint;
        [SerializeField] private List<GameObject> _viewVariants;

        private void Start()
        {
            DisableAllViews();
            _viewVariants[^1].SetActive(true);
        }

        public void PlayMiningAnimation() =>
            _viewBase.transform.DOPunchScale(new Vector3(-0.3f, -0.3f, -0.3f), 0.3f, elasticity: 0.5f);

        public void ChangeView(int miningLeft)
        {
            DisableAllViews();

            int newViewIndex = Mathf.Clamp(miningLeft, 0, _viewVariants.Count - 1);
            _viewVariants[newViewIndex].SetActive(true);
        }

        public void DropLoot(Loot loot)
        {
            DroppedLoot droppedLootPrefab = _lootDropPrefabsByType.GetForType(loot.Type);

            for (var i = 0; i < loot.Amount; i++)
            {
                DroppedLoot drop = Instantiate(droppedLootPrefab, _dropSpawnPoint.position, Random.rotation);
                drop.Init(new Loot(loot.Type, 1), true);

                Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.75f, 1.25f);
                float verticalMagnitude = Random.Range(4f, 5f);

                var rb = drop.GetComponent<Rigidbody>();
                rb.AddForce(randomHorizontalMagnitude.x, verticalMagnitude, randomHorizontalMagnitude.y, ForceMode.VelocityChange);
            }
        }

        private void DisableAllViews()
        {
            foreach (GameObject viewVariant in _viewVariants)
            {
                viewVariant.SetActive(false);
            }
        }
    }
}