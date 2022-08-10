using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDorm42Transporter : TransporterTo
{
    public override bool IsEnabled()
    {
        States states = GameObject.Find("States").GetComponent<States>();
        return (states.GetState(AvailableStates.id42HasBeenPickedUp) == "1");
    }
}
