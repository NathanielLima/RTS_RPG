using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, ISelectable
{
    NavMeshAgent m_agent = null;
    public bool m_selected { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_selected)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray tmpRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit tmpHit;
                if (Physics.Raycast(tmpRay, out tmpHit))
                {
                    m_agent.SetDestination(tmpHit.point);
                }
            }
        }
    }

    public void Select()
    {
        m_selected = true;
    }

    public void Unselect()
    {
        m_selected = false;
    }
}
