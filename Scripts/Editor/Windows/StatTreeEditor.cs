using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class StatTreeEditor : EditorWindow
{

    List<Node> nodes = new List<Node>();
    GUIStyle nodeStyle;

    [MenuItem("Window/Stat Tree Editor")]
    static void OpenWindow()
    {
        var window = GetWindow<StatTreeEditor>();
        window.titleContent = new GUIContent("Stat Tree Editor");
    }

    void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.alignment = TextAnchor.MiddleCenter;
        nodeStyle.border = new RectOffset(10, 10, 10, 10);
    }

    void OnGUI()
    {
        Draw();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed)
            Repaint();
    }


    void Draw()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Draw();
        }
    }

    void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ProcessMenu(e.mousePosition);
                }
                break;
            default:
                break;
        }
    }

    void ProcessNodeEvents(Event e)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].ProcessEvents(e);
        }
    }

    void ProcessMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add node"), false, () => AddNode(mousePos));
        menu.ShowAsContext();
    }

    void AddNode(Vector2 mousePos)
    {
        nodes.Add(new Node(mousePos, 200, 50, nodeStyle));
    }

}

public class Node
{
    string nodeName;
    public string NodeName{ get { return nodeName; } }
    List<Node> parents = new List<Node>();
    public Rect rect;
    Rect textRect;
    bool isDragged;
    bool drawBezier;
    Vector2 bezierStart;

    public GUIStyle style;
    GUIStyle textStyle;

    ConnPoint topPoint;
    ConnPoint botPoint;

    public Node(Vector2 pos, float width, float height, GUIStyle style)
    {
        rect = new Rect(pos.x, pos.y, width, height);
        textRect = new Rect();
        textRect.size = rect.size / 2f;
        textRect.center = rect.center;
        textStyle = new GUIStyle();
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.overflow = new RectOffset(20, 20, 0, 0);
        textStyle.stretchWidth = true;
        textStyle.border = new RectOffset(20, 20, 0, 0);
        textStyle.normal.textColor = Color.white;
        this.style = style;
        topPoint = new ConnPoint(ConnPoint.Kind.Child, OnClickTopPoint);
        botPoint = new ConnPoint(ConnPoint.Kind.Parent, OnClickBotPoint);

    }

    void OnClickTopPoint()
    {
        drawBezier = true;
    }
    void OnClickBotPoint()
    {

    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        textRect.position += delta;
    }

    public void Draw()//udělat měnitelný text
    {
        GUI.Box(rect, "", style);
        nodeName = GUI.TextField(textRect, nodeName, textStyle);
        if(drawBezier)
            Handles.DrawBezier(bezierStart, Event.current.mousePosition, Vector2.up, Vector2.down, Color.white, null, 2f);
        topPoint.Draw(rect);
        botPoint.Draw(rect);

    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                    }

                    break;
                }
            case EventType.mouseUp:
                isDragged = false;
                break;
            case EventType.MouseDrag:
                {
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        GUI.changed = true;
                        return true;
                    }
                    break;
                }


        }

        return false;
    }
}

public class ConnPoint
{
    public enum Kind { Parent, Child }
    public Kind kind;
    Rect rect;
    static GUIStyle style;
    Action onClick;

    static ConnPoint()
    {
        style = new GUIStyle();
        style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        style.border = new RectOffset(4, 4, 4, 4);
    }

    public ConnPoint(Kind k, Action onClick)
    {
        kind = k;
        this.onClick = onClick;
        rect = new Rect(0, 0, 100, 10);
    }

    public void Draw(Rect nodeRect)
    {
        rect.center = nodeRect.center;
        switch (kind)
        {
            case Kind.Parent:
                rect.y += 20;
                break;
            case Kind.Child:
                rect.y -= 20;
                break;
            default:
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            onClick?.Invoke();
        }
    }
}