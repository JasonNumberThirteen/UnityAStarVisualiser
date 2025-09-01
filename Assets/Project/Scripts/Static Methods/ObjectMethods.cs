using UnityEngine;

public static class ObjectMethods
{
	public static T FindComponentOfType<T>(bool includeInactive = true) where T : Component => Object.FindAnyObjectByType<T>(GetFindObjectsInactive(includeInactive));
	public static T[] FindComponentsOfType<T>(bool includeInactive = true) where T : Component => Object.FindObjectsByType<T>(GetFindObjectsInactive(includeInactive), FindObjectsSortMode.None);

	private static FindObjectsInactive GetFindObjectsInactive(bool includeInactive) => includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
}