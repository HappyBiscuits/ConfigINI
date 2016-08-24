using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IniParser.Model;

namespace ConfigINI
{
    public class StringConfigValue<TSection, TSetting> : ConfigValue<String, TSection, TSetting>
    {
        public StringConfigValue(TSection section, TSetting setting, string defaultValue) : base(section, setting, defaultValue)
        {
        }

        public override void ReadData(IniData data)
        {
            Validate(data);
            Value = data[Section.ToString()][Setting.ToString()];
        }
    }
}
