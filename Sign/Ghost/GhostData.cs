using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[CreateAssetMenu(fileName = "GhostData", menuName = "Game Data/Ghost Data")]

public class GhostData : ScriptableObject
{
    public string ghostName;
    public string[] answer;
    public string ghostType; //type 1 º’¥’ type 2 º’‡≈«¡’§”„∫È type 3 º’‡≈«‰¡Ë¡’§”„∫È
    public int ghostDay;
    public int ghostRealDay;
    public int ghostStage;
    public bool isChoose;
    public bool isShow;
    public GameObject ghostPrefab;
    public Sprite ghostSprite;
    public Sprite jmpScareSprite;
}
