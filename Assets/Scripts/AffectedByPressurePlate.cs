using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AffectedByPressurePlate : MonoBehaviour
{
    /// <summary>
    /// Ativa quando a pressure plate ou alavanca é pressionado.
    /// </summary>
    public abstract void OnPressed();
}