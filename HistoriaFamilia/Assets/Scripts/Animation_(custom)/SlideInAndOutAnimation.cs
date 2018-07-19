using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInAndOutAnimation : MonoBehaviour {

	public Vector3 StartPosition;
	public Vector3 DisplayDestination;
	public Vector3 EndPosition;

	private bool _animate = false;
	// How many seconds should the movement transition last?
	public float SlideInSpeed = 0.1f;
	public float SlideOutSpeed = 0.05f;
	public float WaitDuration = 0.5f;

	private Transform t;

	void Start() {
		if ( SlideInSpeed != 0f && StartPosition.Equals(DisplayDestination) ) {
			Debug.LogError("No sliding in possible. StartPosition and DisplayDestination are identical.");
		}
		if ( SlideOutSpeed != 0f && DisplayDestination.Equals(EndPosition) ) {
			Debug.LogError("No sliding out possible. EndPosition and DisplayDestination are identical.");
		}
	}
	
	void Update () {
		ManageAnimation();
	}

	public void Animate(Transform tf) {
		t = tf;
		t.position = StartPosition;
		_animate = true;
	}

	private bool _slideIn = true;
	private bool _wait = false;
	private bool _slideOut = false;

	private void ManageAnimation() {
		//source: https://www.gamasutra.com/blogs/AlexRose/20130905/199662/Animation_in_2D_Unity_Games_InDepth_Starter_Guide.php
		if (_animate) {
			if ( _slideIn ) {
				t.position += (DisplayDestination - t.position) * SlideInSpeed;
				if (HasReachedDisplayDestination() ) {
					_slideIn = false;
					_wait = true;
				}
			} 

			if (_wait) {
				StartCoroutine( SlideOutAfter( WaitDuration ) );
			}
			
			if ( _slideOut ) {
				t.position += (EndPosition - t.position) * SlideOutSpeed;

				if ( HasReachedEndPosition() ) {
					_animate = false;
					_slideOut = false;
					_slideIn = true;
					//t.position = StartPosition;
				}
			}
		}
	}

	private bool HasReachedDisplayDestination() {
		return Vector3.Distance(t.position, DisplayDestination) < 0.001f;
	}

	private bool HasReachedEndPosition() {
		return Vector3.Distance(t.position, EndPosition) < 0.001f;
	}

	private IEnumerator  SlideOutAfter(float seconds) {
		_wait = false;	//prevent subsequent call of this coroutine.
		//pause Coroutine for x seconds.
        yield return new WaitForSeconds(seconds);
		_slideOut = true;

		/*source: https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
		*	"StartCoroutine function always returns immediately, however you can yield the result. This 
		*	will wait until the coroutine has finished execution."
		*
		*	[IEnumerator - C#'s Iterator]
		*	'private IEnumerator ...()':
		*	The return type is basically an iterator (Java). We can say: Iterator (Java) == IEnumerator (C#)
		*
		*	ienumerator.Current returns the current item.
		*	ienumerator.MoveNext() moves to the next item and returns true if it moved forward sucessfully.
		*	There is also a ienumerator.Reset() method, which sets the iterator to its original position, right
		*	infront of the first element in the collection.
		*/
	}
}
