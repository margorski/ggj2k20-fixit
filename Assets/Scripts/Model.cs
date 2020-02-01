using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockModel
{
    public int id = 123;
    public string name = "NAME";
}
public class NetworkMessage
{
    public string dupa = "dupa";
    public int testc = 123;
    public List<BlockModel> blockModels = new List<BlockModel>() { new BlockModel(), new BlockModel() };
}