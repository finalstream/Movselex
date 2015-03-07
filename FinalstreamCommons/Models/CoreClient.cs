﻿using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace FinalstreamCommons.Models
{
    public abstract class CoreClient
    {

        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public void Initialize()
        {
            var execAssembly = AssemblyInfoData.ExecutingAssembly;
            _log.Info("ApplicationName: {0} {1}", execAssembly.Product, execAssembly.Version);
            InitializeCore();
        }

        /// <summary>
        /// 終了処理を行います。
        /// </summary>
        public void Finish()
        {
            FinalizeCore();
        }

        /// <summary>
        /// 設定をファイルからロードします。
        /// </summary>
        protected T LoadConfig<T>(string configFilePath) where T : IAppConfig, new()
        {
            if (!File.Exists(configFilePath)) return new T();
            return JsonConvert.DeserializeObject<T>(
                File.ReadAllText(configFilePath));
        }

        /// <summary>
        /// 設定をファイルに保存します。
        /// </summary>
        protected void SaveConfig<T>(string configFilePath, T config) where T : IAppConfig, new()
        {
            if (configFilePath == null) return;
            Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        protected abstract void InitializeCore();
        protected abstract void FinalizeCore();
    }
}