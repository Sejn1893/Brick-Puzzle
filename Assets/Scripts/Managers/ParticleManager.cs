using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    public GameObject GlowFx;
    public GameObject GlowFxDestroy;
    
    
    Spawner _spawner;
    // Start is called before the first frame update
    void Start()
    {
        
        _spawner = FindObjectOfType<Spawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
   
    public void PlayGlowFx(GameObject activeShape)
    {

        List<Transform> children = GetChildren(activeShape);
        children.RemoveAt(0);
     
        foreach (var item in children)
        {
            
            
            GameObject tmpGlowFx = Instantiate(GlowFx, item.transform.position, item.transform.rotation);

            //tmpGlowFx.transform.position = new Vector3(item.transform.position.x -.3f, item.transform.position.y, -5f);
            Destroy(tmpGlowFx, 0.6f);
        }
    }



    public void PlayGlowFxDestroy(GameObject activeShape)
    {
        List<Transform> children = GetChildren(activeShape);
        //children[0] = null;
        foreach (var item in children)
        {
            
            GameObject tmpFx = Instantiate(GlowFxDestroy, item.transform.position, item.transform.rotation);

            Destroy(tmpFx, 0.8f);
        }
    }
    
    public List<Transform> GetChildren(GameObject activeShape)
    {
        List<Transform> tmpTrans = new List<Transform>(activeShape.GetComponentsInChildren<Transform>());
        return tmpTrans;
    }
}
