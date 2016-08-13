using IniParser.Model;

namespace ConfigINI
{
    public interface IConfigValue<out TSection, out TValues>
    {
        TSection GetSection();
        TValues GetSetting();
        void WriteData(IniData data);
        void ReadData(IniData data);
    }
}