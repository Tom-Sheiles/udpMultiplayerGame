using System.Collections;
using System.Collections.Generic;


public class CommandDictionary 
{
    private IDictionary<int, string> socketMessageDict = new Dictionary<int, string>()
    {
        {0, "ConnectRequest"},
        {1, "DisconnectRequest"},
        {2, "ConnectionSuccessful"},
        {3, "NewPlayerData" }
    };
}
