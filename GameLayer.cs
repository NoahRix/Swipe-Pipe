using System;
using System.Collections.Generic;
using CocosSharp;
using Android.Content;
using Android.Media;
using Android.Gms.Ads;
namespace Swipe_Pipe
{
    public class GameLayer : CCLayerColor
    {
        MainActivity context;
        Random random = new Random();
        Background background;
        Pipes pipes;
        List<Bird> birds = new List<Bird>();
        GameOver gameover;
        CCLabel score_front;
        CCLabel score_shadow;
        MediaPlayer player_1;
        MediaPlayer player_2;
        public int width;
        public int height;
        public int game_state = 0;
        public int ad_count = 0;
        int sound_coin_id = Resource.Raw.coin;
        int hitmark_coin_id = Resource.Raw.hitmark;
        int score_count = 0;
        int main_game_tick = 0;
        int post_game_tick = 0;
        float bird_min;
        float bird_max;
        float pipes_y;
        float random_number;
        float touch_delta = 0;
        bool game_reset_flag;
        public GameLayer(MainActivity _context) : base(CCColor4B.Black)
        {
            context = _context;
            width = context.Resources.DisplayMetrics.WidthPixels;
            height = context.Resources.DisplayMetrics.HeightPixels;
            player_1 = MediaPlayer.Create(context, sound_coin_id);
            player_2 = MediaPlayer.Create(context, hitmark_coin_id);
            gameover = new GameOver(this);
            pipes = new Pipes(this, context);
            background = new Background(this, pipes);
            for (int i = 0; i < 1; i++)
                birds.Add(new Bird(this));
            score_front = new CCLabel("0", "Fonts/04B_19__.TTF", height * 0.15f);
            score_shadow = new CCLabel("0", "Fonts/04B_19__.TTF", height * 0.15f);

            Schedule(RunGameLogic);
        }
        void initiate()
        {
            pipes_y = height / 2;
            score_front.AnchorPoint = new CCPoint(0, 1.0f);
            score_shadow.AnchorPoint = new CCPoint(0, 1.0f);
            score_front.Position = new CCPoint(0, ContentSize.Height);
            score_shadow.Position = new CCPoint(10, ContentSize.Height - 10);
            score_shadow.Color = CCColor3B.Black;
            bird_min = pipes.get_top_bounds_of_ground() + height * 0.1f;
            bird_max = height - height * 0.1f;
            pipes.set_position_x(ContentSize.Width - pipes.get_width());
        }
        void RunGameLogic(float frameTimeInSeconds)
        {
            switch (game_state)
            {
                case 0:
                    pre_game();
                    break;
                case 1:
                    main_game();
                    break;
                case 2:
                    post_game();
                    break;
            }
        }
        void pre_game()
        {
            ad_count = 0;
            //context.show_ad();
            initiate();
            for (int i = 0; i < birds.Count; i++)
            {
                birds[i].set_position(new CCPoint(0, ContentSize.Height / 2 + pipes.get_pipe_gap()));
                birds[i].set_speed_x(width * 0.018f * (i + 1));
            }
            remove_children();
            draw_children();
            add_children();
        }
        void main_game()
        {
            main_game_tick += 1;
            remove_children();
            draw_children();
            background.scroll_clouds();
            bird_logic();
            add_children();
        }
        void post_game()
        {
            post_game_tick += 1;
            if (post_game_tick >= 30)
                game_reset_flag = true;
            for (int i = 0; i < birds.Count; i++)
                birds[i].set_position(birds[i].get_position());
            remove_children();
            draw_children();
            add_children();
        }

        void bird_logic()
        {
            for (int i = 0; i < birds.Count; i++)
            {
                if (main_game_tick % 20 < 10)
                    birds[i].set_frame(1);
                else
                    birds[i].set_frame(2);

                if (birds[i].get_position().X > ContentSize.Width)
                {
                    player_1.Start();
                    score_count++;
                    score_front.Text = score_count.ToString();
                    score_shadow.Text = score_count.ToString();
                    random_number = random.Next((int)bird_min, (int)bird_max);
                    birds[i].set_position(new CCPoint(0, random_number));
                }

                birds[i].move();

                if (pipes.is_collide(birds[i].get_sprite()))
                {
                    game_state = 2;
                    player_2.Start();
                    Unschedule(RunGameLogic);
                    Schedule(RunGameLogic);
                }
            }
        }
        void add_children()
        {
            background.add_children();
            pipes.add_children();
            AddChild(score_shadow);
            AddChild(score_front);
            for (int i = 0; i < birds.Count; i++)
                birds[i].add_children();
            gameover.add_children();
        }
        void remove_children()
        {
            gameover.remove_children();
            background.remove_children();
            pipes.remove_children();
            for (int i = 0; i < birds.Count; i++)
                birds[i].remove_children();
            RemoveChild(score_shadow);
            RemoveChild(score_front);
        }
        void draw_children()
        {
            background.draw_children();
            pipes.draw_children();
        }
        protected override void AddedToScene()
        {
            base.AddedToScene();
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            AddEventListener(touchListener, this);
        }
        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {

        }
        void HandleTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            touch_delta = touches[touches.Count - 1].Location.Y - touches[touches.Count - 1].PreviousLocation.Y;
            CCPoint loc = touches[touches.Count - 1].Location;
            switch (game_state)
            {
                case 0:
                    game_state = 1;
                    Schedule(RunGameLogic);
                    break;
                case 1:
                    pipes_y += touch_delta;
                    if (pipes.at_max())
                        pipes_y = pipes.get_max();
                    if (pipes.at_min())
                        pipes_y = pipes.get_min();
                    pipes.set_position_y(pipes_y);
                    break;
                case 2:
                    if (game_reset_flag)
                        if (gameover.get_button_region().ContainsPoint(touches[touches.Count - 1].Location))
                        {
                            game_state = 1;
                            post_game_tick = 0;
                            score_count = 0;
                            pipes_y = height / 2;
                            pipes.set_position_y(pipes_y);
                            score_front.Text = score_count.ToString();
                            score_shadow.Text = score_count.ToString();
                            for (int i = 0; i < birds.Count; i++)
                                birds[i].set_position(new CCPoint(0, ContentSize.Height / 2 + pipes.get_pipe_gap()));
                            game_reset_flag = false;
                            Unschedule(RunGameLogic);
                            Schedule(RunGameLogic);
                        }
                    break;
            }
        }
    }
}
