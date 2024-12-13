using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DefaultMessage
{

    public int id;
    public string content;
    public string created_at;
    public string updated_at;
}
[System.Serializable]
public class DefaultMessageRoot
{
    public List<DefaultMessage> defaultMessages;
}
