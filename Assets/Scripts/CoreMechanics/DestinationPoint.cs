using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    public GameTile _DestPrefab { get; private set; }
    //public GameTileContentType destination { get; private set; }

    public Vector3 Position => transform.position;

    public float ColliderSize { get; private set; }
    private const int DESTINATION_LAYER_MASK = 1 << 6; 

    private static Collider[] _buffer = new Collider[100];
    public static int BufferedCount { get; private set; }

    private void Awake()
    {
        _DestPrefab = transform.root.GetComponent<GameTile>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }
    public static bool FillBuffer(Vector3 position, float range)
    {
        Vector3 top = position;
        top.y += 0f;
        BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _buffer, DESTINATION_LAYER_MASK);
        return BufferedCount > 0;
    }
    public static DestinationPoint GetBuffered(int index)
    {
        var target = _buffer[index].GetComponent<DestinationPoint>();
        return target;
    }
}
