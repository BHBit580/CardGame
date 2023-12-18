using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ListGameObjectSO")]
public class ListGameObjectSO : ScriptableObject
{
    public List<GameObject> data = new ();
}
