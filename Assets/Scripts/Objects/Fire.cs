using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire is the script controlling a fire when that a lantern spawns
/// </summary>
public class Fire : MonoBehaviour
{

    #region Attributes
    [SerializeField]
    private float duration;
    #endregion

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Ignite should be called whenever a fire is spawned, it should
    /// take in the gameObject representing the platform this fire will be
    /// a child of
    /// </summary>
    /// <param name="platform">platform that the fire is on</param>
    public void Ignite(GameObject platform, float y)
    {
        // set parent
        transform.parent = platform.transform;
        transform.position = new Vector3(platform.transform.position.x, y, platform.transform.position.z);
    }
}
