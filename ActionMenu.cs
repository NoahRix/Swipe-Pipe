using System;
using System.Collections.Generic;
using CocosSharp;
using Android.Content;

namespace Swipe_Pipe
{
    class ActionMenu : CCNode
    {
        float scale_factor = 0.60f;
        CCSprite sprite_1 = new CCSprite("gameover.png");
        CCSprite start_button = new CCSprite("start_button.png");
        CCSprite restart_button = new CCSprite("restart_button.png");

        public ActionMenu()
        {
        }
        public ActionMenu(CCSizeI size)
        {
            sprite_1.Position = new CCPoint(size.Width / 2, size.Height * 0.7f);
            sprite_1.Scale = ((size.Width / sprite_1.ContentSize.Width) * scale_factor);
            restart_button.Scale = ((size.Width / restart_button.ContentSize.Width) * scale_factor * 0.7f);
            start_button.Scale = ((size.Width / start_button.ContentSize.Width) * scale_factor * 0.7f);
            restart_button.Position = new CCPoint(size.Width / 2, sprite_1.Position.Y - restart_button.ScaledContentSize.Height);
            start_button.Position = new CCPoint(size.Width / 2, size.Height * 0.6f);
        }
        public void AddStart()
        {
            AddChild(start_button);
        }
        public void AddRestart()
        {
            AddChild(sprite_1);
            AddChild(restart_button);
        }
        public void RemoveStart()
        {
            RemoveChild(start_button);
        }
        public void RemoveRestart()
        {
            RemoveChild(sprite_1);
            RemoveChild(restart_button);
        }
        public CCRect get_button_region()
        {
            return restart_button.BoundingBoxTransformedToParent;
        }
    }
}
