using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager dataManager;
    private EventSystemController _EventSystemController;

    public DataContainer dataContainer;

    private void Awake()
    {
        if (dataManager == null)
            dataManager = this;
    }

    private void Start()
    {
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onOpenDoor += (door) => SaveData<Door>(dataContainer.doorData, door);
        _EventSystemController.onRestart += RestartDoor;
    }

    public void SaveData<t>(List<t> dataList, t data)
    {
        if (dataList.Contains(data))
            return;

        dataList.Add(data);
        Debug.Log($"Data has been saved: {data.GetType()}");
    }

    private List<t> GetData<t>(List<t> dataList, bool criteria)
    {
        List<t> resultData = new List<t>();

        foreach (t _Data in dataList)
        {
            if (criteria)
                resultData.Add(_Data);
        }

        return resultData;
    }

    public void DeleteData<t>(List<t> dataList, t data)
    {
        if (!dataList.Contains(data))
            return;

        dataList.Remove(data);
        Debug.Log($"Data has been removed: {data.GetType()}");
    }

    private void RestartDoor()
    {
        foreach (Door door in dataContainer.doorData)
        {
            door.gameObject.SetActive(false);
        }
    }
}
