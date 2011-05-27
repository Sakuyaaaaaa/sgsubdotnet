﻿using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace SGSDatatype
{
    [DataContract(Name = "SGSConfig", Namespace = "SGSDatatype")]
    public class SGSConfig
    {
        private string _filename;

        [DataMember]
        private string Version { get; set; }

        /// <summary>
        /// 布局名称
        /// </summary>
        [DataMember]
        public string LayoutName { get; set; }
        /// <summary>
        /// 起始时间点相对于按键时刻的偏移量（负为提前）（秒）
        /// </summary>
        [DataMember]
        public double StartOffset { get; set; }

        /// <summary>
        /// 终止时间点相对于按键时刻的偏移量（负为提前）（秒）
        /// </summary>
        [DataMember]
        public double EndOffset { get; set; }

        /// <summary>
        /// 快进、退的步长（秒）
        /// </summary>
        [DataMember]
        public double SeekStep { get; set; }

        /// <summary>
        /// 高亮的行定位于第几行
        /// </summary>
        [DataMember]
        public int SelectRowOffset { get; set; }

        /// <summary>
        /// 暂停、继续按键
        /// </summary>
        [DataMember]
        public Keys Pause { get; set; }

        /// <summary>
        /// 插入时间点按键（按下起始，抬起终止）
        /// </summary>
        [DataMember]
        public Keys AddTimePoint { get; set; }

        /// <summary>
        /// 插入单元格时间点按键（按下插入当前时间）
        /// </summary>
        [DataMember]
        public Keys AddCellTime { get; set; }

        /// <summary>
        /// 连续插入时间点（插入结束时间点和下一行开始时间点）
        /// </summary>
        [DataMember]
        public Keys AddContTimePoint { get; set; }

        /// <summary>
        /// 插入起始时间点按键
        /// </summary>
        [DataMember]
        public Keys AddStartTime { get; set; }

        /// <summary>
        /// 插入终止时间点按键
        /// </summary>
        [DataMember]
        public Keys AddEndTime { get; set; }

        /// <summary>
        /// 前进
        /// </summary>
        [DataMember]
        public Keys SeekForward { get; set; }

        /// <summary>
        /// 后退
        /// </summary>
        [DataMember]
        public Keys SeekBackword { get; set; }

        /// <summary>
        /// 跳至当前行
        /// </summary>
        [DataMember]
        public Keys GotoCurrent { get; set; }


        /// <summary>
        /// 跳至上一行
        /// </summary>
        [DataMember]
        public Keys GotoPrevious { get; set; }

        /// <summary>
        /// 进入编辑模式
        /// </summary>
        [DataMember]
        public Keys EnterEditMode { get; set; }


        [DataMember]
        public Keys SaveAss { get; set; }
        /// <summary>
        /// 默认格式定义行
        /// </summary>
        [DataMember]
        public string DefaultFormatLine { get; set; }

        [DataMember]
        public string DefaultFormat { get; set; }

        [DataMember]
        public string DefaultMarked { get; set; }

        [DataMember]
        public string DefaultLayer { get; set; }

        [DataMember]
        public double DefaultStart { get; set; }

        [DataMember]
        public double DefaultEnd { get; set; }

        [DataMember]
        public string DefaultStyle { get; set; }

        [DataMember]
        public string DefaultName { get; set; }

        [DataMember]
        public string DefaultActor { get; set; }

        [DataMember]
        public int DefaultMarginL { get; set; }

        [DataMember]
        public int DefaultMarginR { get; set; }

        [DataMember]
        public int DefaultMarginV { get; set; }

        [DataMember]
        public string DefaultEffect { get; set; }

        [DataMember]
        public bool AutoOverlapCorrection { get; set; }

        [DataMember]
        public string TemplateName { get; set; }

        /// <summary>
        /// 自动保存周期 秒
        /// </summary>
        [DataMember]
        public int AutoSavePeriod { get; set; }

        /// <summary>
        /// 自动保存数据保留时间 小时
        /// </summary>
        [DataMember]
        public int AutoSaveLifeTime { get; set; }


        public SGSConfig()
        {
        }

        /// <summary>
        /// Check weather the configuration object is compatible with this version.
        /// </summary>
        /// <param name="config">Configuration Object</param>
        /// <returns></returns>
        public bool Compatible(SGSConfig config)
        {
            return config.Version == Version;
        }

        public static SGSConfig FromFile(string filename)
        {
            var fs = new FileStream(filename,FileMode.Open,FileAccess.Read);

            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(SGSConfig));

            var sgsCfgObject = (SGSConfig)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            sgsCfgObject._filename = filename;
            return sgsCfgObject;
        }

        public void Save(string filename)
        {
            var writer = new FileStream(filename, FileMode.Create);
            var ser = new DataContractSerializer(typeof(SGSConfig));
            ser.WriteObject(writer, this);
            writer.Close();
            _filename = filename;
        }
        public void Save()
        {
            if (_filename != null)
            {
                Save(_filename);
            }
        }
        
    }
}
