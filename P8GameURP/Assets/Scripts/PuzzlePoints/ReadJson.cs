using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
public class ReadJson : MonoBehaviour
{
    private string jsonString;
    private string path = "/Json/PuzzleStates.json";
    private JsonData itemData;
    private List<Vector3> positions;
    int insertNumber = 0;
    public Material mat;   
    MeshRenderer meshrender ;
    SphereCollider col ;
    // File.AppendAllText(Application.dataPath + path, data);
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Vector3>();
        jsonString = File.ReadAllText(Application.dataPath + path);
        itemData = JsonMapper.ToObject(jsonString);
        
        for (int i = 0; i < itemData[0].Count; i++){
            GetPlayerPositionsFromPuzzle(i);
        }
        Debug.LogError("pos count: " + positions.Count + " insert numbers " + insertNumber);     
        for (int j=0; j<positions.Count; j++){
            CreateHeatMap(j);
        }
    }
    void CreateHeatMap(int posIndex){

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        meshrender = sphere.GetComponent<MeshRenderer>();
        col = sphere.GetComponent<SphereCollider>();

        col.enabled = false;
        sphere.transform.position = positions[posIndex];
        sphere.transform.localScale = new Vector3(2, 2, 2);
        meshrender.material = mat;
    }

    void GetPlayerPositionsFromPuzzle(int PuzzleNumberInOrder){
        float x = 0;
        float y = 0;
        float z = 0;
        for (int i = 0; i < itemData[0][PuzzleNumberInOrder]["Player_Positions"].Count; i++){

            int startZ = itemData[0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().IndexOf(itemData[0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Split(',')[2]);
            int endZ = itemData[0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().IndexOf(')');

            string xString = itemData[0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Split(',')[0].Trim('(');
            string yString = itemData[0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Split(',')[1];
            string zString = itemData[0][PuzzleNumberInOrder]["Player_Positions"][i].ToString().Substring(startZ, endZ - startZ);

            x = float.Parse(xString);
            y = float.Parse(yString);
            z = float.Parse(zString);

            positions.Insert(insertNumber, new Vector3(x, y, z));
            insertNumber++;
        }
    }

}
