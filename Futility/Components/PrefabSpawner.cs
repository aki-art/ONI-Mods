using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FUtility.Components
{
    public class PrefabSpawner : KMonoBehaviour
    {
        [SerializeField]
        public List<(float, Tag)> options = new List<(float, Tag)> ();

        [SerializeField]
        public bool yeet = true;

        [SerializeField]
        public bool spawnElementInWorld = true;

        [SerializeField]
        public bool yeetOnlyUp = false;

        [SerializeField]
        public float yeetSpeed = 1f;

        [SerializeField]
        public int yeetMin = 1;

        [SerializeField]
        public int yeetMax = 3;

        [SerializeField]
        public float minDelay = 0.1f;

        [SerializeField]
        public float volume = 1f;

        [SerializeField]
        public float maxDelay = 0.5f;

        [SerializeField]
        public int minCount = 1;

        [SerializeField]
        public int maxCount;

        [SerializeField]
        public string soundFx;

        [SerializeField]
        public bool rotate;

        [SerializeField]
        public SpawnFXHashes fxHash;

        [SerializeField]
        public Action<GameObject> OnItemSpawned;

        private int itemsSpawned;

        private int itemCount;

        private bool beginSpawning = false;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            itemCount = Random.Range(minCount, maxCount);

            Log.Debuglog($"Spawned Spawner with {options.Count} options", itemCount );
            StartCoroutine(SpawnStuff());
        }

        IEnumerator SpawnStuff()
        {
            //elapsedTime += Time.deltaTime;

            while(itemsSpawned < itemCount) { 
                if (beginSpawning)
                {
                    Log.Debuglog("Spawning started");
                    var selectedItem = options.GetRandom();
                    var itemTag = selectedItem.Item2;
                    var amount = selectedItem.Item1;

                    if(spawnElementInWorld && ElementLoader.GetElement(itemTag) is Element element)
                    {
                        Log.Debuglog("Spawning an element");
                        //SimMessages.AddElementChunk(Grid.PosToCell(this), element.id, element.defaultValues.mass, element.defaultValues.temperature, )

                        if (element.IsLiquid)
                        {
                            FallingWater.instance.AddParticle(Grid.PosToCell(this), element.idx, amount, element.defaultValues.temperature, byte.MaxValue, 0);
                        }
                        else
                        {
                            SimMessages.AddRemoveSubstance(Grid.PosToCell(this), element.idx, CellEventLogger.Instance.Dumpable, element.defaultValues.mass / 10f, element.defaultValues.temperature, byte.MaxValue, 0);
                        }
                    }
                    else
                    {
                        Log.Debuglog("Spawning an item");
                        var item = Utils.Spawn(itemTag, gameObject);
                        item.GetComponent<PrimaryElement>().Mass = amount;
                        Log.Debuglog(item.PrefabID());
                        Utils.YeetRandomly(item, yeetOnlyUp, yeetMin, yeetMax, rotate);
                        OnItemSpawned?.Invoke(item);
                    }

                    if(fxHash != SpawnFXHashes.None)
                    {
                        Game.Instance.SpawnFX(fxHash, transform.GetPosition(), 0);
                    }

                    if(!soundFx.IsNullOrWhiteSpace())
                    {
                        //PlaySound(soundFx);
                        KFMOD.PlayOneShot(soundFx, SoundListenerController.Instance.transform.GetPosition(), volume);
                    }

                    itemsSpawned++;
                }
                else
                {
                    beginSpawning = true;
                }

                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }

            RemoveSelf();
            yield return null;
        }

        private void RemoveSelf()
        {
            Log.Debuglog("Deleting self");
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
