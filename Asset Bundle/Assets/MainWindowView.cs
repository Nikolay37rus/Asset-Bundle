using UnityEngine;
using UnityEngine.UI;

public class MainWindowView : AssetBundleViewBase
{
    [SerializeField]
    private Button _loadAssetButton;

    private void Start()
    {
        _loadAssetButton.onClick.AddListener(LoadAssets);
    }

    private void OnDestroy()
    {
        _loadAssetButton.onClick.RemoveAllListeners();
    }

    private void LoadAssets()
    {
        _loadAssetButton.interactable = false;

        StartCoroutine(DownloadAndSetAssetBundle());
    }
}
