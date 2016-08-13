using IniParser.Model;

namespace ConfigINI
{
    public interface IDynamicConfig
    {
        string GetGroupName();
        void LoadData(IniData iniData);
        void SaveData(IniData iniData);
    }
}