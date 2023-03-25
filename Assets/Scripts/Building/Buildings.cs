using System.Collections;
using System.Linq;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    [Header("Plant")]
    [SerializeField] private float _timeDelayProduction;
    [Header("Input Resource")]
    [SerializeField] private ResourcePlace _storageResourceInput;
    [SerializeField] private InputResource[] _inputResource;
    [Header("Output Resource")]
    [SerializeField] private ResourcePlace _storageoutputResource;
    [SerializeField] private Resource _outputResource;

    public InputResource[] InputResources => _inputResource;
    public Resource OutputResource => _outputResource;

    private void Start()
    {
        CheckProduction();
    }

    private void CheckProduction()
    {
        if (_outputResource != null)
        {
            if (_inputResource.Length > 0)
            {
                StartCoroutine(ProduceResources());
            }
            else
            {
                StartCoroutine(SpawnResources());
            }
        }
    }

    IEnumerator ProduceResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeDelayProduction);

            if (!_storageResourceInput.IsEmpty && !_storageoutputResource.IsFull)
            {
                bool resourcesAvailable = false;
                foreach (var requiredResource in _inputResource)
                {
                    resourcesAvailable = _storageResourceInput.CountResource(requiredResource.Resource) >= requiredResource.RequiredCount;

                    if (!resourcesAvailable)
                        break;
                }

                if (resourcesAvailable)
                {
                    foreach (var requiredResource in _inputResource)
                    {
                        _storageResourceInput.DestroyFromEnd(requiredResource.Resource, requiredResource.RequiredCount);
                    }
                    _storageoutputResource.PutResource(Instantiate(_outputResource), 0f);
                }
            }
        }
    }

    IEnumerator SpawnResources()
    {
        while (true)
        {
            if (!_storageoutputResource.IsFull)
            {
                _storageoutputResource.PutResource(Instantiate(_outputResource), 0f);
            }

            yield return new WaitForSeconds(_timeDelayProduction);
        }
    }

    public Resource GetResource()
    {
        return _storageoutputResource != null ? _storageoutputResource.LastPull() : null;
    }

    public bool TakeResource(Resource resource, float time)
    {
        if (_storageResourceInput != null)
        {
            if (_storageResourceInput.PutResource(resource, time))
            {
                return true;
            }
        }

        return false;
    }


    [System.Serializable]
    public struct InputResource
    {
        public Resource Resource;
        public int RequiredCount;
        public int ResourceMaxStoreCount;
    }
}
