using UnityEngine;

namespace Twitchery.Content.Scripts
{
    public class AETE_DuplicatedDupe : KMonoBehaviour, ISim1000ms
    {
        public float remainingLifeTimeSeconds;

        [SerializeField] public StatusItem statusItem;

        public override void OnSpawn()
        {
            base.OnSpawn();
            if(TryGetComponent(out KSelectable kSelectable))
            {
                kSelectable.AddStatusItem(statusItem, this);
            }
        }

        public void Sim1000ms(float dt)
        {
            remainingLifeTimeSeconds -= dt / 1000f;
            if(remainingLifeTimeSeconds <= 0)
                Die();
        }

        public object GetDeathTime() => GameUtil.GetFormattedTime(remainingLifeTimeSeconds);

        private void Die()
        {
            Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, transform.position, 0);
            Util.KDestroyGameObject(this);
        }

        public void CopyStatsFrom(MinionIdentity original)
        {

        }
    }
}
