using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITemplate : MonoBehaviour
{
    [SerializeField] UIPlayerTemplate playerRight;
    [SerializeField] UIPlayerTemplate playerLeft;

}

public class UIPlayerTemplate : MonoBehaviour
{
    [SerializeField] GameObject playerPos;

}
public class UIUnitTemplate : MonoBehaviour
{
    [SerializeField] GameObject unitPos;
    [SerializeField] Row row;
    [SerializeField] Column column;
}