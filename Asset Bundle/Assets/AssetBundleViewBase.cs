using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AssetBundleViewBase : MonoBehaviour
{
    private const string UrlAssetBundleSprites = "https://drive.google.com/uc?export=download&id=1Ps7ZbM5_k9GHg-nCJkJL4UHvUOWlGMGV";
    private const string UrlAssetBundleAudio = "https://drive.google.com/uc?export=download&id=1FjtPveQqlavnYIv5YD86W2oM4INobM3c";

    [SerializeField]
    private DataSpriteBundle[] _dataSpriteBundle;


    [SerializeField]
    private DataAudioBundle[] _dataAudioBundle;

    private AssetBundle _spriteAssetBundle;
    private AssetBundle _audioAssetBundle;

    protected IEnumerator DownloadAndSetAssetBundle()
    {
        yield return GetSpritesAssetBundle();
        yield return GetAudioAssetBundle();

        if (_spriteAssetBundle == null || _audioAssetBundle == null)
        {
            Debug.LogError("Error asset bundle");
            yield break;
        }

        SetDownLoadAssets();
        yield break;
    }

    private void SetDownLoadAssets()
    {
        foreach (var data in _dataSpriteBundle)
        {
            data.Image.sprite = _spriteAssetBundle.LoadAsset<Sprite>(data.IdImage);
        }

        foreach (var data in _dataAudioBundle)
        {
            data.AudioSource.clip = _audioAssetBundle.LoadAsset<AudioClip>(data.IdAudio);
            data.AudioSource.Play();
        }
    }

    private IEnumerator GetAudioAssetBundle()
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleAudio);

        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        StateRequest(request, ref _audioAssetBundle);
    }

    private IEnumerator GetSpritesAssetBundle()
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleSprites);

        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        StateRequest(request, ref _spriteAssetBundle);
    }

    private void StateRequest(UnityWebRequest request, ref AssetBundle assetBundle)
    {
        if (request.error == null)
        {
            assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            Debug.LogError("Complete");
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
