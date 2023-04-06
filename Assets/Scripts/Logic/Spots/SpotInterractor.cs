using System.Collections;
using System.Collections.Generic;
using Data;
using Data.DataLoot;
using DG.Tweening;
using Logic.Drop;
using Logic.Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Spots
{
    [RequireComponent(typeof(Collider))]
    public class SpotInterractor : MonoBehaviour
    {
        private readonly List<Spot> _nearSpots = new List<Spot>();

        [SerializeField] private PlayerProgressProvider _playerProgressProvider;
        [SerializeField] private LootDropPrefabsByType _lootDropPrefabsByType;

        [SerializeField] private CharacterMovement _characterMovement;

        [SerializeField] private Transform _dropTransitionSpawnPoint;

        [SerializeField] private float _interactionCooldown = 5;

        private float _remainingCooldown;
        private LootData _lootData;

        private void Start() =>
            _lootData = _playerProgressProvider.PlayerProgress.LootData;

        private void Update()
        {
            _remainingCooldown -= Time.deltaTime;
            if (Ready())
            {
                TryGiveLootToSpot();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var spot = other.GetComponent<Spot>();
            Assert.IsNotNull(spot);
            _nearSpots.Add(spot);
        }

        private void OnTriggerExit(Collider other)
        {
            var spot = other.GetComponent<Spot>();
            _nearSpots.Remove(spot);
        }

        private bool Ready() =>
            _characterMovement.IsMoving == false && _remainingCooldown < 0 && _nearSpots.Count > 0;

        private void TryGiveLootToSpot()
        {
            Spot spot = _nearSpots[0];

            Loot loot = _lootData.TrySubtract(spot.RemainingRequiredLoot.Type, spot.RemainingRequiredLoot.Amount);

            if (loot == null || loot.Amount == 0) return;

            StartCoroutine(AnimateLootTransition(loot, spot));
            // spot.Collect(loot);
            _remainingCooldown = _interactionCooldown;
        }

        private IEnumerator AnimateLootTransition(Loot loot, Spot spot)
        {
            DroppedLoot droppedLootPrefab = _lootDropPrefabsByType.GetForType(loot.Type);

            Debug.Log("Loot amount " + loot.Amount);

            for (var i = 0; i < loot.Amount; i++)
            {
                Debug.Log("Inside loop " + i);
                DroppedLoot drop = Instantiate(droppedLootPrefab, _dropTransitionSpawnPoint.position, Random.rotation);
                var lootPerDrop = new Loot(loot.Type, 1);
                drop.Init(lootPerDrop, false);


                Vector3[] path =
                {
                    drop.transform.position + new Vector3(0, 2, 0),
                    spot.transform.position,
                };

                DOTween.Sequence()
                    // .Append(transform.DOJump(transferToPoint, 0.5f, 1, 0.35f))
                    .Append(drop.transform.DOPath(path, 2.5f, PathType.CatmullRom, gizmoColor: Color.magenta))
                    // .Join(drop.transform.DOScale(0.8f, 0.35f)) // fix
                    .OnComplete(() =>
                    {
                        spot.Collect(lootPerDrop); // инкапсулировать
                        Destroy(drop.gameObject);
                    })
                    .SetEase(Ease.Linear);


                // Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.75f, 1.25f);
                // float verticalMagnitude = Random.Range(4f, 5f);
                //
                // var rb = drop.GetComponent<Rigidbody>();
                // rb.AddForce(randomHorizontalMagnitude.x, verticalMagnitude, randomHorizontalMagnitude.y, ForceMode.VelocityChange);

                // yield return waitForSeconds;
                yield return new WaitForSeconds(0.5f);
                Debug.Log("End of loop " + i);
            }

            Debug.Log("End of function");
        }
    }
}