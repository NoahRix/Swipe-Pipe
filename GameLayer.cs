/*******************************************************
 * Programmer: Noah Rix
 * Date Created: November 1, 2019
 *******************************************************/

using System;
using System.Collections.Generic;
using CocosSharp;
using Android.Content;
using Android.Media;
using Android.Gms.Ads;
namespace Swipe_Pipe
{
    public class GameLayer : CCLayer
    {
        Random random = new Random();                   //For random number generation.
        Background background;                          //Background nodes.
        Pipes pipes;                                    //Pipe nodes.
        List<Bird> birds = new List<Bird>();            //List of birds.
        ActionMenu actionMenu;                          //Start and gameover action menus.
        CCLabel scoreFront;                             //Score counter.
        CCLabel scoreShadow;                            //Score counter shadow. (just another text layer but black font color.)
        MediaPlayer player_1;                           //For sound effect.
        MediaPlayer player_2;                           //For sound effect.
        public int width;                               //Absoute screen width by pixel.
        public int height;                              //Absoute screen height by pixel.    
        public int gameState = 0;                       //Value for game state. Pre, main, and, post.
        public int adCount = 0;                         //How many gameovers per one interstitial ad.
        int soundCoinId = Resource.Raw.coin;            //Coin sound effect.
        int hitmarkCoinId = Resource.Raw.hitmark;       //Hitmark sound effect.
        int scoreCount = 0;                             //Score counter value.
        int mainGameTick = 0;                           //Main game tick for time tracking.
        int postGameTick = 0;                           //Post game tick for time tracking.
        float birdMin;                                  //Minimum pixel height for a bird.
        float birdMax;                                  //Maximum pixel height for a bird.
        float pipesY;                                   //Vertical postion of the pipes in pixels.
        float randomNumber;                             //Random number for bird replace position.
        float touchDelta = 0;                           //Distance between two consecutive touches.
        bool gameResetFlag;                             //If the game is ready to restart.

        public GameLayer(MainActivity context)
        {
            width = context.screenSize.Width;
            height = context.screenSize.Height;
            player_1 = MediaPlayer.Create(context, soundCoinId);
            player_2 = MediaPlayer.Create(context, hitmarkCoinId);
            actionMenu = new ActionMenu(context.screenSize);
            pipes = new Pipes(context.screenSize);
            background = new Background(context.screenSize, pipes);
            for (int i = 0; i < 1; i++)
                birds.Add(new Bird(context.screenSize));
            scoreFront = new CCLabel("0", "Fonts/04B_19__.TTF", height * 0.15f);
            scoreShadow = new CCLabel("0", "Fonts/04B_19__.TTF", height * 0.15f);

            //Run the game logic in a loop set to 60fps.
            Schedule(RunGameLogic);
        }

        //Initiate starting values.
        void initiate()
        {
            pipesY = height / 2;
            scoreFront.AnchorPoint = new CCPoint(0, 1.0f);
            scoreShadow.AnchorPoint = new CCPoint(0, 1.0f);
            scoreFront.Position = new CCPoint(0, ContentSize.Height);
            scoreShadow.Position = new CCPoint(10, ContentSize.Height - 10);
            scoreShadow.Color = CCColor3B.Black;
            birdMin = pipes.GetTopBoundsOfGround() + height * 0.1f;
            birdMax = height - height * 0.1f;
            pipes.SetPositionX(ContentSize.Width - pipes.GetWidth());   
        }

        //Based on the gamestate value, we shall know whenever the game is ready to start, has started, or is over.
        void RunGameLogic(float frameTimeInSeconds)
        {
            switch (gameState)
            {
                case 0:
                    PreGame();
                    break;
                case 1:
                    MainGame();
                    break;
                case 2:
                    PostGame();
                    break;
            }
        }

        //Pree game initiates starting values, clear the screen and draw the respected nodes.
        void PreGame()
        {
            adCount = 0;
            //context.show_ad();
            initiate();

            //The bird's initial position is the exact vertical mid-point of the screen plus the size of the pipe gap.
            //The bird's speed is about 1/55ths of the width of the screen per 1/60ths of a second.
            for (int i = 0; i < birds.Count; i++)
            {
                birds[i].set_position(new CCPoint(0, ContentSize.Height / 2 + pipes.GetPipeGap()));
                birds[i].set_speed_x(width * 0.018f * (i + 1));
            }
            RemoveChildren();
            DrawChildren();
            AddChildren();
        }

