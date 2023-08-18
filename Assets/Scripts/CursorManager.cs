using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] public CursorConfig cursorConfig;
    
    // Should always be maintained in sorted order
    [SerializeField] List<int> cursorIndicesStack = new List<int>();

    void Start()
    {
        UpdateCursor();
    }

    // Add the cursor index to cursorIndicesStack by name
    public void EnableCursor(string name) {
        int cursorIndex = GetCursorIndex(name);

        int stackIndex = cursorIndicesStack.FindIndex(ci => ci == cursorIndex);
        if (stackIndex != -1) return;

        // Insert it into the stack in order
        int indexInStack = cursorIndicesStack.BinarySearch(cursorIndex);
        if (indexInStack < 0)
            indexInStack = 0;

        cursorIndicesStack.Insert(indexInStack, cursorIndex);

        // Update the cursor
        UpdateCursor();
    }

    // Remove the cursor index from cursorIndicesStack by name
    public void DisableCursor(string name) {
        int cursorIndex = GetCursorIndex(name);
        cursorIndicesStack.Remove(cursorIndex);

        // Update the cursor
        UpdateCursor();
    }

    // Get the cursor index based on the name
    private int GetCursorIndex(string name) {
        List<CursorEntry> cursors = cursorConfig.cursors;
        int cursorIndex = cursors.FindIndex(cursor => cursor.name == name);
        return cursorIndex;
    }

    // Update the actual cursor based on the cursorIndicesStack
    private void UpdateCursor() {
        int cursorIndex;
        if (cursorIndicesStack.Count > 0) {
            // Only look at the highest element in this list, which is the 
            // index of the enabled cursor with the highest priority
            cursorIndex = cursorIndicesStack[cursorIndicesStack.Count - 1];
        }
        else {
            cursorIndex = 0;
        }
        CursorEntry cursorEntry = cursorConfig.cursors[cursorIndex];
        Cursor.SetCursor(cursorEntry.sprite, cursorEntry.hotspot, CursorMode.ForceSoftware);
    }
}
