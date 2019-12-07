using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class SourceObject : MonoBehaviour
{
    int id = 0;
    const string azimuth = "/MultiEncoder/azimuth";
    const string elevation = "/MultiEncoder/elevation";
    float offset = 0.1f;
    Transform trans;
    GameObject handler;
    OSCOutput output;
    ToolTip toolTip;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>().transform;
        handler = GameObject.FindGameObjectWithTag("OSCHandler");
        output = handler.GetComponent<OSCOutput>();

        toolTip = GetComponent<ToolTip>();
        toolTip.ToolTipText = "Source "+(id+1);
    }

    public void setAzimuth(float azimuth) {
        float radA = azimuth * Mathf.Deg2Rad;
        float radE = this.GetElevation()*Mathf.Deg2Rad;
        float z = Mathf.Cos(radE) * Mathf.Cos(radA);
        float x = Mathf.Cos(radE) * Mathf.Sin(radA);
        float y = Mathf.Sin(radE);
        transform.position =new Vector3(x,y,-z);
  
    }
    public void setElevation(float elevation)
    {
        float radE = elevation * Mathf.Deg2Rad;
        float radA = this.GetAzimuth() * Mathf.Deg2Rad;
        float z = Mathf.Cos(radE) * Mathf.Cos(radA);
        float x = Mathf.Cos(radE) * Mathf.Sin(radA);
        float y = Mathf.Sin(radE);
        transform.position =new Vector3(x, y, -z);
    }

    public float GetElevation() {
        Vector3 position = transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan(position.y / Mathf.Sqrt(position.z * position.z + position.x * position.x));
        return angle;

    }
    public float GetAzimuth() {
        Vector3 position = transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan(position.x/position.z);
        angle = position.z > 0 ? angle + 180 : angle;
        angle = angle > 180 ? angle - 360 : angle;
        return angle*-1;
    }

    public void sendMessageToOSCHandler() {
        string[] data = new string[2];

        data[0] = azimuth + GetID().ToString();
        data[1] = GetAzimuth().ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        data[0] = elevation + GetID().ToString();
        data[1] = GetElevation().ToString();
        output.SendMessage("SendOSCMessageToClient", data);
    }

    public int GetID() {
        return id;
    }
    public void SetID(int id) {
        this.id = id;
    }
}
