using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class StorageInformationViewer : MonoBehaviour
{
    [SerializeField] private ResourceItem _prefabResourceItem;
    [SerializeField] private RectTransform _resourceViewer;
    [SerializeField] private TMP_Text _textMessage;
    [SerializeField] private InputResource[] _resources;

    private void Start()
    {
        InitializeResourceViewer();
    }

    public void SetResources(InputResource[] resources)
    {
        _resources = resources;
        ClearVResourceiewer();
        InitializeResourceViewer();
    }

    private void InitializeResourceViewer()
    {
        for (int i = 0; i < _resources.Length; i++)
        {
            ResourceItem item = Instantiate(_prefabResourceItem, _resourceViewer);
            Instantiate(_resources[i].Resource, item.PlaceForResource);
            item.TextResourceInfo.text = _resources[i].Count + "/" + _resources[i].Required;
        }
    }

    private void ClearVResourceiewer()
    {
        foreach (RectTransform child in _resourceViewer)
        {
            Destroy(child.gameObject);
        }
    }

    public void EnableTextMessage(bool isVisible)
    {
        _textMessage.gameObject.SetActive(isVisible);
    }


    [System.Serializable]
    public struct InputResource
    {
        public Resource Resource;
        public int Count;
        public int Required;
    }
}
