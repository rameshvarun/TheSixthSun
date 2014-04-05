using UnityEngine;
using System.Collections;

public class UnitBehavior : MonoBehaviour
{
	public virtual Vector3 getInspectCameraPosition() {
		throw new System.NotImplementedException();
	}

	public virtual bool MoveGUI() {
		throw new System.NotImplementedException();
	}

	/// <summary>
	/// This is called by the CameraControls script when focus has been removed from the object.
	/// This method should be used to clean up move selectors, destroy GUI's, and other actions that
	/// should be reversed when the object is no longer selected
	/// </summary>
	public virtual void cleanUp() {
		throw new System.NotImplementedException();
	}
}