        //during the main game, the background animates the clouds.
        void MainGame()
        {
            mainGameTick += 1;
            if (mainGameTick > 3600)
                mainGameTick = 0;
            RemoveChildren();
            DrawChildren();
            background.scroll_clouds();
            BirdLogic();
            AddChildren();
        }
        void PostGame()
        {
            postGameTick += 1;
            if (postGameTick >= 30)
                gameResetFlag = true;
            for (int i = 0; i < birds.Count; i++)
                birds[i].set_position(birds[i].get_position());
            RemoveChildren();
            DrawChildren();
            AddChildren();
        }

        void BirdLogic()
        {
            for (int i = 0; i < birds.Count; i++)
            {
                if (mainGameTick % 20 < 10)
                    birds[i].set_frame(1);
                else
                    birds[i].set_frame(2);

                if (birds[i].get_position().X > ContentSize.Width)
                {
                    player_1.Start();
                    scoreCount++;
                    scoreFront.Text = scoreCount.ToString();
                    scoreShadow.Text = scoreCount.ToString();
                    randomNumber = random.Next((int)birdMin, (int)birdMax);
                    birds[i].set_position(new CCPoint(0, randomNumber));
                }

                birds[i].move();

                if (pipes.IsCollide(birds[i].get_sprite()))
                {
                    gameState = 2;
                    player_2.Start();
                    Unschedule(RunGameLogic);
                    Schedule(RunGameLogic);
                }
            }
        }
        void AddChildren()
        {
            AddChild(background);
            AddChild(pipes);
            AddChild(scoreShadow);
            AddChild(scoreFront);
            for (int i = 0; i < birds.Count; i++)
                AddChild(birds[i]);
            if (gameState == 0)
            {
                actionMenu.AddStart();
                AddChild(actionMenu);
            }
            if (gameState == 2)
            {
                actionMenu.AddRestart();
                AddChild(actionMenu);
            }
        }
        void RemoveChildren()
        {
            RemoveChild(background);
            for (int i = 0; i < birds.Count; i++)
                RemoveChild(birds[i]);
            RemoveChild(scoreShadow);
            RemoveChild(scoreFront);
            RemoveChild(pipes);
            RemoveChild(actionMenu);
        }
        void DrawChildren()
        {
            background.Draw();
            pipes.Draw();
        }
        protected override void AddedToScene()
        {
            base.AddedToScene();
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            AddEventListener(touchListener, this);
        }
        void HandleTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            touchDelta = touches[touches.Count - 1].Location.Y - touches[touches.Count - 1].PreviousLocation.Y;
            CCPoint loc = touches[touches.Count - 1].Location;
            switch (gameState)
            {
                case 0:
                    gameState = 1;
                    Schedule(RunGameLogic);
                    break;
                case 1:
                    pipesY += touchDelta;
                    if (pipes.AtMax())
                        pipesY = pipes.GetMax();
                    if (pipes.AtMin())
                        pipesY = pipes.get_min();
                    pipes.SetPositionY(pipesY);
                    break;
                case 2: 
                    if (gameResetFlag)
                        if (actionMenu.get_button_region().ContainsPoint(touches[touches.Count - 1].Location))
                        {  
                            gameState = 1;
                            postGameTick = 0;
                            scoreCount = 0;
                            pipesY = height / 2;
                            pipes.SetPositionY(pipesY);
                            scoreFront.Text = scoreCount.ToString();
                            scoreShadow.Text = scoreCount.ToString();
                            for (int i = 0; i < birds.Count; i++)
                                birds[i].set_position(new CCPoint(0, ContentSize.Height / 2 + pipes.GetPipeGap()));
                            gameResetFlag = false;
                            Unschedule(RunGameLogic);
                            Schedule(RunGameLogic);
                        }
                    break;
            }
        }
    }
}
