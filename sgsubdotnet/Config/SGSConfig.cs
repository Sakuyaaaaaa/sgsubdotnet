﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace Config
{
    [DataContract(Name ="SGSConfig",Namespace="Config")]
    public class SGSConfig
    {
        private string m_filename;
        /// <summary>
        /// 起始时间点相对于按键时刻的偏移量（负为提前）（秒）
        /// </summary>
        [DataMember()]
        public double StartOffset { get; set; }

        /// <summary>
        /// 终止时间点相对于按键时刻的偏移量（负为提前）（秒）
        /// </summary>
        [DataMember()]
        public double EndOffset { get; set; }

        /// <summary>
        /// 快进、退的步长（秒）
        /// </summary>
        [DataMember()]
        public double SeekStep { get; set; }

        /// <summary>
        /// 高亮的行定位于第几行
        /// </summary>
        [DataMember()]
        public int SelectRowOffset { get; set; }

        /// <summary>
        /// 暂停、继续按键
        /// </summary>
        [DataMember()]
        public Keys Pause { get; set; }

        /// <summary>
        /// 插入时间点按键（按下起始，抬起终止）
        /// </summary>
        [DataMember()]
        public Keys AddTimePoint { get; set; }

        /// <summary>
        /// 插入起始时间点按键
        /// </summary>
        [DataMember()]
        public Keys AddStartTime { get; set; }

        /// <summary>
        /// 插入终止时间点按键
        /// </summary>
        [DataMember()]
        public Keys AddEndTime { get; set; }

        /// <summary>
        /// 前进
        /// </summary>
        [DataMember()]
        public Keys SeekForward { get; set; }

        /// <summary>
        /// 后退
        /// </summary>
        [DataMember()]
        public Keys SeekBackword { get; set; }

        /// <summary>
        /// 默认ass文件头
        /// </summary>
        [DataMember()]
        public Subtitle.AssHead DefaultAssHead { get; set; }

        /// <summary>
        /// 默认格式定义行
        /// </summary>
        [DataMember()]
        public string DefaultFormatLine { get; set; }

        public SGSConfig()
        {
            Pause = Keys.Space;
            AddTimePoint = Keys.A;
            SeekBackword = Keys.Q;
            SelectRowOffset = 2;
            SeekStep = 2;

        }

        static SGSConfig FromFile(string filename)
        {
            SGSConfig SGSCfgObject;
            FileStream fs = new FileStream(filename, FileMode.Open);

            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(SGSConfig));

            SGSCfgObject = (SGSConfig)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            SGSCfgObject.m_filename = filename;
            return SGSCfgObject;
        }

        public void Save(string filename)
        {
            FileStream writer = new FileStream(filename, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(SGSConfig));
            ser.WriteObject(writer, this);
            writer.Close();
            m_filename = filename;
        }
        
    }
}
