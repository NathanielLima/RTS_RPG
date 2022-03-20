using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    bool m_selected { get; set; }

    void Select();
    void Unselect();
}
