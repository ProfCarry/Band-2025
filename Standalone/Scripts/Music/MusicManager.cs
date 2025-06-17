using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Band.Music
{
    [RequireComponent (typeof (AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            DontDestroyOnLoad (this.gameObject);
            List<MusicManager> managers = GameObject.FindObjectsByType<MusicManager>(FindObjectsSortMode.InstanceID).ToList<MusicManager>();
            managers=managers.Where((manager) => manager != this).ToList();
            foreach (MusicManager manager in managers)
                Destroy(manager);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
