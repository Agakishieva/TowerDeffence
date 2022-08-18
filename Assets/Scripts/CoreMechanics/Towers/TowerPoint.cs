using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    public Tower Tower { get; private set; }

    public Vector3 Position => transform.position;

    public float ColliderSize { get; private set; }
    private const int TOWER_LAYER_MASK = 1 << 8; //????????? ????? ?? 9 ?????????

    private static Collider[] _buffer = new Collider[100];
    public static int BufferedCount { get; private set; }

    private void Awake()
    {
        Tower = transform.root.GetComponent<Tower>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }
    public static bool FillBuffer(Vector3 position, float range)
    {
        Vector3 top = position;
        top.y += 0f;
        BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _buffer, TOWER_LAYER_MASK);
        return BufferedCount > 0;
    }
    public static TowerPoint GetBuffered(int index)
    {
        var target = _buffer[index].GetComponent<TowerPoint>();
        return target;
    }
}
