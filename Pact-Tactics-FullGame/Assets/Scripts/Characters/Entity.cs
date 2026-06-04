using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour { }

public interface IEntity
{
    public string characterName { get; set; }

    public string characterBio { get; set; }

}
