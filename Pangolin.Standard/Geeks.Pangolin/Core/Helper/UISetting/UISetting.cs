﻿using System.Diagnostics;
using System.IO;
using Geeks.Pangolin.Core.Parameters;

namespace Geeks.Pangolin.Core.Helper
{
    public class UISetting
    {
        #region [Property]

        public string AppBaseUrl { get; set; }
        public string DownloadUrl { get; set; }
        public bool Headless { get; set; } = !Debugger.IsAttached;
        public Browser Browser { get; set; } = Browser.Chrome;

        private static UISetting instance = null;
        public static UISetting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UISetting();
                    instance.Init();
                }
                return instance;
            }
        }


        #endregion

        #region [Constructor]

        private UISetting() { }

        #endregion

        #region [Public Method]

        #endregion

        #region [Private Method]

        private void Init()
        {
            this.AppBaseUrl = ConfigHelper.Instance.Get($"{ConfigHelper.AppSettingsSection}:AppBaseUrl").GetWellFormedUrl();

            this.DownloadUrl = ConfigHelper.Instance.Get($"{ConfigHelper.AppSettingsSection}:Download.Url", Path.Combine(AppConstants.ProjectPath, "Download")).AsDirectory().EnsureExists().FullName;

            this.Browser = ConfigHelper.Instance.Get($"{ConfigHelper.AppSettingsSection}:Browser", Browser.Chrome);

            this.Headless = ConfigHelper.Instance.Get($"{ConfigHelper.AppSettingsSection}:Headless", !Debugger.IsAttached);

        }

        #endregion
    }
}
