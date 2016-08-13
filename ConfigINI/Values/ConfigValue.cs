using IniParser.Model;

namespace ConfigINI
{
    public abstract class ConfigValue<T, TSection, TSetting> : IConfigValue<TSection, TSetting>
    {
        public TSection Section;
        public TSetting Setting;
        public T Value;
        public T DefaultValue;

        public TSection GetSection()
        {
            return Section;
        }

        public TSetting GetSetting()
        {
            return Setting;
        }

        public virtual void WriteData(IniData data)
        {
            Validate(data);
            data[Section.ToString()][Setting.ToString()] = Value.ToString();
        }

        protected ConfigValue(TSection section, TSetting setting, T defaultValue)
        {
            Section = section;
            Setting = setting;
            DefaultValue = defaultValue;
        }

        protected void Validate(IniData data)
        {
            if (!data.Sections.ContainsSection(Section.ToString()))
            {
                data.Sections.AddSection(Section.ToString());
            }
            if (!data[Section.ToString()].ContainsKey(Setting.ToString()))
            {
                data[Section.ToString()].AddKey(Setting.ToString());
                data[Section.ToString()][Setting.ToString()] = DefaultValue.ToString();
            }

        }

        public abstract void ReadData(IniData data);

    }
}