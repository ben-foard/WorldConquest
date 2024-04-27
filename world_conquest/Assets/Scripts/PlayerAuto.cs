using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System;
using Unity.VisualScripting;

public class playerAI : Player
{
    //Generate Randome Integer
    int ranAction()
    {
        int i = UnityEngine.Random.Range(1, 6);
        return i;
    }

    //decides based on a d6 probability
    //  mid 1 [1/6 probability 5/6]
    //  mid 2 [1/3 probability 2/3]
    //  mid 3 [1/2 probability 1/2]
    int midmake()
    {
        int mid = 0;
        if(troopCount > 50)
        {
            mid = 3;
        }
        else if(troopCount > 25)
        {
            mid = 2;
        }
        else
        {
            mid = 1;
        }
        return mid;
    }

    //choiceDet(midmake(), ranAction())
    bool choiceDet(int mid, int rand)
    {
        bool inact;
        if(rand >= mid)
        {
            inact = true;
        }
        else
        {
            inact = false;
        }
        return inact;
    }

    void openingAuto()
    {
        //select randomly from unselected list territory
    }

    void deployAuto()
    {
        //num of troops to deploy
            //collect cards automatically
        //select random NPC terriroy
        //deploy random num of troops (1 to troopToDeloy)
    }
    

    // attackAuto()
    void attackAuto()
    {
        while(choiceDet(midmake(), ranAction()))
        {
            //select territory
                //is terriroy troops > 1
                //is all territory owned by NPC
            //create list ofopponent player territory
            //randomly select from list
            //attack country
            //win
                //add random number of troops to new territory
                //win card
            //lose
            //can u attack? is troops > 1
        }
    }

    // attackAuto()
    void fortfyAuto()
    {
        int mid = 6 - midmake();
        while(choiceDet(mid, ranAction()))
        {
            //select terriroty
                //is territory troops > 1
                //is territory around owned by NPC
            //create a list of NPC neigbours
            //add troops (range 1 to troopCount - 1)
        }
    }
}
