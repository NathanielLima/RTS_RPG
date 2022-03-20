using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject m_visualSelection = null;
    [SerializeField] GameObject[] m_UIUnits = new GameObject[10];
    [SerializeField] GameObject m_UISelectedFace = null;
    [SerializeField] GameObject m_UISelectedActions = null;
    [SerializeField] List<Unit> m_units = new List<Unit>();

    EPlayerMode m_playerMode = EPlayerMode.MOVE;
    List<ISelectable> m_selection = new List<ISelectable>();
    Vector3 m_startSelect = Vector3.zero;
    Vector3 m_endSelect = Vector3.zero;

    int m_UISelected = -1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_units.Count; ++i)
        {
            m_UIUnits[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_playerMode)
        {
            case EPlayerMode.SELECT:
                Select();
                break;
            default:
                break;
        }
    }

    void Select()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            m_startSelect = Input.mousePosition;
            m_endSelect = m_startSelect;
            m_visualSelection?.SetActive(true);
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            m_endSelect = Input.mousePosition;
            if (m_visualSelection)
            {
                RectTransform transform = m_visualSelection.GetComponent<RectTransform>();
                Vector3 pos = (m_startSelect + m_endSelect) / 2;
                Vector3 size = m_endSelect - m_startSelect;
                size.x = Mathf.Abs(size.x);
                size.y = Mathf.Abs(size.y);
                transform.position = pos;
                transform.sizeDelta = size;
            }
        }
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var selection = FindObjectsOfType<MonoBehaviour>().OfType<ISelectable>();
            foreach(ISelectable current in selection)
            {
                MonoBehaviour monoBehaviour = current as MonoBehaviour;
                Vector3 position = Camera.main.WorldToScreenPoint(monoBehaviour.transform.position);
                Vector3 start;
                Vector3 end;
                start.x = m_startSelect.x < m_endSelect.x ? m_startSelect.x : m_endSelect.x;
                start.y = m_startSelect.y < m_endSelect.y ? m_startSelect.y : m_endSelect.y;
                end.x = m_startSelect.x < m_endSelect.x ? m_endSelect.x : m_startSelect.x;
                end.y = m_startSelect.y < m_endSelect.y ? m_endSelect.y : m_startSelect.y;
                if (position.x >= start.x && position.x <= end.x && position.y >= start.y && position.y <= end.y)
                {
                    current.Select();
                    m_selection.Add(current);
                }
            }
            m_playerMode = EPlayerMode.MOVE;
            m_visualSelection?.SetActive(false);
        }
    }

    void Unselect()
    {
        foreach (ISelectable current in m_selection)
        {
            current.Unselect();
        }
        m_selection.Clear();
    }

    public void UISelect(int _index)
    {
        m_UISelected = _index;
        m_UISelectedFace.SetActive(true);
        m_UISelectedActions.SetActive(true);
    }

    public void UIActionType(int _index)
    {
        for (int i = 0; i < m_UISelectedActions.transform.childCount; i++)
        {
            m_UISelectedActions.transform.GetChild(i).gameObject.SetActive(false);
        }
        m_UISelectedActions.transform.GetChild(_index).gameObject.SetActive(true);
    }

    public void SearchSelected()
    {
        if (m_UISelected != -1)
        {
            Camera.main.GetComponent<CameraController>().Search(m_units[m_UISelected].transform.position);
        }
    }

    public void SetSelectMode()
    {
        Unselect();
        m_playerMode = EPlayerMode.SELECT;
    }
}
