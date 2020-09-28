using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TestObject
{
    class SoundPlayer : KMonoBehaviour, ISidescreenButtonControl
    {
        //AudioSource audioSource;
        AudioSource audioSource;

        protected override void OnSpawn()
        {
            LoadAssetBundle();
        }

        public void OnSidescreenButtonPressed()
        {
            //StartCoroutine(GetAudioClip());
            Debug.Log("playing sound");
            audioSource.clip.LoadAudioData();
            audioSource.enabled = true;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.volume = 1f;
            audioSource.minDistance = 0;
            audioSource.maxDistance = float.MaxValue;
            
            audioSource.Play();
            Debug.Log("isPlaying " + audioSource.isPlaying);
            Debug.Log("length " + audioSource.clip.length);
            Debug.Log("loadState " + audioSource.clip.loadState.ToString());
            Debug.Log("isActiveAndEnabled " + audioSource.isActiveAndEnabled);
            Debug.Log("isVirtual " + audioSource.isVirtual);
            Debug.Log("volume " + audioSource.volume);

        }

        // Loads a Unity Assetbundle
        public void LoadAssetBundle()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "soundtest");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            AudioSource source = assetBundle.LoadAsset<GameObject>("AudioSource").GetComponent<AudioSource>();
            audioSource = CopyComponent(source, gameObject);
            audioSource.clip = source.clip;
            if(!audioSource.clip.LoadAudioData())
            {
                Debug.Log("couldnt load data");
            }
        }

        AudioSource CopyComponent(AudioSource original, GameObject destination)
        {
            Type type = original.GetType();
            AudioSource copy = destination.AddComponent(type) as AudioSource;
            // Copied fields can be restricted with BindingFlags
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        public string SidescreenTitleKey => "title";
        public string SidescreenStatusMessage => "";
        public string SidescreenButtonText => "Play sound";

    }
}
