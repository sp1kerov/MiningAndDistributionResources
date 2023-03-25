using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] protected Container[] _containers;

    public bool IsEmpty => GetCurrentContainer().IsEmpty;
    public bool IsFull => GetCurrentContainer().IsFull;

    protected virtual Container GetCurrentContainer()
    {
        var container = GetNotFullContainer();
        return container != null ? container : _containers[_containers.Length - 1];
    }

    protected virtual Container GetNotFullContainer()
    {
        foreach (var container in _containers)
        {
            if (!container.IsFull)
            {
                return container;
            }
        }

        return null;
    }
    protected virtual int CapacityStorage()
    {
        int capacity = 0;

        foreach (var container in _containers)
        {
            capacity += container.InventoryCapacity;
        }

        return capacity;
    }

    protected virtual void SortResources()
    {
        if (!IsEmpty)
        {
            foreach (var container in _containers)
            {
                container.SortResources();
            }
        }
    }

    public virtual Resource EndPull(Resource resource)
    {
        if (resource == null)
        {
            return null;
        }

        for (int i = _containers.Length - 1; i >= 0; i--)
        {
            var container = _containers[i];
            var res = container.PullFromEnd(resource);
            SortResources();

            if (res != null)
            {
                return res;
            }
        }

        return null;
    }

    public virtual bool PutResource(Resource resource, float time)
    {
        if (resource == null || IsFull)
        {
            return false;
        }

        GetCurrentContainer().PutResource(resource, time);
        return true;
    }

    public virtual Resource LastPull()
    {
        if (!IsEmpty)
        {
            for (int i = _containers.Length - 1; i >= 0; i--)
            {
                var container = _containers[i];

                if (!container.IsEmpty)
                {
                    var last = container.PullLast();
                    return last;
                }
            }
        }

        return null;
    }

    public virtual int CountResource(Resource resource)
    {
        int count = 0;

        foreach (var container in _containers)
        {
            count += container.CountResource(resource);
        }

        return count;
    }

    public virtual void DestroyLast()
    {
        if (!IsEmpty)
        {
            GetCurrentContainer().DestroyLast();
        }
    }

    public virtual void DestroyFromEnd(Resource resource, int count)
    {
        if (!IsEmpty)
        {
            int needDestroy = count;

            for (int i = _containers.Length - 1; i >= 0; i--)
            {
                var container = _containers[i];
                var available = container.CountResource(resource);

                if (available < needDestroy)
                {
                    needDestroy -= available;
                    container.DestroyFromEnd(resource, needDestroy);
                }
                else
                {
                    container.DestroyFromEnd(resource, needDestroy);
                    break;
                }
            }
        }
    }
}
