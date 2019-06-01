// C#
// MouseHelper.cs
using UnityEngine;
using System.Collections;

public static class MouseHelper
{
    private static Vector2 m_CurrentMousePos;
    private static Vector2 m_LastMousePos;
    private static Vector2 m_CurrentDelta;
    private static Vector2[] m_DragStartVector = new Vector2[3];
    private static Vector2[] m_DragVector = new Vector2[3];
    private static int m_LastFrame = -1;

    public static Vector2 mousePosition { get { Update(); return m_CurrentMousePos; } }
    public static Vector2 lastMousePosition { get { Update(); return m_LastMousePos; } }
    public static Vector2 mouseDelta { get { Update(); return m_CurrentDelta; } }
    public static Vector2 GetDragStartPoint(int aIndex) { Update(); return m_DragStartVector[aIndex]; }
    public static Vector2 GetDragOffset(int aIndex) { Update(); return m_DragVector[aIndex]; }

    static MouseHelper()
    {
        // force initialization on first access
        m_CurrentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Update();
        m_LastFrame = -1;
    }

    static void Update()
    {
        if (m_LastFrame >= Time.frameCount)
            return;
        if (m_LastFrame < Time.frameCount - 1)
            m_CurrentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_LastFrame = Time.frameCount;
        m_LastMousePos = m_CurrentMousePos;
        m_CurrentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_CurrentDelta = m_CurrentMousePos - m_LastMousePos;
        for (int i = 0; i < m_DragStartVector.Length; i++)
        {
            if (Input.GetMouseButtonDown(i))
                m_DragStartVector[i] = m_CurrentMousePos;
            if (Input.GetMouseButton(i))
                m_DragVector[i] = m_CurrentMousePos - m_DragStartVector[i];
        }
    }
}