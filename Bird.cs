using CocosSharp;

namespace Swipe_Pipe
{
    class Bird : CCNode
    {
        float speed = 0;
        CCSprite bird = new CCSprite("flappy1.jpg");
        CCSpriteFrame frame1 = new CCSpriteFrame(new CCTexture2D("flappy1"), new CCRect(0, 0, 395, 279));
        CCSpriteFrame frame2 = new CCSpriteFrame(new CCTexture2D("flappy2"), new CCRect(0, 0, 395, 279));
        public Bird()
        {
        }
        public Bird(CCSize size)
        {
            bird.AnchorPoint = new CCPoint(0, 0);
            bird.Scale = size.Width / bird.ContentSize.Width * 0.15f;
            Add();
        }
        public void Add()
        {
            AddChild(bird);
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
            switch(i)
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
