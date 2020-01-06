using System;
using System.Collections.Generic;
using CocosSharp;
using Android.Content;

namespace Swipe_Pipe
{
    public class Pipes : CCLayerColor
    {
        GameLayer layer;
        CCSprite pipe_1 = new CCSprite("pipe_1.png");
        CCSprite pipe_2 = new CCSprite("pipe_2.png");
        CCSprite ground = new CCSprite("ground.png");
        float pipe_gap;
        float pos_x;
        float pos_y;
        public Pipes() : base(CCColor4B.Transparent)
        {
        }
        public Pipes(GameLayer _layer, Context _context) : base(CCColor4B.Transparent)
        {
            layer = _layer;
            pos_y = layer.height / 2;
            pipe_1.AnchorPoint = new CCPoint(0.5f, 1);
            pipe_2.AnchorPoint = new CCPoint(0.5f, 0);
            pipe_1.Scale = layer.width / pipe_1.ContentSize.Width * 0.15f;
            pipe_2.Scale = layer.width / pipe_2.ContentSize.Width * 0.15f;
            ground.AnchorPoint = new CCPoint(0, 0);
            ground.Scale = layer.width / ground.ContentSize.Width;
            ground.Position = new CCPoint(0, -(ground.ScaledContentSize.Height / 2));
            pipe_gap = pipe_1.ScaledContentSize.Width * 1.5f;
            draw_children();
        }
        public void add_children()
        {
            layer.AddChild(pipe_1);
            layer.AddChild(pipe_2);
            layer.AddChild(ground);
        }
        public void remove_children()
        {
            layer.RemoveChild(pipe_1);
            layer.RemoveChild(pipe_2);
            layer.RemoveChild(ground);
        }
        public void set_position_x(float _pos_x)
        {
            pos_x = _pos_x;
        }
        public void set_position_y(float _pos_y)
        {
            pos_y = _pos_y;
        }
        public void draw_children()
        {
            pipe_1.Position = new CCPoint(pos_x, pos_y);
            pipe_2.Position = new CCPoint(pos_x, pos_y + pipe_gap);
        }
        public float get_width()
        {
            return pipe_1.BoundingBox.Size.Width;
        }
        public float get_ground_image_width()
        {
            return ground.BoundingBox.Size.Width;
        }
        public bool at_max()
        {
            return (pipe_1.BoundingBoxTransformedToParent.MaxY >= layer.height);
        }
        public bool at_min()
        {
            return (pipe_2.BoundingBoxTransformedToParent.MinY <= get_top_bounds_of_ground());
        }
        public float get_top_bounds_of_ground()
        {
            return ground.PositionY + ground.ScaledContentSize.Height;
        }
        public float get_pipe_gap()
        {
            return pipe_gap;
        }
        public float get_max()
        {
            return layer.height - 1;
        }
        public float get_min()
        {
            return get_top_bounds_of_ground() + 1 - pipe_gap;
        }
        public bool is_collide(CCSprite obj)
        {
            bool cwp1 = (pipe_1.BoundingBoxTransformedToParent.IntersectsRect(obj.BoundingBoxTransformedToParent));
            bool cwp2 = (pipe_2.BoundingBoxTransformedToParent.IntersectsRect(obj.BoundingBoxTransformedToParent));
            return (cwp1 || cwp2);
        }

    }
}