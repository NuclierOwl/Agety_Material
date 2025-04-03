using System.IO;
using Agents_BD_Tres.Hardik.Connect.Dao;
using Avalonia.Media;
using Avalonia.Media.Imaging;

public class AgentViewModel //моде отображения огентов
{
    public AgentDao Agent { get; set; }
    public AgentTypeDao AgentType { get; set; }
    public int SalesCount { get; set; }
    public int Discount { get; set; }
    public string AgentImagePath { get; set; }
    public string TypeImagePath { get; set; }
    public IImage AgentImage
    {
        get
        {
            try
            {
                if (!string.IsNullOrEmpty(Agent.logo) && File.Exists(Agent.logo))
                {
                    return new Bitmap(Agent.logo);
                }
            }
            catch { }
            return new Bitmap("Assets/picture.png");
        }
    }
    public IImage TypeImage
    {
        get
        {
            try
            {
                if (AgentType != null && !string.IsNullOrEmpty(AgentType.image) && File.Exists(AgentType.image))
                {
                    return new Bitmap(AgentType.image);
                }
            }
            catch { }
            return null;
        }
    }
}