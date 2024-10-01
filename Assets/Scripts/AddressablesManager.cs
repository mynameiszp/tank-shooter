using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}

public class AddressablesManager : MonoBehaviour
{
    [SerializeField] private AssetReferenceAudioClip _gameMusicReference;

    private void Start()
    {
        Addressables.InitializeAsync().Completed += HandleInitialization;
    }

    private void HandleInitialization(AsyncOperationHandle<IResourceLocator> handle)
    {
        _gameMusicReference.LoadAssetAsync<AudioClip>().Completed += (clip) =>
        {
            InitializeAudioSource(clip);
        };
    }

    private void InitializeAudioSource(AsyncOperationHandle<AudioClip> clip)
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip.Result;
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        if (_gameMusicReference.IsValid())
        {
            Addressables.Release(_gameMusicReference);
        }
    }
}
