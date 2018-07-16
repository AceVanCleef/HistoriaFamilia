using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDebugTool {

	private Stack<Dictionary<Vector3, Vector3>> DrawLineStack = new Stack<Dictionary<Vector3, Vector3>>();

	public void AddLine(Vector3 start, Vector3 end) {
		Dictionary<Vector3, Vector3> line = new Dictionary<Vector3, Vector3>();
		line.Add(start, end);
		DrawLineStack.Push(line);
	}

	public void EmptyDrawLineStack() {
		DrawLineStack.Clear();
	}
	

	public IEnumerator PrintDebugStack(Color c)
	{
		float duration = 2.0f * DrawLineStack.Count;
		Debug.Log("------ Printing " + DrawLineStack.Count + " lines -----");
        do
        {
            Dictionary<Vector3, Vector3> line = DrawLineStack.Pop();
			foreach( Vector3 key in line.Keys) {
				DrawDebugLine(key, line[key], c, duration);
			}
            yield return new WaitForSeconds(1);
        } while (DrawLineStack.Count > 0);
	}

	private void  DrawDebugLine(Vector3 start, Vector3 end, Color c,  float duration) {
			Debug.DrawLine( start, end, c, duration, false);
			Debug.Log("Drawing from " + start + " to " + end);
	}
}
