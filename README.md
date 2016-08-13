# ConfigINI
INI Based Configuration System

## Basic Use

1. Decide what to use for IDs. (Enums are a good choice)

   Section:
   ```csharp
public enum ConfigSection
    {
        Monitor,
        Logging
    }
   ```
   Setting:
   ```csharp
    public enum ConfigSetting
    {
        Frequency,
        TimeOut,
        SleepPeriod,
        LogPingReply,
        LogActiveFail
    }
   ```

2. Init the `Config<TSection, TSetting>` Class

   ```csharp
   var config = new Config<ConfigSection, ConfigSetting>("Config.ini", new List<IConfigValue<ConfigSection, ConfigSetting>>()
     {
       new FloatConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Monitor, ConfigSetting.Frequency, 3f),
       new IntConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Monitor, ConfigSetting.TimeOut, 4000),
       new IntConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Monitor, ConfigSetting.SleepPeriod, 1000),
       new BoolConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Logging, ConfigSetting.LogPingReply, true),
       new BoolConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Logging, ConfigSetting.LogActiveFail, true),
   });
   ```

   The variables passed to the config values are the secion, setting and the default value for the setting.  

   By default ConfigINI can handle Floats, Ints and Bools however it can be easily extended.  

3. Load the Config INI

   ```csharp
   config.LoadConfig();
   ```
   This will check to see if the INI file exists and if not it will populate the settings with the default value.

4. Save the Config INI

   ```csharp
   config.SaveConfig();
   ```
   
   This will write the default values if the INI file did not exist.

5. Access the Setting

   ```csharp
   float freq = config.GetFloatSetting(ConfigSection.Monitor, ConfigSetting.Frequency);  
   ```
   
6. Write to the Setting
   ```csharp
   int val = 0;
   config.SetIntSetting(ConfigSection.Monitor, ConfigSetting.TimeOut, val);
   ```
   
   This writes to the setting and then writes to the INI file.

### Example Output

```
[Monitor]
Frequency = 5.34
TimeOut = 3000
SleepPeriod = 1000

[Logging]
LogPingReply = True
LogActiveFail = True
```

## Dynamic Use

ConfigINI supports dynamic numbered sections for storing lists of objects.

1. Create storage object

   ```csharp
   var ipAddressConfig = new DynamicConfig<IpAddressData>("IpAddress", SetIpAddressData, GetIpAddressData,
                new List<IpAddressData>()
                {
                new IpAddressData("Google DNS", IPAddress.Parse("8.8.8.8")),
                new IpAddressData("Level 3", IPAddress.Parse("4.2.2.2")),
                new IpAddressData("Open DNS", IPAddress.Parse("208.67.222.222")),
                });
   ```
   The first argument is the group name, this is the section the values will be under (e.g. "IpAddress0").
   
   SetIpAddressData is a method that creates an IpAddressData class from INI Data
   ```csharp
        private IpAddressData SetIpAddressData(KeyDataCollection data)
        {
            IPAddress ip;
            if (IPAddress.TryParse(data["Address"], out ip))
            {
                return new IpAddressData(data["Name"], ip);
            }
            throw new InvalidCastException("Not a valid IpAddress");
        }
   ```
   
   GetIpAddressData is a method that writes an IpAddressData class to the INI Data.
   ```csharp
        private void GetIpAddressData(IpAddressData addr, KeyDataCollection data )
        {
            data.AddKey("Name", addr.Name);
            data.AddKey("Address", addr.Ip.ToString());
        }
   ```
   
   You can then pass a list of default values.
   
2. Add the data to the config 

   ```csharp
               var ipAddressConfig = new DynamicConfig<IpAddressData>("IpAddress", SetIpAddressData, GetIpAddressData,
                new List<IpAddressData>()
                {
                new IpAddressData("Google DNS", IPAddress.Parse("8.8.8.8")),
                new IpAddressData("Level 3", IPAddress.Parse("4.2.2.2")),
                new IpAddressData("Open DNS", IPAddress.Parse("208.67.222.222")),
                });

            var config = new Config<ConfigSection, ConfigSetting>("Config.ini", new List<IConfigValue<ConfigSection, ConfigSetting>>()
            {
            new FloatConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Monitor, ConfigSetting.Frequency, 3f),
            new IntConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Monitor, ConfigSetting.TimeOut, 4000),
            new IntConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Monitor, ConfigSetting.SleepPeriod, 1000),
            new BoolConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Logging, ConfigSetting.LogPingReply, true),
            new BoolConfigValue<ConfigSection, ConfigSetting>(ConfigSection.Logging, ConfigSetting.LogActiveFail, true),
            }, new List<IDynamicConfig>()
            {
                ipAddressConfig,
            });
   ```

2. Access the data

   You can either access the `DynamicConfig<T>` class directly, or you can access it from the config class.
   ```csharp
   var dynamic = (DynamicConfig<IpAddressData>)config.GetDynamicConfig("IpAddress");
   ```
   The data is stored in a List<T> called Data.
  ```csharp
   var dynamic = (DynamicConfig<IpAddressData>)config.GetDynamicConfig("IpAddress");
   foreach(var ip in dynamic.Data)
   {
      //Do Something...
   }
   ```
   
3. Save the data

   After Modifying the Data list be sure to run `config.SaveConfig();` to write to the INI file.
   
### Example Output

```
[IpAddress0]
Name = Google DNS
Address = 8.8.8.8

[IpAddress1]
Name = Level 3
Address = 4.2.2.2

[IpAddress2]
Name = Open DNS
Address = 208.67.222.222

[IpAddress3]
Name = LocalHost
Address = 127.0.0.1
```
