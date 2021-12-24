using UnityEngine;

public class ObjectLogic : MonoBehaviour
{
    public bool respawn;
    public Arena myArena;
    
    public void OnEaten(){
    
    	if(respawn)
    	{
    		transform.position = new Vector3(
	            Random.Range(-myArena.range, myArena.range),
    			0f,
    			Random.Range(-myArena.range, myArena.range)) + myArena.transform.position;
    	}
    	else{
    		Destroy(gameObject);
    	}
    }
}
