using System;
using System.Collections.Generic;
using CocosSharp;

namespace Swipe_Pipe
{
    class Background : CCNode
    {
        CCSizeI size;
        Random random = new Random(100);
        List<CCSprite> clouds_1 = new List<CCSprite>(); 
        List<CCSprite> clouds_2 = new List<CCSprite>();
        CCSprite city = new CCSprite("city.png");
        CCRect sky_region = new CCRect();
        CCDrawNode sky = new CCDrawNode();
        public Background() 
        {
        }
        public Background(CCSizeI size, Pipes pipes)
        {
            this.size = size;
            city.AnchorPoint = new CCPoint(0, 0);
            city.Scale = size.Width / city.ContentSize.Width;
            city.Position = new CCPoint(0, pipes.GetTopBoundsOfGround());
            for (int i = 0; i < 3; i++)
            {
                clouds_1.Add(new CCSprite("cloud-1.png"));
                clouds_2.Add(new CCSprite("cloud-2.png"));
                clouds_1[i].Opacity = 200; 
                clouds_2[i].Opacity = 200; 
                clouds_1[i].AnchorPoint = new CCPoint(0, 0);
                clouds_2[i].AnchorPoint = new CCPoint(0, 0);
                clouds_1[i].Position = new CCPoint(random.Next(0, size.Width), random.Next((int)(size.Height / 4), (int)(size.Height * 0.8f)));
                clouds_2[i].Position = new CCPoint(random.Next(0, size.Width), random.Next((int)(size.Height / 4), (int)(size.Height * 0.8f)));
                clouds_1[i].Scale = size.Height / clouds_1[i].ContentSize.Height * 0.1f;
                clouds_2[i].Scale = size.Height / clouds_2[i].ContentSize.Height * 0.1f;
            }
            Draw();
            Add();
        }
        public void ClearAll()
        {
            sky.Clear();
        }
        public void Add()
        {
            AddChild(sky);
            AddChild(city);
            for (int i = 0; i < clouds_1.Count; i++)
            {
                AddChild(clouds_1[i]);
                AddChild(clouds_2[i]);
            }
        }
        public void Remove()
        {
            RemoveChild(sky);
            RemoveChild(city);
            for (int i = 0; i < clouds_1.Count; i++)
            {
                RemoveChild(clouds_1[i]);
                RemoveChild(clouds_2[i]);
            }
        }
        public void scroll_clouds()
        {
            for (int i = 0; i < clouds_1.Count; i++)
            {
                clouds_1[i].PositionX += size.Width * 0.001f;
                clouds_2[i].PositionX += size.Width * 0.001f;
                if (clouds_1[i].PositionX >= size.Width)
                    clouds_1[i].PositionX = 0 - clouds_1[i].ScaledContentSize.Width;
                if (clouds_2[i].PositionX >= size.Width)
                    clouds_2[i].PositionX = 0 - clouds_2[i].ScaledContentSize.Width;
            }
        }
        public void Draw()
        {
            ClearAll();
            sky_region = new CCRect(0, 0, size.Width, size.Height);
            sky.DrawRect(sky_region, fillColor: new CCColor4B(47, 196, 255), borderWidth: 1, borderColor: CCColor4B.Transparent);
        }
    }
    
}
