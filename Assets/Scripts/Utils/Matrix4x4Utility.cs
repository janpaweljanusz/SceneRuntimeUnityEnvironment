using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix4x4Utility
{
    public static Matrix4x4 Matrix4X4FromJson(Globals.Transform gTransform)
    {
        var pos = new Vector3(gTransform.position[0], gTransform.position[1], gTransform.position[2]);
        var rotation = new Quaternion(gTransform.rotation[0], gTransform.rotation[1], gTransform.rotation[2], gTransform.rotation[3]);
        var scale = new Vector3(gTransform.scale[0], gTransform.scale[1], gTransform.scale[2]);
        return Matrix4x4.TRS(pos, rotation, scale);
    }
}
