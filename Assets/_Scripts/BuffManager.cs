using System.Collections;
using System.Collections.Generic;
using _Scripts.Collectables;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    private SpeedBuff[] _buffsInScence;
    private SpeedDebuff[] _debuffsInScene;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Find all the buffs / debuffs in the scene
        _buffsInScence = FindObjectsOfType(typeof(SpeedBuff)) as SpeedBuff[];
        _debuffsInScene = FindObjectsOfType(typeof(SpeedDebuff)) as SpeedDebuff[];
    }

    public void RespawnAllCollectables()
    {
        // Active all buffs / debuffs in the scene
        foreach (var buff in _buffsInScence)
            if (!buff.gameObject.activeSelf)
                buff.gameObject.SetActive(true);
        
        foreach (var debuff in _debuffsInScene)
            if (!debuff.gameObject.activeSelf)
                debuff.gameObject.SetActive(true);
    }
}
