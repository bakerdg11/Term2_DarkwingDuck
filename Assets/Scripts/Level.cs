using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform levelStart;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SpawnPlayer(levelStart);
    }

}