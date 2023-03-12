using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class TimeTickSystem : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;
    public static event EventHandler<OnTickEventArgs> OnTick_5;

    private const float TICK_TIMER_MAX = .2f; // 2ms

    private int tick;
    private float tickTimer;

    private void Awake()
    {
        tick = 0;   
    }
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer > TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });
            
            if (tick % 5 == 0)//5ms
            {
                if (OnTick_5 != null) OnTick_5(this, new OnTickEventArgs { tick = tick });               
            }
        }
    }
}
/*      USING GUIDE
 * 
 * TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickventArgs e){
 *  CMDebug.TextPopupMouse("Tick: " + e.tick);
 *  };
 * 
 * TimeTickSystem.OnTick_5 += delegate (object sender, TimeTickSystem.OnTickventArgs e){
 *  CMDebug.TextPopupMouse("MegaTick: " + e.tick);
 *  };
 * 
 * 
 */
