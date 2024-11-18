﻿using UnityEngine;
namespace Canons
{
    public class CanonView : MonoBehaviour
    {

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

    }
}
