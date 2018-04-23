using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {

    public GameObject[] roadBlocs;
    public GameObject[] obstacles;
    public List<GameObject> roadList;
    public List<GameObject> obstaclesList;
    public GameObject Player;
    public Canvas GUI;
    public float zPos = 0f;
    public float ozPos = 0f;
    int count = 0;

	// Use this for initialization
	void Start () {
        GUI.transform.GetChild(1).gameObject.SetActive(false);
        GUI.transform.GetChild(2).gameObject.SetActive(false);
        for (int i = 0; i < 18; ++i)
        {
            Addbloc(roadBlocs[0], ref zPos);
        }
        for (int i = 0; i < 7; i++)
        {
            AddObstacles(obstacles[0], ref ozPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponent<PlayerScript>().isGameOver)
        {
            GUI.transform.GetChild(1).gameObject.SetActive(true);
            GUI.transform.GetChild(2).gameObject.SetActive(true);
        }
        GameObject rtmp = roadList[roadList.Count - 1];
        GameObject otmp = obstaclesList[obstaclesList.Count - 1];
        if (rtmp.transform.position.z - Player.transform.position.z < -8)
        {
            Movebloc(ref zPos);
        }
        if (otmp.transform.position.z - Player.transform.position.z < -10)
        {
            MoveObstacles(ref ozPos);
        }
    }

    void Addbloc(GameObject blocModel, ref float pos)
    {
        GameObject newBloc = (GameObject)Instantiate(blocModel, new Vector3(0, 0, zPos), Quaternion.identity);
        pos += 5;
        newBloc.transform.SetParent(this.transform);
        roadList.Insert(0, newBloc);
    }

    void Movebloc(ref float pos)
    {
        GameObject newBloc = roadList[roadList.Count - 1];
        roadList.Remove(newBloc);
        newBloc.transform.position = new Vector3(newBloc.transform.position.x, newBloc.transform.position.y, pos);
        pos += 5;
        newBloc.SetActive(true);
        roadList.Insert(0, newBloc);
    }

    void AddObstacles(GameObject obstacle, ref float zpos)
    {
        float xpos = Random.Range(-4f, 5f);
        float ypos = Random.Range(1f, 5f);
        zpos += 10 + Random.Range(0, 20);
        Vector3 pos = new Vector3(xpos, ypos, zpos);
        GameObject obs = (GameObject)Instantiate(obstacle, pos, Quaternion.identity);
        obs.transform.SetParent(this.transform);
        obstaclesList.Insert(0, obs);
    }

    void MoveObstacles(ref float pos)
    {
        int destroyRoad = Random.Range(0, 10);
        if (Player.transform.position.z > 50 && destroyRoad == 6)
        {
            GameObject tmp = roadList[0];
            GameObject tmp2 = roadList[1];
            tmp.SetActive(false);
            tmp2.SetActive(false);
            pos += Player.GetComponent<PlayerScript>().jumpDistance + Random.Range(0, 20);
            return;
        }
        GameObject obs = obstaclesList[obstaclesList.Count - 1];
        obstaclesList.Remove(obs);
        obs.transform.position = new Vector3(Random.Range(-4f, 5f), Random.Range(1f, 5f), pos);
        pos += Player.GetComponent<PlayerScript>().jumpDistance + Random.Range(0, 20);
        obstaclesList.Insert(0, obs);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
