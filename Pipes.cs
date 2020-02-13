using CocosSharp;

namespace Swipe_Pipe
{
    public class Pipes : CCNode
    {
        CCSizeI size = new CCSizeI();
        CCSprite pipe_1 = new CCSprite("pipe_1.png");
        CCSprite pipe_2 = new CCSprite("pipe_2.png");
        CCSprite ground = new CCSprite("ground.png");
        float pipe_gap;
        float pos_x;
        float pos_y;
        public Pipes()
        {
        }
        public Pipes(CCSizeI size)        
        {
            this.size = size;
            pos_y = size.Height / 2;
            pipe_1.AnchorPoint = new CCPoint(0.5f, 1);
            pipe_2.AnchorPoint = new CCPoint(0.5f, 0);
            pipe_1.Scale = size.Width / pipe_1.ContentSize.Width * 0.15f;
            pipe_2.Scale = size.Width / pipe_2.ContentSize.Width * 0.15f;
            ground.AnchorPoint = new CCPoint(0, 0);
            ground.Scale = size.Width / ground.ContentSize.Width;
            ground.Position = new CCPoint (0, -(ground.ScaledContentSize.Height / 2));
            pipe_gap = pipe_1.ScaledContentSize.Width * 1.5f;
            Draw();
            Add();
        }
        public void Add()
        {
            AddChild(pipe_1);
            AddChild(pipe_2);
            AddChild(ground);
        }
        public void Draw()
        {
            pipe_1.Position = new CCPoint(pos_x, pos_y);
            pipe_2.Position = new CCPoint(pos_x, pos_y + pipe_gap);
        }
        public void SetPositionX(float _pos_x)
        {
            pos_x = _pos_x;
        }
        public void SetPositionY(float _pos_y)
        {
            pos_y = _pos_y;
        }
        public float GetWidth()
        {
            return pipe_1.BoundingBox.Size.Width;
        }
        public float GetGroundImageWidth()
        {
            return ground.BoundingBox.Size.Width;
        }
        public bool AtMax()
        {
            return (pipe_1.BoundingBoxTransformedToParent.MaxY >= size.Height);
        }
        public bool AtMin()
        {
            return (pipe_2.BoundingBoxTransformedToParent.MinY <= GetTopBoundsOfGround());
        }
        public float GetTopBoundsOfGround()
        {
            return ground.PositionY + ground.ScaledContentSize.Height;
        }
        public float GetPipeGap()
        {
            return pipe_gap;
        }
        public float GetMax()
        {
            return size.Height - 1;
        }
        public float get_min()
        {
            return GetTopBoundsOfGround() + 1 - pipe_gap;
        }
        public bool IsCollide(CCSprite obj)
        {
            bool cwp1 = (pipe_1.BoundingBoxTransformedToParent.IntersectsRect(obj.BoundingBoxTransformedToParent));
            bool cwp2 = (pipe_2.BoundingBoxTransformedToParent.IntersectsRect(obj.BoundingBoxTransformedToParent));
            return (cwp1 || cwp2);
        }
      
    }
}
