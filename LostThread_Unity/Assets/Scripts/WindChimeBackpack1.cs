// 卡槽版
//using UnityEngine;

//public class WindChimeBackpack1 : MonoBehaviour
//{
//    [Header("Attach Points")]
//    public Transform[] attachPoints;

//    public GameObject FindAttachedChime(int melodyID)
//    {
//        foreach (Transform slot in attachPoints)
//        {
//            if (slot.childCount > 0)
//            {
//                Transform chime = slot.GetChild(0);
//                WindChimeData1 data = chime.GetComponentInChildren<WindChimeData1>();
//                if (data != null && data.melodyID == melodyID)
//                {
//                    return chime.gameObject;
//                }
//            }
//        }
//        return null;
//    }
//}


//全挂一个版

using UnityEngine;

public class WindChimeBackpack1 : MonoBehaviour
{
    [Header("Attach Points (container for all chimes)")]
    public Transform attachPoints; // 一个Transform，挂所有风铃

    public GameObject FindAttachedChime(int melodyID)
    {
        foreach (Transform chime in attachPoints)
        {
            WindChimeData1 data = chime.GetComponentInChildren<WindChimeData1>();
            if (data != null && data.melodyID == melodyID)
            {
                return chime.gameObject;
            }
        }
        return null;
    }
}

