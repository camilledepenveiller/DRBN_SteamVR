using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;



public class SaveSnapShot : MonoBehaviour
{
    //variables to get Server and transformName from servermode
    ServerMode Server = new ServerMode();
    public string transformName, directory;


    void Start ()
	{
        //create directory to save trajectories and use transformName as... name
        transformName = Server.transformName;
        directory = Application.persistentDataPath + "/trajectory_data/" + transformName + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss-tt");

        Directory.CreateDirectory(directory);

        //		string json = saveJSON();
        //		loadJSON(json);
        //Debug.Log("WTF!");
        //savePDB2();

        //toy function to test transform.TransformPoint
        //testcube();
    }

    //private float nextActionTime = 0.0f;
    //public float period = 100000f;
    int nextActionTime = 0;
    int period = 10;


    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            periodicsaveJSON(nextActionTime);
        }
    }

    static string GameObjectPath (GameObject obj)
	{
		string path = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			path = "/" + obj.name + path;
		}
		return path;
	}

	/// <summary>
	/// Method to create a list of PDBs in a directory
	/// </summary>
	/// <param name="_dataList">Data list value</param>
	List<string> pdb_dir(string pdb_path)
	{
		List<string> pdbs = new List<string>();

		List<string> content = new List<string>();

		content = Directory.GetFiles (pdb_path).ToList();

		//for loop to read all PDBs in the directory
		for (int i = 0; i < content.Count; i++) 
		{
			//if .pdb
			string instr = content[i];
			string subinstr = instr.Substring (instr.Length - 3);
			if (subinstr=="pdb") 
			{
				pdbs.Add (instr);
			}
		}
		return pdbs;
	}
    	
    public void savePDB()
    {
        //path with all the PDBs 
        string pdb_path = Application.dataPath + "/DRBN_STEAMVR/Prefab/Simulation/PDBs_To_Scale/_PDBs/";
        //list all the PDBs in path
        List<string> pdb_names = pdb_dir(pdb_path);

        Debug.Log(pdb_names.Count);

        //read the content of the json file containing all the rigid body location and rotation
        string json;
        json = File.ReadAllText(Application.persistentDataPath + "/gamesave_list_test.jsonbrn");

        RBListContainer container = JsonUtility.FromJson<RBListContainer>(json);

        Debug.Log("container size " + container.dataList.Count);
        for (int i = 0; i < container.dataList.Count; i++)
        {
            Debug.Log("i " + i);
            string GOname = container.dataList[i].name;
            Vector3 GOpos = container.dataList[i].pos;
            Vector3 GOrot = container.dataList[i].rot;
            //Debug.Log("Name: " + container.dataList[i].name + " Rot: " + container.dataList[i].pos + ", Pos: " + container.dataList[i].rot);
            //Debug.Log((GameObject.Find (container.dataList [i].name)));
            for (int files = 0; files < pdb_names.Count; files++)
            {
                Debug.Log("files " + files);
                string filename = pdb_names[files];
                //Debug.Log (filename);
                if (filename.Contains(GOname))
                {
                    string currentline;
                    List<string> pdbcontent = File.ReadAllLines(filename).ToList();
                    List<string> grocontent = new List<string>();

                    grocontent.Add(filename);
                    grocontent.Add((pdbcontent.Count - 1).ToString());

                    for (int pdbline = 0; pdbline < pdbcontent.Count; pdbline++)
                    {
                        currentline = pdbcontent[pdbline];
                        //change split to "hard" columns because whitespaces are unreliable
                        //string[] linewords = currentline.Split (new char[]{' ','\t'}, StringSplitOptions.RemoveEmptyEntries);
                        List<string> linewords = new List<string>();
                        //tediously cut currentline by hand...
                        if (currentline != "END")
                        {
                            Debug.Log("currentline length " + currentline.Length);
                            Debug.Log("currentline" + currentline);
                            //atom number : 
                            linewords.Add(currentline.Substring(7, 4));
                            //atom type :
                            linewords.Add(currentline.Substring(13, 4));
                            //Amino Acid Name : 
                            linewords.Add(currentline.Substring(17, 3));
                            //Residue number :
                            linewords.Add(currentline.Substring(23, 3));
                            //Coord xyz
                            linewords.Add(currentline.Substring(30, 7));
                            linewords.Add(currentline.Substring(38, 7));
                            linewords.Add(currentline.Substring(46, 7));

                            //*0.1f to transition from PDB to GRO
                            float x = float.Parse(linewords[4]) * 0.1f;
                            float y = float.Parse(linewords[5]) * 0.1f;
                            float z = float.Parse(linewords[6]) * 0.1f;

                            Vector3 coords = new Vector3(x, y, z);
                            Vector3 trans_pos_coord = coords + Vector3.Scale(GOpos, new Vector3(100f, 100f, 100f));

                            //Vector3 trans_rot_coord = Vector3.Scale(trans_pos_coord,GOrot);//not working
                            Vector3 trans_rot_coord = Quaternion.Euler(GOrot.z, GOrot.x, GOrot.y) * trans_pos_coord;

                            //variable created for test reasons... 
                            Vector3 finalcoords = trans_rot_coord;

                            Debug.Log("coords " + coords + " trans_pos_coord " + trans_pos_coord);

                            //Debug.Log ("coords " + coords + " trans_pos_coords " + trans_pos_coord + " GOpos " + GOpos + " GOrot " + GOrot);
                            //make string for string replacement
                            //Debug.Log("    1LEU    N      1   0.109   0.254  -2.926" + " test line ");
                            var atom_number = linewords[0];
                            var atom_type = linewords[1];
                            var AminoA_name = linewords[2];
                            var AminoA_num = linewords[3];

                            grocontent.Add(string.Format("{0,5}{1,-7}{2,-4}{3,4}{4,8:##0.000}{5,8:##0.000}{6,8:##0.000}",
                                AminoA_num, AminoA_name, atom_type, atom_number,
                                finalcoords.x, finalcoords.y, finalcoords.z));
                        }
                    }
                    //grocontent.Add (string.Format ("{0,9:0.00000}", 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    grocontent.Add(string.Format("{0,10:0.00000}{1,10:0.00000}{2,10:0.00000}{3,10:0.00000}{4,10:0.00000}{5,10:0.00000}{6,10:0.00000}{7,10:0.00000}{8,10:0.00000}", 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    //File.WriteAllLines("D:\\PDBs\\" + GOname + ".gro", grocontent.ToArray());
                    File.WriteAllLines("D:\\PDBs\\" + GOname + files + ".gro", grocontent.ToArray());
                }
            }
        }
    }

    public void saveGRO()
    {
        string pdb_path = Application.dataPath + "/DRBN_STEAMVR/Prefab/Simulation/PDBs_To_Scale/_PDBs/";

        List<string> pdb_names = pdb_dir(pdb_path);

        Debug.LogWarning("pdb names " + pdb_names.Count);

        string json;
        json = File.ReadAllText(Application.persistentDataPath + "/gamesave_coor_test.jsonbrn");

        RBCoordContainer container = JsonUtility.FromJson<RBCoordContainer>(json);

        //Debug.Log("container size " + container.coordList.Count);
        List<string> GOname = new List<string>();
        List<List<Vector3>> GOcoord = new List<List<Vector3>>();

        for (int i = 0; i < container.coordList.Count; i++)
        {
            //Debug.LogWarning(container.coordList[i].name != "" + " _ " + container.coordList[i].name);

            //Debug.LogWarning("name bis! " + i + " " + container.coordList[i].name);

            if (container.coordList[i].name!=" " || container.coordList[i].name != "")
            {
                //Debug.LogWarning("crikey");
                GOname.Add(container.coordList[i].name);
                GOcoord.Add(container.coordList[i].atom_coor);
            }

            Debug.LogWarning("coordinate list" + GOcoord[i]);
        }

        for (int files = 0; files < pdb_names.Count; files++)
        {
            Debug.LogWarning("files " + files);
            string filename = pdb_names[files];
            for (int GOnames = 0; GOnames < GOname.Count; GOnames++)
            {
                if (filename.Contains(GOname[GOnames]) && GOname[GOnames]!="")//too general, changing the if conditions 
                //if (filename == GOname[GOnames]+"_bric.pdb")
                {
                    Debug.LogWarning("filename " + filename + " GOname " + GOname[GOnames]);
                    ///
                    //read the PDB files and keep info for exportation to GRO
                    ///
                    string currentline;
                    List<string> pdbcontent = File.ReadAllLines(filename).ToList();
                    List<string> grocontent = new List<string>();

                    grocontent.Add(filename);
                    grocontent.Add((pdbcontent.Count - 1).ToString());

                    //Debug.LogWarning("pdb " + pdbcontent.Count + " GRO " + GOcoord[GOnames].Count + " name " + GOname[GOnames]);

                    for (int pdbline = 0; pdbline < pdbcontent.Count; pdbline++)
                    {
                        currentline = pdbcontent[pdbline];
                        //change split to "hard" columns because whitespaces are unreliable
                        //string[] linewords = currentline.Split (new char[]{' ','\t'}, StringSplitOptions.RemoveEmptyEntries);
                        List<string> linewords = new List<string>();
                        //tediously cut currentline by hand...
                        if (currentline != "END")
                        {
                            //Debug.Log("currentline length " + currentline.Length);
                            //Debug.Log("currentline " + currentline);
                            //atom number : 
                            linewords.Add(currentline.Substring(7, 4));
                            //atom type :
                            linewords.Add(currentline.Substring(13, 4));
                            //Amino Acid Name : 
                            linewords.Add(currentline.Substring(17, 3));
                            //Residue number :
                            linewords.Add(currentline.Substring(23, 3));
                            //Coord xyz
                            linewords.Add(currentline.Substring(30, 7));
                            linewords.Add(currentline.Substring(38, 7));
                            linewords.Add(currentline.Substring(46, 7));

                            //*0.1f to transition from PDB to GRO
                            for (int i = 0; i < linewords.Count; i++)
                            {
                                Debug.LogWarning(i + " linewords[i]" + linewords[i]);
                            }

                            float x = float.Parse(linewords[4]) * 0.1f;
                            float y = float.Parse(linewords[5]) * 0.1f;
                            float z = float.Parse(linewords[6]) * 0.1f;



                            var atom_number = linewords[0];
                            var atom_type = linewords[1];
                            var AminoA_name = linewords[2];
                            var AminoA_num = linewords[3];

                            grocontent.Add(string.Format("{0,5}{1,-7}{2,-4}{3,4}{4,8:##0.000}{5,8:##0.000}{6,8:##0.000}",
                                AminoA_num, AminoA_name, atom_type, atom_number,
                                           //x, y, 0)); //replace finalcoords with coords from JSON
                                           GOcoord[GOnames][pdbline][0]*-100 , GOcoord[GOnames][pdbline][1]*100, GOcoord[GOnames][pdbline][2]*100));
                            Debug.LogWarning("GOcoord names " + filename + "  " + GOname[GOnames]);
                            Debug.LogWarning("GOcoord[GOnames][0] " + GOnames + " " + GOcoord[GOnames][0]);
                            //Debug.Log("Len GOcoord[GOnames] " + 1/3*GOcoord[GOnames].Count + " pdbcontent.Count " + pdbcontent.Count);
                            Debug.LogWarning("Len GOcoord[GOnames] " + GOcoord[GOnames][pdbline] + " pdbcontent.Count " + pdbcontent[pdbline]);
                        }
                    }
                    //write the GRO and include info from the JSON file
                    grocontent.Add(string.Format("{0,10:0.00000}{1,10:0.00000}{2,10:0.00000}{3,10:0.00000}{4,10:0.00000}{5,10:0.00000}{6,10:0.00000}{7,10:0.00000}{8,10:0.00000}", 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    File.WriteAllLines("D:\\PDBs\\" + GOname[GOnames] + GOnames.ToString() + ".gro", grocontent.ToArray());
                    Debug.LogWarning("boo" + GOname[GOnames]);
                }
                else
                {
                    Debug.Log("filename " + filename + " GOname " + GOname[GOnames]);
                    Debug.Log("oi! did not found any coresponding file");
                }
            }
        }
    }

    //public void testcube()
    //{
    //    GameObject CoordGO;
    //    Vector3[] Coord;
    //    List<Vector3> World=new List<Vector3>();

    //    CoordGO = GameObject.Find("coord_test2");
    //    Coord = CoordGO.GetComponent<MeshFilter>().mesh.vertices;


    //    for (int i = 0; i < Coord.Length; i++)
    //    {
    //        Debug.Log("Coord " + Coord[i] + " before");
    //        Coord[i] = CoordGO.transform.TransformPoint(Coord[i]);
    //        Debug.Log("Coord " + Coord[i].ToString("F4") + " after");
    //        World.Add(Coord[i]);
    //    }
    //}

    public void testcube()
    {
        GameObject CoordGO;
        Vector3[] Coord;
        List<Vector3> World = new List<Vector3>();

        CoordGO = GameObject.Find("coord_test2");
        Coord = CoordGO.GetComponent<MeshFilter>().mesh.vertices;


        for (int i = 0; i < Coord.Length; i++)
        {
            Debug.Log("Coord " + Coord[i] + " before");
            Coord[i] = CoordGO.transform.TransformPoint(Coord[i]);
            Debug.Log("Coord " + Coord[i].ToString("F4") + " after");
            World.Add(Coord[i]);
        }
    }

    public void saveJSON()
    {
        List<Savedata> savefile = new List<Savedata>();
        List<Coords> savecoords = new List<Coords>();

        List<Rigidbody> RBlist = new List<Rigidbody>();

        List<GameObject> Coordlist = new List<GameObject>();

        Debug.Log("fark!" + gameObject.transform.position + " " + transform.localScale);
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2);
        Debug.Log("gameObject.transform.position " + gameObject.transform.position + " transform.localScale " + transform.localScale);

        List<Coords> GOVertscoords = new List<Coords>();

        int max = hitColliders.Length;
        Debug.Log("max " + hitColliders.Length);
        for (int i = 0; i < max; i++)
        {
            string GO_name = new string(new char[] { });
            Vector3 scale;
            Vector3[] v;
            List<Vector3> vlist = new List<Vector3>();
            //because unity makes f*g dupes!
            List<Vector3> vlistnodupes = new List<Vector3>();
            if (hitColliders[i].attachedRigidbody != null)
            {
                RBlist.Add(hitColliders[i].attachedRigidbody);

                GameObject GO;
                GameObject GOchild;

                //Coordlist.Add(hitColliders[i].gameObject.transform.Find("coord").GetComponent<GameObject>()); //find gameobject "coord" with hardcoded atom coordinates
                GO = hitColliders[i].gameObject;
                Debug.Log("GO.name " + GO.name);
                GOchild = GO.transform.Find("coord").gameObject;
                
                
                //GO = hitColliders[i].gameObject.transform.gameObject.
                //GO_name = GO.name;
                GO_name = GO.name;
                scale = GO.transform.localScale;
                Debug.Log("scale " + scale.ToString("F4"));
                
                v = GOchild.GetComponent<MeshFilter>().mesh.vertices;

                for (int viteration = 0; viteration < v.Length; viteration++)
                {
                    v[viteration] = GO.transform.TransformPoint(v[viteration]);
                    //Debug.Log("local space? " + v[viteration] + " world position? " + GO.transform.TransformPoint(v[viteration]).ToString("F6"));
                }

                vlist = v.ToList<Vector3>();
                vlistnodupes = vlist.Distinct().ToList();
                vlist = vlistnodupes;

                
            }
            GOVertscoords.Add(new Coords(GO_name, vlist));
            Debug.Log("GOVertscoord.Count results " + GOVertscoords.Count + " vlist " + vlist.Count + " vlistnodupes " + vlistnodupes.Count);
        }

        Debug.Log("Saving...");

        int rbmax = RBlist.Count;
        for (int i = 0; i < rbmax; i++)
        {
            savefile.Add(new Savedata(RBlist[i].name, RBlist[i].position, RBlist[i].rotation.eulerAngles));
        }

        //int gomax = Coordlist.Count;
        //for (int go = 0; go < gomax; go++)
        //{

        //}

        RBListContainer container = new RBListContainer(savefile);

        RBCoordContainer coorcontainer = new RBCoordContainer(GOVertscoords);

        string json = JsonUtility.ToJson(container, true);
        string coojson = JsonUtility.ToJson(coorcontainer, true);

        Debug.Log("json " + json + "coojson" + coojson);
        File.WriteAllText(Application.persistentDataPath + "/gamesave_list_test.jsonbrn", json);
        File.WriteAllText(Application.persistentDataPath + "/gamesave_coor_test.jsonbrn", coojson);

        //testing see if it saves .gro files in the same time
        Debug.LogWarning("oh come on");
        saveGRO();
        //savePDB();
    }

    public void periodicsaveJSON(int step)
    {
        Langevin_v2 Lange = GameObject.FindGameObjectWithTag("Physics_Sim").GetComponent<Langevin_v2>();

        //List<Savedata> savefile = new List<Savedata>();
        List<SavedataHierarchy> savefile = new List<SavedataHierarchy>();

        List<Rigidbody> RBlist = new List<Rigidbody>();

        Debug.Log("fark!" + gameObject.transform.position + " " + transform.localScale);

        Debug.Log("gameObject.transform.position " + gameObject.transform.position + " transform.localScale " + transform.localScale);

        List<Coords> GOVertscoords = new List<Coords>();

        int max = Lange.GOS.Count;
        int mol = Lange.GOmolist.Count;

        for (int i = 0; i < max; i++)
        {
            RBlist.Add(Lange.GOS[i]);
        }

        int rbmax = RBlist.Count;
        for (int i = 0; i < rbmax; i++)
        {
            savefile.Add(new SavedataHierarchy(RBlist[i].transform.parent.name,RBlist[i].name, RBlist[i].position, RBlist[i].rotation.eulerAngles));
        }

        RBListContainerHierarchy container = new RBListContainerHierarchy(savefile);

        string json = JsonUtility.ToJson(container, true);

        Debug.Log("json " + json);
        Debug.Log("step " + step);

        //File.WriteAllText("D:/trajectory_data/gamesave_list_test_"+ step.ToString() + ".jsonbrn", json);//not multiplatform, try something else 
        //File.WriteAllText("/mnt/d/trajectory_data/gamesave_list_test_" + step.ToString() + ".jsonbrn", json);//not multiplatform, try something else 
        //File.WriteAllText(Application.persistentDataPath + "/trajectory_data/gamesave_list_test_" + step.ToString() + ".jsonbrn", json);
        //Debug.Log("writing " + Application.persistentDataPath + "/trajectory_data/gamesave_list_test_" + step.ToString() + ".jsonbrn");

        //multiplatform but writing empty saves WTF
        //if (Directory.Exists(Application.persistentDataPath + "/trajectory_data"))
        if (Directory.Exists(directory))
        {
            //File.WriteAllText(Application.persistentDataPath + "/trajectory_data/gamesave_list_test_" + step.ToString() + ".jsonbrn", json);
            File.WriteAllText(directory + "/gamesave_list_test_" + step.ToString() + ".jsonbrn", json);
        }
        //else
        //{
        //    var folder = Directory.CreateDirectory(Application.persistentDataPath + "/trajectory_data");
        //}


        //File.WriteAllText("/trajectory_data/gamesave_list_test_" + step.ToString() + ".jsonbrn", json);
    }

    //public void saveJSON()
    //{
    //    List<Savedata> savefile = new List<Savedata>();

    //    List<Rigidbody> RBlist = new List<Rigidbody>();


    //    Debug.Log("fark!" + gameObject.transform.position + " " + transform.localScale);
    //    Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2);
    //    Debug.Log("gameObject.transform.position " + gameObject.transform.position + " transform.localScale " + transform.localScale);

    //    int max = hitColliders.Length;
    //    for (int i = 0; i < max; i++)
    //    {
    //        if (hitColliders[i].attachedRigidbody != null)
    //        {
    //            RBlist.Add(hitColliders[i].attachedRigidbody);
    //        }
    //    }

    //    int rbmax = RBlist.Count;
    //    for (int i = 0; i < rbmax; i++)
    //    {
    //        savefile.Add(new Savedata(RBlist[i].name, RBlist[i].position, RBlist[i].rotation.eulerAngles));
    //    }

    //    RBListContainer container = new RBListContainer(savefile);

    //    string json = JsonUtility.ToJson(container, true);

    //    Debug.Log(json);
    //    File.WriteAllText(Application.persistentDataPath + "/gamesave_list_test.jsonbrn", json);
    //}

    void loadJSON(string json)
    {
        // TODO: Wrap this in try/catch to handle deserialization exceptions
        RBListContainer container = JsonUtility.FromJson<RBListContainer>(json);

        Debug.Log(container.dataList.Count);
        for (int i = 0; i < container.dataList.Count; i++)
        {
            Debug.Log("Name: " + container.dataList[i].name + " Rot: " + container.dataList[i].pos + ", Pos: " + container.dataList[i].rot);
        }
    }

    void JSON2GRO(string json)
    {
        RBCoordContainer coordContainer = JsonUtility.FromJson<RBCoordContainer>(json);
    }


}

