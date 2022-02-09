using UnityEngine;
using System.Collections;
using System.Linq;

public class Freeze_On_Contact_Configurable_Colorchange : MonoBehaviour
{
    public string[] GOTag;
    public int maxjoints = 0;
    //public FixedJoint[] j;
    float currentDelay;
    public float timer = 0f;
    public int countjoints = 0;
    private Shader shader1;
    private Material material1;
    public bool Change_Color = false;
    public bool Allow_Self = false;

    void timertrig()
    {
        currentDelay = Time.time + timer;
        //Debug.Log(currentDelay);
    }

    void Update()
    {
        if (timer != 0)
        {
            if (Time.time > currentDelay)
            {
                foreach (FixedJoint myJoint in gameObject.GetComponents<FixedJoint>())
                {
                    Destroy(myJoint);
                }
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        timertrig();
        //foreach (ContactPoint contact in collision.contacts)
        ContactPoint contact = collision.contacts[0]; //to avoid problems with multiple point of contacts...
        {
            //Debug.Log("n_collisions " + collision.contacts.Length);
            foreach (string GOTagItem in GOTag)
            {
                if (contact.otherCollider.gameObject.transform.parent != contact.thisCollider.gameObject.transform.parent)
                {
                    if (contact.otherCollider.gameObject.tag == GOTagItem)
                    {
                        if (
                            gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().countjoints
                            <
                            gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().maxjoints * 2
                            &
                            collision.gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().countjoints
                            <
                            collision.gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().maxjoints * 2
                            )
                        {
                            //change to configurable joint
                            //FixedJoint myJoint1 = gameObject.AddComponent<FixedJoint>();
                            ConfigurableJoint myJoint1 = gameObject.AddComponent<ConfigurableJoint>();
                            myJoint1.xMotion = ConfigurableJointMotion.Locked;
                            myJoint1.yMotion = ConfigurableJointMotion.Locked;
                            myJoint1.zMotion = ConfigurableJointMotion.Locked;


                            //FixedJoint myJoint2 = collision.gameObject.AddComponent<FixedJoint>();
                            myJoint1.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                            //myJoint2.connectedBody = gameObject.GetComponent<Rigidbody>();
                            gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().countjoints = (gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().countjoints) + 1;
                            collision.gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().countjoints = (collision.gameObject.GetComponent<Freeze_On_Contact_Configurable_Colorchange>().countjoints) + 1;

                            //change GO color
                            //get the material list
                            //Material newMat = Resources.Load("DRBNVR/Materials/Glass", typeof(Material)) as Material;

                            Transform parent = gameObject.transform.parent;
                            //Debug.Log("parent name " + gameObject.transform.parent.name);
                            //Debug.Log("number of child under parent : " + parent.childCount);
                            //Change the colour for all child attached to that parent
                            if (Change_Color == true)
                            {
                                if (parent.childCount > 0)
                                {
                                    for (int pch = 0; pch <= parent.childCount - 1; pch++)
                                    {
                                        Transform children = parent.GetChild(pch);
                                        //Debug.Log(children.name);
                                        if (children.childCount > 0)
                                        {
                                            for (int ch = 0; ch <= children.childCount - 1; ch++)
                                            {
                                                Transform child = children.GetChild(ch);
                                                //Debug.Log("child " + child.name + " " + transform.childCount);
                                                Material[] mats = child.gameObject.GetComponent<MeshRenderer>().materials;
                                                //Debug.Log("utwatkid " + child.gameObject.GetComponent<MeshRenderer>().materials[0].name);
                                                //mats[0] = newMat;
                                                //gameObject.GetComponentInChildren<MeshRenderer>().materials = mats;
                                                child.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
                                            }
                                        }
                                        else
                                        {
                                            //Material[] mats = gameObject.GetComponent<MeshRenderer>().materials;
                                            //Debug.Log("utwat " + gameObject.GetComponent<MeshRenderer>().materials[0].name);
                                            //mats[0] = newMat;
                                            //gameObject.GetComponentInChildren<MeshRenderer>().materials = mats;
                                            children.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    timertrig();
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        if (contact.thisCollider.gameObject.transform.root.name != contact.otherCollider.gameObject.transform.root.name)
    //        {
    //            Debug.Log("root name " + contact.thisCollider.gameObject.transform.root);
    //            Debug.Log("root child name " + contact.thisCollider.gameObject.transform.root.GetChild(0));
    //            Debug.Log("root child name " + contact.thisCollider.gameObject.transform.root.GetChild(1));
    //            //if (gameObject.GetComponent<FixedJoint>().
    //            //bool to find if tag in list
    //            foreach (string GOTagItem in GOTag)
    //            {
    //                if (contact.otherCollider.gameObject.tag == GOTagItem)
    //                {
    //                    FixedJoint myJoint = gameObject.AddComponent<FixedJoint>();
    //                    myJoint.connectedBody = collision.rigidbody;
    //                    Debug.Log("boop " + collision.collider.gameObject.transform.root.name + " " + collision.collider.gameObject.transform.root.name);
    //                    Debug.Log("Components " + gameObject.GetComponent<FixedJoint>());
    //                }
    //            }
    //        }
    //    }
    //    /*
    //    FixedJoint myJoint = gameObject.AddComponent<FixedJoint>();
    //    myJoint.connectedBody = collision.rigidbody;
    //    Debug.Log("boop " + collision.collider.gameObject.transform.parent.name + " " + collision.collider.gameObject.transform.parent.name);
    //    */
    //}
}

