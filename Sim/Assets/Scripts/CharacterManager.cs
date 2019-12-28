using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : CharBaseManager<CharPlayer>
{
    public static CharacterManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnChar();
        }
    }
}
