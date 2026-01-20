using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GhostDatabase", menuName = "Game Data/Ghost Database")]
public class GhostDatabase : ScriptableObject
{
    public List<GhostData> allGhosts;
}
