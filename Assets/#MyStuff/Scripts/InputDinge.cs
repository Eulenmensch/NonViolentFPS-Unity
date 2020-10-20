using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDinge : MonoBehaviour
{
    void Update()
    {
        RaycastWorldUI();
    }

    void RaycastWorldUI()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            PointerEventData pointerData = new PointerEventData( EventSystem.current );

            pointerData.position = Input.mousePosition;

            Debug.Log( pointerData.position );

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll( pointerData, results );

            if ( results.Count > 0 )
            {
                Debug.Log( "boop" );
                //WorldUI is my layer name
                if ( results[0].gameObject.layer == LayerMask.NameToLayer( "WorldUI" ) )
                {
                    string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                    Debug.Log( string.Format( dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name ) );
                    //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                    //Debug.Log("GrandChild Element: "+results[0].gameObject.name);
                    results.Clear();
                }
            }
        }
    }
}