using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour {

    public AudioSource audioSource;
    public GameObject ocean;
    public GameObject wall;
    public GameObject head;
    public GameObject boat;
    public Light[] lights = new Light[2];

    // Update is called once per frame
    public delegate bool Func(Logic logic);
    static bool end = false;
    static bool stepDone = true;
    Func[] steps =  {           START ,
                                PLAY_AMBIENT_AUDIO,
                                SHOW_OCEAN,
                                LIGHTS_ON,
                                WALL_UP,
                                NAVIGATE,
                                END };
    /*,                           
                                SHOW_TITLE,
                                SHOW_INTRO_TITLE,
                                PLAY_INTRO_NARRATIVE,
                                
                                
                                PLAY_CONTEXT_AUDIO,
                                SHOW_BOAT,
                                PLAY_BOAT_AUDIO,
                                
                                END};
    
    */
    float[] timing = { 3, 3, 3, 3, 5, 4, 3, 2, 1, 2,3,4,5,6};
    float lastTime = 0;
    int currentStep = 0;
    
	void Update () {
        if ( ! end )
            if ( stepDone )
                if ( Time.realtimeSinceStartup > lastTime + timing[currentStep])
                {
                    stepDone = false;
                    if (steps[currentStep] == END)
                        end = true;
                    Debug.Log("Time: " + Time.realtimeSinceStartup+",   Duration:"+timing[currentStep]);
                    steps[currentStep++](this);
                    lastTime = Time.realtimeSinceStartup;             
                }        
    }

    static bool START(Logic go)
    {
        Debug.Log("STEP START");
        stepDone = true;
        go.lights[0].intensity = 0.0f;
        return false;
    }
    
    static bool PLAY_AMBIENT_AUDIO(Logic go)
    {

        Debug.Log("STEP PLAY_AMBIENT_AUDIO");
        if (go.audioSource)
            go.audioSource.Play();
        stepDone = true;

        return false;
    }
    static bool SHOW_TITLE(Logic go)
    {
        Debug.Log("STEP SHOW_TITLE");
        return false;
    }
    static bool SHOW_INTRO_TITLE(Logic go)
    {
        Debug.Log("STEP SHOW_INTRO_TITLE");
        return false;
    }
    static bool PLAY_INTRO_NARRATIVE(Logic go)
    {
        Debug.Log("STEP PLAY_INTRO_NARRATIVE");
        return false;
    }
    static bool LIGHTS_ON(Logic go)
    {
        Debug.Log("STEP LIGHTS_ON");
        go.StartCoroutine(go.lights_on());
        return false;
    }
    IEnumerator lights_on()
    {
        float intensity = 0.0f;
        while (intensity < 1.0f)
        {
            lights[0].intensity = intensity;
            intensity += 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
        stepDone = true;
    }

    static bool SHOW_OCEAN(Logic go)
    {
        Debug.Log("STEP SHOW_OCEAN");
        if (go.ocean)
            go.ocean.SetActive(true);
        stepDone = true;
        return false;
    }
    static bool PLAY_CONTEXT_AUDIO(Logic go)
    {
        Debug.Log("STEP PLAY_CONTEXT_AUDIO");
        return false;
    }
    static bool SHOW_BOAT(Logic go)
    {
        Debug.Log("STEP SHOW_BOAT");
        return false;
    }
    static bool PLAY_BOAT_AUDIO(Logic go)
    {
        Debug.Log("STEP PLAY_BOAT_AUDIO");
        return false;
    }
    static bool WALL_UP(Logic go)
    {
        Debug.Log("STEP WALL_UP");
        go.StartCoroutine(go.showPresentationWall(go.wall));
        return false;
    }

    IEnumerator showPresentationWall(GameObject go)
    {
        go.SetActive(true);
        Transform tr = go.GetComponent<Transform>();
        float wallHeight = go.GetComponent<Renderer>().bounds.size.y * 1.5f;
        Debug.Log("Wall height: " + wallHeight);
  
        // loop until the wall is 20 meters high
        while (tr.position.y < wallHeight)
        {
            tr.position = tr.position + new Vector3(0f, 0.05f, 0f);
            yield return null;
        }
        stepDone = true;
    }
    static bool END(Logic go)
    {
        Debug.Log("STEP END");
       // go.StartCoroutine(go.doEnd());
        return false;
    }
    IEnumerator doEnd()
    {
        yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }
    static bool NAVIGATE(Logic go)
    {
        Debug.Log("STEP NAVIGATE");
        stepDone = true;
        
        //go.StartCoroutine(go.doNavigate(go.head));

        return false;
    }
    IEnumerator doNavigate( GameObject head )
    {
        return null;
    }
    GameObject target = null;
    float gazeDuration = 0.0f;
    public void GazeIn(GameObject enterTarget)
    {
        Debug.Log("GazeIN: "+enterTarget.name);
        if (target == null)
        {
            target = enterTarget;
            gazeDuration = Time.realtimeSinceStartup;
            Invoke("checkTime", 2);
        }
    }
    public float speed;
    void checkTime()
    {
        Debug.Log("Should I Move ? ");
        if ( target != null )
        {
            Debug.Log("Move the boat");
            boat.transform.position = Vector3.MoveTowards(boat.transform.position, target.transform.position, Time.deltaTime * speed);
            Invoke("checkTime", 1);
        }
    }
    
    public void GazeOut(GameObject exitTarget)
    {
        Debug.Log("GazeOut: "+exitTarget.name);
        target = null;
    }
}
