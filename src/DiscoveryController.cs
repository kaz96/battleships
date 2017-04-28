
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Diagnostics;
using SwinGameSDK;
using System.Timers;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{
    

     static Random _Random = new Random ();

    /// <summary>
    /// Handles input during the discovery phase of the game.
    /// </summary>
    /// <remarks>
    /// Escape opens the game menu. Clicking the mouse will
    /// attack a location.
    /// </remarks>
    public static void HandleDiscoveryInput()
	{
        SwinGameSDK.Timer test = new SwinGameSDK.Timer ();


        test = SwinGame.CreateTimer ();
        SwinGame.StartTimer (test);

        bool test1 = true;

        bool random;

        while (test1 == true) 
        {
            var ticks =SwinGame.TimerTicks (test);

            if (ticks >= 5000)
            {
                random = true;
                test1 = false;
                DoAttack (random);

            }

            if (SwinGame.KeyTyped (KeyCode.vk_ESCAPE)) {
                GameController.AddNewState (GameState.ViewingGameMenu);
            }






            if (SwinGame.MouseClicked (MouseButton.LeftButton)) {
                random = false;
                test1 = false;
                DoAttack (random);
            }

            if (SwinGame.KeyTyped (KeyCode.vk_r)) {
            random = true;
                test1 = false;
            DoAttack (random);
        }
        }


	}

  

    /// <summary>
    /// Attack the location that the mouse if over.
    /// </summary>
    private static void DoAttack(bool random)
	{
        Point2D mouse = default (Point2D);

        if (random == false) 
        {


            mouse = SwinGame.MousePosition ();
        }

        if (random == true) 
        {
            mouse.X = _Random.Next(354,762);
            mouse.Y = _Random.Next (130, 539);
            
        }
		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height) {
			if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width) {
				GameController.Attack(row, col);
			}
		}
	}

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery()
	{
		const int SCORES_LEFT = 172;
		const int SHOTS_TOP = 157;
		const int HITS_TOP = 206;
		const int SPLASH_TOP = 256;

		if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c)) {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		} else {
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		}

		UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		UtilityFunctions.DrawMessage();

		SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
	}

}
