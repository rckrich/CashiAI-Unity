using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMessage
{
    public int id { get; set; }
    public string content { get; set; }
    public object created_at { get; set; }
    public object updated_at { get; set; }
}

public class DefaultMessageRoot
{
    public List<DefaultMessage> defaultMessages { get; set; }
}
