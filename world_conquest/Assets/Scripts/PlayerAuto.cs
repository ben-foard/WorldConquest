
///<summary>
/// Represents an AI player in the game.
///</summary>
public class playerAI : Player
{
    ///<summary>
    /// Generates a random integer.
    ///</summary>
    ///<returns>The random integer.</returns>
    int RanAction()
    {
        int i = UnityEngine.Random.Range(1, 6);
        return i;
    }

    ///<summary>
    /// Decides based on a d6 probability.
    ///</summary>
    ///<returns>The decision based on probability.</returns>
    int MidMake()
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

    ///<summary>
    /// Determines the choice based on the given criteria.
    ///</summary>
    ///<param name="mid">The mid value.</param>
    ///<param name="rand">The random value.</param>
    ///<returns>True if the choice is active, false otherwise.</returns>
    bool ChoiceDet(int mid, int rand)
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

    ///<summary>
    /// Handles the opening move of the AI player.
    ///</summary>
    void OpeningAuto()
    {
        // Select randomly from unselected list territory
    }

    ///<summary>
    /// Automatically deploys troops for the AI player.
    ///</summary>
    void DeployAuto()
    {
        // Determine number of troops to deploy
        // Collect cards automatically
        // Select random NPC territory
        // Deploy random number of troops (1 to troopsToDeploy)
    }

    ///<summary>
    /// Automatically handles attacks for the AI player.
    ///</summary>
    void AttackAuto()
    {
        while(ChoiceDet(MidMake(), RanAction()))
        {
            // Select territory
            // Is territory troops > 1
            // Is all territory owned by NPC
            // Create list of opponent player territory
            // Randomly select from list
            // Attack country
            // Win
            //   Add random number of troops to new territory
            //   Win card
            // Lose
            // Can you attack? Is troops > 1
        }
    }

    ///<summary>
    /// Automatically fortifies territories for the AI player.
    ///</summary>
    void FortifyAuto()
    {
        int mid = 6 - MidMake();
        while(ChoiceDet(mid, RanAction()))
        {
            // Select territory
            // Is territory troops > 1
            // Is territory around owned by NPC
            // Create a list of NPC neighbors
            // Add troops (range 1 to troopCount - 1)
        }
    }
}
