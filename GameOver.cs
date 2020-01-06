using System;
using System.Collections.Generic;
using CocosSharp;
using Android.Content;

namespace Swipe_Pipe
{
    class GameOver : CCLayerColor
    {
        float scale_factor = 0.60f;
        GameLayer layer;
        CCSprite sprite_1 = new CCSprite("gameover.png");
        CCSprite start_button = new CCSprite("start_button.png");
        CCSprite restart_button = new CCSprite("restart_button.png");

        public GameOver()
        {
        }
        public GameOver(GameLayer _layer)
        {
            layer = _layer;
            sprite_1.Position = new CCPoint(layer.width / 2, layer.height * 0.7f);
            sprite_1.Scale = ((layer.width / sprite_1.ContentSize.Width) * scale_factor);
            restart_button.Scale = ((layer.width / restart_button.ContentSize.Width) * scale_factor * 0.7f);
            start_button.Scale = ((layer.width / start_button.ContentSize.Width) * scale_factor * 0.7f);
            restart_button.Position = new CCPoint(layer.width / 2, sprite_1.Position.Y - restart_button.ScaledContentSize.Height);
            start_button.Position = new CCPoint(layer.width / 2, layer.height * 0.6f);
        }
        public void add_children()
        {
            if (layer.game_state == 0)
            {
                layer.AddChild(start_button);
            }
            if (layer.game_state == 2)
            {
                layer.AddChild(sprite_1);
                layer.AddChild(restart_button);
            }
        }
        public void remove_children()
        {
            if (layer.game_state == 0)
            {
                layer.RemoveChild(start_button);
            }
            if (layer.game_state == 2)
            {
                layer.RemoveChild(sprite_1);
                layer.RemoveChild(restart_button);
            }
        }
        public CCRect get_button_region()
        {
            return restart_button.BoundingBoxTransformedToParent;
        }
    }
}