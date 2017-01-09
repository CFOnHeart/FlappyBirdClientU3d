
using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeScript : MonoBehaviour
{
    public const float EPS = 0.0000000001f;
    public const float SCORE_INCREASING_INTERVAL = 0.2f;
    public const float BIRD_START_POS = -1.09f;
    public static string [] MEDAL = { "gold" , "silver" , "bronze"};

    public static bool isEnd = false;
    public static bool isCollide = false; //检测是否碰撞
    public GameObject endPanel;

    public double starttime = 0;

    public static float speed = 4.0f;
    public static float duration = 2.0f;
    private const float RANGEUP = 3.37f;

    //pipe
    private GameObject pipe_upObject;
    private GameObject pipe_downObject;
    private List<GameObject> Pipes = new List<GameObject>();

    //background
  //  private GameObject background1;
  //  private GameObject background2;

    //bird
    public static GameObject bird;
   // private Vector3 end_pos;
    private float bird_angle;
    //score
    public static int score = 0;
    public static int best_score = 0;


    public static GameObject scorecount_label; //实时计分板
    public GameObject curscore_label;
    public GameObject bestscore_label;
  //  public static UISprite[] scorecount_sprite = new UISprite[3];
    // Use this for initialization
    void Start()
    {
        Debug.Log("Begin");

        isEnd = false;
        isCollide = false;
        bird_angle = 0;
        scorecount_label = GameObject.Find("UI Root/scorecount/scorecount_label");
        curscore_label = GameObject.Find("UI Root/endPanel/scoreboard/curscore_label");
        bestscore_label = GameObject.Find("UI Root/endPanel/scoreboard/bestscore_label");

        System.TimeSpan ts = System.DateTime.Now - System.DateTime.Parse("1970-1-1");
        System.String tim = ts.TotalMilliseconds.ToString();
        starttime = double.Parse(tim);
      //  Debug.Log("curtime = " + tim);

        pipe_upObject = Resources.Load("Prefabs/pipe_up") as GameObject;
        pipe_downObject = Resources.Load("Prefabs/pipe_down") as GameObject;

        GameObject background = Resources.Load("Prefabs/background") as GameObject;
        GameObject birdLoad = Resources.Load("Prefabs/bird_1") as GameObject;

        bird = GameObject.Instantiate(birdLoad);
        bird.transform.position = new Vector3(BIRD_START_POS , bird.transform.position.y , bird.transform.position.z);
        bird.GetComponent<AudioSource>().Stop();
        if (pipe_upObject == null || pipe_downObject == null)
        {
            Debug.Log("Can't load prefab pipe.");
        }

        //隐藏结束画面的ui
        endPanel = GameObject.Find("endPanel");
        endPanel.SetActive(false);

        

        //实时计分
        score = 0;


        //ganjun update 12/29 上传鸟即时的y坐标
        StartCoroutine(UploadBirdInfo.uploadBirdPosY());  

      //  setRealtimeScore(10);
        StartCoroutine( PerPipeLoad() );
        StartCoroutine( angleRotate() );
        //所有的消息获取全在下面这个协程中
        StartCoroutine( messageGetFromServer() );
     //   Debug.Log("after PerPipeLoad");
    }

    // Update is called once per frame
    void Update()
    {
        // double curtime = convertCurrentTime();
        
        GameObject[] del = new GameObject[2];
        int cnt = 0;
        foreach (GameObject obj in Pipes)
        {
            if (obj.transform.position.x < -8.0)
            {
                del[cnt++] = obj;
                if (cnt >= 2) break;
            }
        }
        for(int i=0; i<2; i++)
        {
            Pipes.Remove(del[i]);
            GameObject.Destroy(del[i]);
        }

        if (bird_angle < -90.0f) bird_angle = -90.0f;
        if (bird_angle > 90.0f) bird_angle = 90.0f;
    //    Debug.Log("curtime bird's angle: " + bird_angle.ToString());
        bird.transform.rotation = Quaternion.AngleAxis(bird_angle, new Vector3(0, 0, 1)); //绕着0,0,1的轴旋转了bird_angle的角度

        if (isEnd == true)
        {
          //  bird.transform.position = end_pos;
            
            return;
        }

        if(isCollide == true)
        {
            //自己的鸟撞到了，那么就给对面那个鸟发个消息说自己死了
            string message = "15";
            try
            {
                if(ServerConnect.instance == null)
                {
                    ServerConnect.instance = new ServerConnect();
                    ServerConnect.instance.connect();
                }
                ServerConnect.instance.sendMessage(message);
            }
            catch(Exception ex)
            {
                Debug.LogWarning("The bird Collid Exception: "+ex.ToString());
            }
            GameObject.Find("background").GetComponent<AudioSource>().Stop();
            StartCoroutine(gameOver());
            isEnd = true;
            return;
        }
        
        // if (Input.GetKey(KeyCode.A))
        if (Input.GetMouseButtonDown(0)) //鼠标左键
        {
            
            float y = bird.GetComponent<Rigidbody2D>().velocity.y;
         //   Debug.Log("put the mouse down "+y.ToString());
            bird_angle =45.0f;
            bird.GetComponent<Rigidbody2D>().velocity = new Vector3(0, y+3.6f, 0);
           // bird.GetComponent<AudioSource>().
            bird.GetComponent<AudioSource>().Play();
            
        }

    }

    void FixedUpdate()
    {
    }

    public void printEndPanel()
    {
        endPanel.SetActive(true);
        PipeScript.score = (PipeScript.score+1)/2;
        if (PipeScript.score < 5)
        {
            GameObject.Find("UI Root/endPanel/scoreboard/medal").GetComponent<UISprite>().spriteName = MEDAL[2];
        }else if (PipeScript.score < 10)
        {
            GameObject.Find("UI Root/endPanel/scoreboard/medal").GetComponent<UISprite>().spriteName = MEDAL[1];
        }else
        {
            GameObject.Find("UI Root/endPanel/scoreboard/medal").GetComponent<UISprite>().spriteName = MEDAL[0];
        }
        PipeScript.best_score = Mathf.Max(PipeScript.best_score, PipeScript.score);
        isEnd = true; 
        //将实时计分隐藏起来
        scorecount_label.SetActive(false);
      //  setScore(PipeScript.score);
        StartCoroutine( scoreIncreasing(score) );

        updateBestScoreOnServer();
        setBestScore(PipeScript.best_score);
        
    }
    
    //更新服务器里的最高分
    public void updateBestScoreOnServer()
    {
        //ServerConnect serverConnect = new ServerConnect();
        if (ServerConnect.instance == null)
        {
            ServerConnect.instance = new ServerConnect();
            ServerConnect.instance.connect();
        }
        //  serverConnect.connect();
       // ServerConnect.instance.connect();
        string message = "2" + PipeScript.best_score.ToString();
        int len = message.Length;
        if (len >= 10) message = len.ToString() + message;
        else message = "0" + len.ToString() + message;
        ServerConnect.instance.sendMessage(message);
       
        
        Debug.Log("put the info to the server ");

      //  ServerConnect.instance.close();
    }
    public void restart()
    {
        isEnd = false; score = 0;
        foreach(GameObject obj in Pipes)
        {
            GameObject.Destroy(obj);
        }
        Pipes.Clear();
        //Application.LoadLevel("start_scene"); 
        SceneManager.LoadScene("get_ready");
    }
   // private int index;
    IEnumerator PerPipeLoad()
    {
        PublicInfo.pipeNumber = 0;
        while (true)
        {
          //  Debug.Log("duration: " + duration);
            yield return new WaitForSeconds(duration);
            if (isEnd == false)
            {
                Debug.Log("bird's posx is: " + bird.transform.position.x.ToString());
                
                float delta = PublicInfo.RANDOM_PIPE[PublicInfo.pipeNumber];
                PublicInfo.pipeNumber++;
                if (PublicInfo.pipeNumber >= PublicInfo.RANDOM_SIZE) PublicInfo.pipeNumber = 0;
                //Add up pipes
                GameObject upPipe = GameObject.Instantiate(pipe_upObject);
                upPipe.transform.position = new Vector3(5, -0.02f + delta, -1.0f);
                Pipes.Add(upPipe);

                GameObject downPipe = GameObject.Instantiate(pipe_downObject);
                downPipe.transform.position = new Vector3(5, -2.72f + delta, -1.0f);
                Pipes.Add(downPipe);

                // Debug.Log("...");
            }
        }
    }

    //将分数对应填写进结束的面板中
    public void setScore(int score)
    {
        curscore_label.GetComponent<UILabel>().text = score.ToString();
        return;
    }
    public void setBestScore(int score)
    {
        bestscore_label.GetComponent<UILabel>().text = score.ToString();
        return;
    }
    public static void setRealtimeScore(int score)//设置实时分数
    {
        scorecount_label.GetComponent<UILabel>().text = score.ToString();
    }

    //游戏结束
    public IEnumerator gameOver()
    {
        printEndPanel();
        int index = 0;
        // endPanel.SetActive(true);
        GameObject gameover = new GameObject();
        gameover = GameObject.Find("UI Root/endPanel/gameover");
        GameObject scoreboard = new GameObject();
        scoreboard = GameObject.Find("UI Root/endPanel/scoreboard");
        GameObject buttonPanel = new GameObject();
        buttonPanel = GameObject.Find("UI Root/endPanel/buttonPanel");
        gameover.SetActive(false);
        scoreboard.SetActive(false);
        buttonPanel.SetActive(false);
        while (index < 3)
        {
            endPanel.SetActive(true);
            if (index == 0) gameover.SetActive(true);
            else if (index == 1) scoreboard.SetActive(true);
            else buttonPanel.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            index++;
        }
    }
    //协程令鸟每一段时间都要转点方向
    public IEnumerator angleRotate()
    {
        while (isEnd == false && isCollide == false)
        {
            yield return new WaitForSeconds(0.016f);
            bird_angle -= 1.0f;
            //bird 的体型不断变大增加难度
            bird.transform.localScale += new Vector3(0.001f, 0.001f);
        }
    }
    //协程令最后结束计分板计分从0开始递增直到自己的分数
    public IEnumerator scoreIncreasing(int score)
    {
        for(int i=1; i <= score; i++)
        {
            setScore(i);
            yield return new WaitForSeconds(SCORE_INCREASING_INTERVAL);
            
        }
    }

    //协程等待server传递过来的消息
    public IEnumerator messageGetFromServer()
    {
        while (true)
        {
            string mess = ServerConnect.instance.receiveMessage();
            if(mess.CompareTo("another_gameover") == 0) //对面的鸟死了
            {
                //需要把对面的鸟给删了，自己继续游戏
                Debug.Log("对面有只鸟死了");
            }
            else
            {
                //自己的鸟死了弹出了计分板
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public static double convertCurrentTime()
    {
        
        System.TimeSpan ts = System.DateTime.Now - System.DateTime.Parse("1970-1-1");
        System.String tim = ts.TotalMilliseconds.ToString();
        double curtime = double.Parse(tim);
        return curtime;
    }

}