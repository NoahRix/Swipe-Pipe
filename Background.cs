using System;
using System.Collections.Generic;
using CocosSharp;
using Android.Content;

namespace Swipe_Pipe
{
    class Background : CCLayerColor
    {
        Random random = new Random(100);
        GameLayer layer;
        List<CCSprite> clouds_1 = new List<CCSprite>();
        List<CCSprite> clouds_2 = new List<CCSprite>();
        CCSprite city = new CCSprite("city.png");
        CCRect sky_region = new CCRect();
        CCDrawNode sky = new CCDrawNode();
        public Background() : base(CCColor4B.Transparent)
        {
        }
        public Background(GameLayer _layer, Pipes pipes) : base(CCColor4B.Transparent)
        {
            layer = _layer;
            city.AnchorPoint = new CCPoint(0, 0);
            city.Scale = layer.width / city.ContentSize.Width;
            city.Position = new CCPoint(0, pipes.get_top_bounds_of_ground());
            for (int i = 0; i < 3; i++)
            {
                clouds_1.Add(new CCSprite("cloud-1.png"));
                clouds_2.Add(new CCSprite("cloud-2.png"));
                clouds_1[i].Opacity = 200;
                clouds_2[i].Opacity = 200;
                clouds_1[i].AnchorPoint = new CCPoint(0, 0);
                clouds_2[i].AnchorPoint = new CCPoint(0, 0);
                clouds_1[i].Position = new CCPoint(random.Next(0, layer.width), random.Next((int)(layer.height / 4), (int)(layer.height * 0.8f)));
                clouds_2[i].Position = new CCPoint(random.Next(0, layer.width), random.Next((int)(layer.height / 4), (int)(layer.height * 0.8f)));
                clouds_1[i].Scale = layer.height / clouds_1[i].ContentSize.Height * 0.1f;
                clouds_2[i].Scale = layer.height / clouds_2[i].ContentSize.Height * 0.1f;
            }
            draw_children();
        }
        public void clear()
        {
            sky.Clear();
        }
        public void add_children()
        {
            layer.AddChild(sky);
            layer.AddChild(city);
            for (int i = 0; i < clouds_1.Count; i++)
            {
                layer.AddChild(clouds_1[i]);
                layer.AddChild(clouds_2[i]);
            }
        }
        public void remove_children()
        {
            layer.RemoveChild(sky);
            layer.RemoveChild(city);
            for (int i = 0; i < clouds_1.Count; i++)
            {
                layer.RemoveChild(clouds_1[i]);
                layer.RemoveChild(clouds_2[i]);
            }
        }
        public void scroll_clouds()
        {
            for (int i = 0; i < clouds_1.Count; i++)
            {
                clouds_1[i].PositionX += layer.width * 0.001f;
                clouds_2[i].PositionX += layer.width * 0.001f;
                if (clouds_1[i].PositionX >= layer.width)
                    clouds_1[i].PositionX = 0 - clouds_1[i].ScaledContentSize.Width;
                if (clouds_2[i].PositionX >= layer.width)
                    clouds_2[i].PositionX = 0 - clouds_2[i].ScaledContentSize.Width;
            }
        }
        public void draw_children()
        {
            clear();
            sky_region = new CCRect(0, 0, layer.ContentSize.Width, layer.ContentSize.Height);
            sky.DrawRect(sky_region, fillColor: new CCColor4B(47, 196, 255), borderWidth: 1, borderColor: CCColor4B.Transparent);
        }
    }

}