using System;
using System.Collections.Generic;
using CocosSharp;

namespace Swipe_Pipe
{
    class Bird : CCLayerColor
    {
        float speed = 0;
        GameLayer layer;
        CCSprite bird = new CCSprite("flappy1.jpg");
        CCSpriteFrame frame1 = new CCSpriteFrame(new CCTexture2D("flappy1"), new CCRect(0, 0, 395, 279));
        CCSpriteFrame frame2 = new CCSpriteFrame(new CCTexture2D("flappy2"), new CCRect(0, 0, 395, 279));
        public Bird() : base(CCColor4B.Transparent)
        {
        }
        public Bird(GameLayer _layer) : base(CCColor4B.Transparent)
        {
            layer = _layer;
            bird.AnchorPoint = new CCPoint(0, 0);
            bird.Scale = layer.width / bird.ContentSize.Width * 0.15f;
        }
        public void add_children()
        {
            layer.AddChild(bird);
        }
        public void remove_children()
        {
            layer.RemoveChild(bird);
        }
        public void set_position(CCPoint pos)
        {
            bird.Position = pos;
        }
        public CCPoint get_position()
        {
            return bird.Position;
        }
        public void set_speed_x(float _speed)
        {
            speed = _speed;
        }
        public void move()
        {
            bird.PositionX += speed;
        }
        public void set_frame(int i)
        {
            switch (i)
            {
                case 1:
                    bird.SpriteFrame = frame1;
                    break;
                case 2:
                    bird.SpriteFrame = frame2;
                    break;
                default:
                    break;
            }
        }
        public CCSprite get_sprite()
        {
            return bird;
        }
        public float sprite_height()
        {
            return bird.ScaledContentSize.Height;
        }
    }
}