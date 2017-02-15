using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : BasePirate {
    #region Attributes
    #endregion
    
    #region Properties
    #endregion
    
    // Use this for initialization
    protected override void Start () {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region Methods
    protected override void Dead()
    {
        Debug.Log("Pirate: " + name + "has Died");
        Destroy(gameObject);
    }
    #endregion
}
