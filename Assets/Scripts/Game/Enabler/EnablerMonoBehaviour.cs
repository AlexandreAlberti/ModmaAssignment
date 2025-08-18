using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    public class EnablerMonoBehaviour : MonoBehaviour
    {
        protected bool _isEnabled;

        public virtual void Enable()
        {
            _isEnabled = true;
        }

        public virtual void Disable()
        {
            _isEnabled = false;
        }
    }
}