using System;
using System.Collections;
using System.Collections.Generic;
public enum PLAYER_TYPE : byte
{
    HUMAN,
    AI
}

public class User
{
  //  public delegate void SendFn(CPacket msg);
   // SendFn send_function;

    public byte player_index { get; private set; }

    public UserAgent agent { get; private set; }

    PLAYER_TYPE player_type;



    public User(byte player_index, PLAYER_TYPE player_type, bool isBoss)
    {
        this.player_index = player_index;
        this.agent = new UserAgent(player_index, isBoss);
        this.player_type = player_type;
        switch (this.player_type)
        {
            case PLAYER_TYPE.HUMAN:

                break;
        }

    }

    public User(String name, int money)
    {
        
    }
    /*
    public void send(CPacket msg)
    {
        this.send_function(msg);
    }

    public void reset()
    {
        if (this.ai_logic != null)
        {
            this.ai_logic.reset();
        }
    }*/
    public bool is_autoplayer()
    {
        return this.player_type == PLAYER_TYPE.AI;
    }



}