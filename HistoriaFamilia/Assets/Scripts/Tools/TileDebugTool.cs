using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDebugTool {

	private Queue<Dictionary<Vector3, Vector3>> DrawLineStack = new Queue<Dictionary<Vector3, Vector3>>();
	private float VisibilityDuration = 60.0f;

	public void AddLine(Vector3 start, Vector3 end) {
		Dictionary<Vector3, Vector3> line = new Dictionary<Vector3, Vector3>();
		line.Add(start, end);
		DrawLineStack.Enqueue(line);
	}

	public void EmptyDrawLineStack() {
		DrawLineStack.Clear();
	}

	public void SetLineVisibilityDurationTo(float visibilityDuration) {
		VisibilityDuration = VisibilityDuration;
	}
	

	public IEnumerator PrintDebugStack(Color c)
	{
		float duration = 2.0f * DrawLineStack.Count;
		Debug.Log("------ Printing " + DrawLineStack.Count + " lines -----");
        do
        {
            Dictionary<Vector3, Vector3> line = DrawLineStack.Dequeue();
			foreach( Vector3 key in line.Keys) {
				DrawDebugLine(key, line[key], c);
			}
            yield return new WaitForSeconds(1);
        } while (DrawLineStack.Count > 0);
	}

	private void  DrawDebugLine(Vector3 start, Vector3 end, Color c) {
			Debug.DrawLine( start, end, c, VisibilityDuration, false);
			Debug.Log("Drawing from " + start + " to " + end);
	}
}