/// <summary>
/// Constructor
/// </summary>
/// <param name="_dataList">Data list value</param>
public struct RBListContainer
{
    public List<Savedata> dataList;

    public RBListContainer(List<Savedata> _dataList)
    {
        dataList = _dataList;
    }
}

public struct RBListContainerHierarchy
{
    public List<SavedataHierarchy> dataList;

    public RBListContainerHierarchy(List<SavedataHierarchy> _dataList)
    {
        dataList = _dataList;
    }
}

public struct RBCoordContainer
{
	public List<Coords> coordList;

	public RBCoordContainer(List<Coords> _coordlist)
	{
		coordList = _coordlist;
	}
}

[Serializable]
public struct Savedata
{
	public string name;
	public Vector3 pos;
	public Vector3 rot;

	public Savedata(string GO_name, Vector3 position ,Vector3 rotation)
	{
		pos = position;
		rot = rotation;
		name = GO_name;
	}
}

[Serializable]
public struct SavedataHierarchy
{
    public string root;
    public string name;
    public Vector3 pos;
    public Vector3 rot;

    public SavedataHierarchy(
        string root_name,
        string GO_name, 
        Vector3 position, 
        Vector3 rotation
        )
    {
        root = root_name;
        pos = position;
        rot = rotation;
        name = GO_name;
    }
}

[Serializable]
public struct Coords
{
	public string name;
	public List<Vector3> atom_coor;

	public Coords(string GO_name, List<Vector3> atom_coordinates)
	{
		name = GO_name;
        atom_coor = atom_coordinates;
    }
}