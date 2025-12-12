using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

using Band.Utils;

public abstract class LevelGraph : MonoBehaviour, IController
{
    private List<LevelRoom> rooms;
    private List<LevelPassage> passages;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rooms=GameObject.FindObjectsByType<LevelRoom>(FindObjectsSortMode.InstanceID).ToList<LevelRoom>();
        passages=GameObject.FindObjectsByType<LevelPassage>(FindObjectsSortMode.InstanceID).ToList<LevelPassage>();
        foreach (LevelRoom room in rooms)
        {
            room.OnEnter.AddListener(OnCharacterEnterRoom);
            room.OnExit.AddListener(OnCharacterExitRoom);
        }
        foreach (LevelPassage passage in passages)
            passage.OnPass.AddListener(OnCharacterPassed);
    }

    protected abstract void OnCharacterPassed(int start, int end);
    protected abstract void OnCharacterEnterRoom(int roomId);
    protected abstract void OnCharacterExitRoom(int roomId);

    public LevelRoom GetRoomById(int roomId)
    {
        return rooms.Where(room => room.Id == roomId).FirstOrDefault();
    }

    public List<LevelPassage> GetPassagesFromRoomId(int roomId)
    {
        return passages.Where(passage=>passage.StartRoomId == roomId).ToList();
    }

    public void Clear()
    {
        Start();
    }
}
