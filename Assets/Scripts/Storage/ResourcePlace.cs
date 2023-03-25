using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ResourcePlace : Storage
{
    [SerializeField] private bool _placeUnloading;
    [SerializeField] private bool _collectPlace;
    [SerializeField] private Buildings _parentBuilding;
    [SerializeField] private StorageInformationViewer _informationByStorage;

    public bool PlaceUnloading => _placeUnloading;
    public Buildings ParentBuilding => _parentBuilding;
    public bool CollectPlace => _collectPlace;

    private void UpdateInfoOfStorage()
    {
        _informationByStorage.EnableTextMessage(IsFull || IsEmpty && _placeUnloading);
        if (_placeUnloading)
        {
            Buildings.InputResource[] requireds = _parentBuilding.InputResources;
            if (requireds != null)
            {
                StorageInformationViewer.InputResource[] resources = new StorageInformationViewer.InputResource[requireds.Length];
                for (int i = 0; i < requireds.Length; i++)
                {
                    resources[i].Resource = requireds[i].Resource;
                    resources[i].Count = CountResource(requireds[i].Resource);
                    resources[i].Required = requireds[i].RequiredCount;
                }
                _informationByStorage.SetResources(resources);
            }
        }
        else
        {
            StorageInformationViewer.InputResource[] resources = new StorageInformationViewer.InputResource[1];
            resources[0].Resource = _parentBuilding.OutputResource;
            resources[0].Count = CountResource(_parentBuilding.OutputResource);
            resources[0].Required = CapacityStorage();
            _informationByStorage.SetResources(resources);
        }
    }

    public override Resource EndPull(Resource resource)
    {
        var res = base.EndPull(resource);
        UpdateInfoOfStorage();
        return res;
    }

    public override Resource LastPull()
    {
        var res = base.LastPull();
        UpdateInfoOfStorage();
        return res;
    }

    public override bool PutResource(Resource resource, float time)
    {
        var res = base.PutResource(resource, time);
        UpdateInfoOfStorage();
        return res;
    }

    public override void DestroyFromEnd(Resource resource, int count)
    {
        base.DestroyFromEnd(resource, count);
        UpdateInfoOfStorage();
    }

    public override void DestroyLast()
    {
        base.DestroyLast();
        UpdateInfoOfStorage();
    }
}