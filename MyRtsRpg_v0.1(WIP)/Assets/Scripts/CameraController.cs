using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float m_speed = 30.0f;
    bool m_isLerping = false;
    Vector3 m_startPos = Vector3.zero;
    Vector3 m_endPos = Vector3.zero;
    float m_lerpCurrentTime = 0.0f;
    float m_lerpMaxTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_lerpCurrentTime += Time.deltaTime;
        if (m_isLerping)
        {
            float ratio = m_lerpCurrentTime / m_lerpMaxTime;
            transform.position = Vector3.Lerp(m_startPos, m_endPos, ratio >= 1.0f ? 1.0f : ratio);
            m_isLerping = ratio >= 1.0f ? false : true;
            return;
        }
        transform.Translate(Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * m_speed, Space.World);
        transform.Translate(Vector3.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime * m_speed, Space.World);
    }

    public void Search(Vector3 _position)
    {
        m_startPos = transform.position;
        m_endPos = _position - 5 * Vector3.forward;
        m_endPos.y = 10.0f;
        m_isLerping = true;
        m_lerpCurrentTime = 0.0f;
    }
}
