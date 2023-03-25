using System;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private Transform[] _positionItem = new Transform[0];
    private int _inventoryCapacity;
    private List<Resource> _listResources = new List<Resource>();

    public Transform[] TransformsDocking => _positionItem;

    public int CountResources => _listResources.Count;
    public int InventoryCapacity => _inventoryCapacity;
    public bool IsFull => _listResources.Count == _inventoryCapacity;
    public bool IsEmpty => _listResources.Count == 0;

    private void Start()
    {
        _inventoryCapacity = transform.childCount;
        _positionItem = new Transform[_inventoryCapacity];

        for (int i = 0; i < _inventoryCapacity; i++)
        {
            _positionItem[i] = transform.GetChild(i);
        }
    }

    public int CountResource(Resource resource)
    {
        int count = 0;

        foreach (var res in _listResources)
        {
            if (res.GetType().Equals(resource.GetType()))
            {
                count++;
            }
        }

        return count;
    }

    public void SortResources()
    {
        for (int i = 0; i < _listResources.Count; i++)
        {
            ResourcesPlace(_listResources[i], i, 0f);
        }
    }

    private void ResourcesPlace(Resource resource, int dockIndex, float time)
    {
        resource.transform.SetParent(_positionItem[dockIndex], true);
        resource.MoveToLocalParent(time);
    }

    public bool PutResource(Resource resource, float time)
    {
        if (_listResources.Count < _inventoryCapacity)
        {
            _listResources.Add(resource);
            ResourcesPlace(resource, CountResources - 1, time);
            return true;
        }

        return false;
    }

    public Resource PullFromEnd(Resource resource)
    {
        for (int i = _listResources.Count - 1; i >= 0; i--)
        {
            var res = _listResources[i];
            if (res.GetType().Equals(resource.GetType()))
            {
                res.transform.parent = null;
                _listResources.Remove(res);
                return res;
            }
        }

        return null;
    }

    public Resource PullLast()
    {
        var resource = _listResources[CountResources - 1];
        resource.transform.parent = null;
        _listResources.Remove(resource);
        return resource;
    }

    public void DestroyLast()
    {
        Resource resource;

        for (int i = _listResources.Count - 1; i >= 0; i--)
        {
            resource = _listResources[i];

            if (resource != null)
            {
                Destroy(resource.gameObject);
                _listResources.RemoveAt(i);
                break;
            }
        }
    }

    public void DestroyFromEnd(Resource resourceToDestroy, int count)
    {
        Resource resource;
        int destroyed = 0;

        for (int resIndex = _listResources.Count - 1; ( resIndex >= 0 && destroyed < count ); resIndex--)
        {
            resource = _listResources[resIndex];

            if (resourceToDestroy != null && resource.GetType().Equals(resourceToDestroy.GetType()))
            {
                Destroy(resource.gameObject);
                _listResources.RemoveAt(resIndex);
                destroyed++;
            }
        }
    }
}
