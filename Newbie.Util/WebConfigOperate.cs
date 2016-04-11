namespace Newbie.Util
{
    public class WebConfigOperate
    {
        #region GetSettings
        public static string GetAppSetting(string key)
        {
            return ConfigurationUtil.GetAppSettingValue(key);
        }

        public static string GetConnectionString(string key)
        {
            return ConfigurationUtil.GetConnectionString(key);
        }
        #endregion
    }
}
