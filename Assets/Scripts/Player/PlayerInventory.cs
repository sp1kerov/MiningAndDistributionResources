using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerInventory : Storage
{
    [SerializeField] private float _timeTransmission = 0.3f;

    private float _timerStayInStorage;

    private void OnTriggerStay(Collider other)
    {
        _timerStayInStorage += Time.fixedDeltaTime;

        if (_timerStayInStorage >= _timeTransmission)
        {
            if (other.TryGetComponent(out ResourcePlace storage))
            {
                var building = storage.ParentBuilding;

                if (building != null)
                {
                    if (!storage.IsFull && storage.PlaceUnloading)
                    {
                        Resource resource;

                        foreach (var requiredResource in building.InputResources)
                        {
                            if (storage.CountResource(requiredResource.Resource) < requiredResource.ResourceMaxStoreCount)
                            {
                                resource = EndPull(requiredResource.Resource);

                                if (resource != null)
                                {
                                    building.TakeResource(resource, _timeTransmission);
                                    _timerStayInStorage = 0;
                                    break;
                                }
                            }
                        }
                    }

                    if (!IsFull && storage.CollectPlace)
                    {
                        PutResource(building.GetResource(), _timeTransmission);
                        _timerStayInStorage = 0;
                    }
                }
            }
        }
    }
}
