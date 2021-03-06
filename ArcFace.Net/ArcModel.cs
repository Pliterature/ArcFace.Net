﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ArcFace.Net
{
    public class ArcModel
    {
        /// <summary>
        /// 人脸库
        /// </summary>
        internal class FaceLib
        {
            public List<Item> Items { get; set; } = new List<Item>();
            public class Item
            {
                /// <summary>
                /// 用于排序
                /// </summary>
                public long OrderId { get; set; }
                /// <summary>
                /// 文件名作为ID
                /// </summary>
                public string ID { get; set; }
                /// <summary>
                /// 人脸模型
                /// </summary>
                public FaceModel FaceModel;// { get; set; }
            }
        }
        /// <summary>
        /// 人脸识别结果集
        /// </summary>
        public class FaceResults
        {
            public List<FaceResult> Items { get; set; }
            public int FaceNumber { get; set; }
            public FaceResults(int maxFaceNumber)
            {
                Items = new List<FaceResult>();
                for (int i = 0; i < maxFaceNumber; i++)
                {
                    Items.Add(new FaceResult());
                }
            }
            public FaceResult this[int index]
            {
                get
                {
                    return Items[index];
                }
                set
                {
                    Items[index] = value;
                }
            }

        }
        /// <summary>
        /// 人脸识别结果
        /// </summary>
        public class FaceResult
        {
            public string ID { get; set; }
            public System.Drawing.Rectangle Rectangle
            {
                get
                {
                    return new System.Drawing.Rectangle(FFI.FaceRect.Left, FFI.FaceRect.Top, FFI.FaceRect.Right - FFI.FaceRect.Left, FFI.FaceRect.Bottom - FFI.FaceRect.Top);
                }
            }
            public byte[] GetFeatureData()
            {
                var data = new byte[22020];
                Marshal.Copy(FaceModel.PFeature, data, 0, 22020);
                return data;
            }
            public float Score { get; set; }
            internal FaceFeatureInput FFI = new FaceFeatureInput() { Orient = 1 };
            internal FaceModel FaceModel = new FaceModel() { Size = 22020, PFeature = Marshal.AllocCoTaskMem(22020) };
        }

        public class FaceMatchResults
        {
            public List<FaceMatchResult> Items { get; set; }
        }

        public class FaceMatchResult
        {
            public string ID { get; set; }
            public float Score { get; set; }
        }

        /// <summary>
        /// 人脸跟踪、检测、性别年龄评估和获取人脸信息的输入参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct ImageData
        {
            public uint PixelArrayFormat;
            public int Width;
            public int Height;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public IntPtr[] ppu8Plane;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I4)]
            public int[] Pitch;
        }

        /// <summary>
        /// 人脸检测的结果
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DetectResult
        {
            [MarshalAs(UnmanagedType.I4)]
            public int FaceCout;
            public IntPtr PFaceRect;
            public IntPtr PEFaceOrient;
        }

        /// <summary>
        /// 人脸在图片中的位置
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct FaceRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        /// <summary>
        /// 获取人脸特征的输入参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct FaceFeatureInput
        {
            public FaceRect FaceRect;
            public int Orient;
        }
        /// <summary>
        /// 人脸特征
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FaceModel
        {
            public IntPtr PFeature;
            [MarshalAs(UnmanagedType.I4)]
            public int Size;
        }


        /// <summary>
        /// 错误代码
        /// </summary>
        internal enum ErrorCode
        {
            /// <summary>
            /// 正确
            /// </summary>
            [Description("正确")]
            Ok = 0,

            /// <summary>
            /// 通用错误类型
            /// </summary>
            [Description("错误原因不明")]
            BasicBase = 0x0001,

            /// <summary>
            /// 错误原因不明
            /// </summary>
            [Description("错误原因不明")]
            Unknown = BasicBase,

            /// <summary>
            /// 无效的参数
            /// </summary>
            [Description("无效的参数")]
            InvalidParam = BasicBase + 1,

            /// <summary>
            /// 引擎不支持
            /// </summary>
            [Description("引擎不支持")]
            Unsupported = BasicBase + 2,

            /// <summary>
            /// 内存不足
            /// </summary>
            [Description("内存不足")]
            NoMemory = BasicBase + 3,

            /// <summary>
            /// 状态错误
            /// </summary>
            [Description("状态错误")]
            BadState = BasicBase + 4,

            /// <summary>
            /// 用户取消相关操作
            /// </summary>
            [Description("用户取消相关操作")]
            UserCancel = BasicBase + 5,

            /// <summary>
            /// 操作时间过期
            /// </summary>
            [Description("操作时间过期")]
            Expired = BasicBase + 6,

            /// <summary>
            /// 用户暂停操作
            /// </summary>
            [Description("用户暂停操作")]
            UserPause = BasicBase + 7,

            /// <summary>
            /// 缓冲上溢
            /// </summary>
            [Description("缓冲上溢")]
            BufferOverflow = BasicBase + 8,

            /// <summary>
            /// 缓冲下溢
            /// </summary>
            [Description("缓冲下溢")]
            BufferUnderflow = BasicBase + 9,

            /// <summary>
            /// 存贮空间不足
            /// </summary>
            [Description("存贮空间不足")]
            NoDiskspace = BasicBase + 10,

            /// <summary>
            /// 组件不存在
            /// </summary>
            [Description("组件不存在")]
            ComponentNotExist = BasicBase + 11,

            /// <summary>
            /// 全局数据不存在
            /// </summary>
            [Description("全局数据不存在")]
            GlobalDataNotExist = BasicBase + 12,

            /// <summary>
            /// Free SDK通用错误类型
            /// </summary>
            [Description("SDK通用错误类型")]
            SdkBase = 0x7000,

            /// <summary>
            /// 无效的App Id
            /// </summary>
            [Description("无效的App Id")]
            InvalidAppId = SdkBase + 1,

            /// <summary>
            /// 无效的SDK key
            /// </summary>
            [Description("无效的SDK key")]
            InvalidSdkId = SdkBase + 2,

            /// <summary>
            /// AppId和SDKKey不匹配
            /// </summary>
            [Description("AppId和SDKKey不匹配")]
            InvalidIdPair = SdkBase + 3,

            /// <summary>
            /// SDKKey 和使用的SDK 不匹配
            /// </summary>
            [Description("SDKKey 和使用的SDK 不匹配")]
            MismatchIdAndSdk = SdkBase + 4,

            /// <summary>
            /// 系统版本不被当前SDK所支持
            /// </summary>
            [Description("系统版本不被当前SDK所支持")]
            SystemVersionUnsupported = SdkBase + 5,

            /// <summary>
            /// SDK有效期过期，需要重新下载更新
            /// </summary>
            [Description("SDK有效期过期，需要重新下载更新")]
            LicenceExpired = SdkBase + 6,

            /// <summary>
            /// Face Recognition错误类型
            /// </summary>
            [Description("Face Recognition错误类型")]
            FaceRecognitionBase = 0x12000,

            /// <summary>
            /// 无效的输入内存
            /// </summary>
            [Description("无效的输入内存")]
            InvalidMemoryInfo = FaceRecognitionBase + 1,

            /// <summary>
            /// 无效的输入图像参数
            /// </summary>
            [Description("无效的输入图像参数")]
            InvalidImageInfo = FaceRecognitionBase + 2,

            /// <summary>
            /// 无效的脸部信息
            /// </summary>
            [Description("无效的脸部信息")]
            InvalidFaceInfo = FaceRecognitionBase + 3,

            /// <summary>
            /// 当前设备无GPU可用
            /// </summary>
            [Description("当前设备无GPU可用")]
            NoGpuAvailable = FaceRecognitionBase + 4,

            /// <summary>
            /// 待比较的两个人脸特征的版本不一致
            /// </summary>
            [Description("待比较的两个人脸特征的版本不一致")]
            MismatchedFeatureLevel = FaceRecognitionBase + 5
        }
        /// <summary>
        /// 脸部角度的检测范围
        /// </summary>
        public enum EOrientPriority
        {
            /// <summary>
            /// 检测 0 度（±45 度）方向，相当于鼻孔朝下
            /// </summary>
            Only0 = 0x1,
            /// <summary>
            /// 检测 90 度（±45 度）方向，估计是鼻孔朝东
            /// </summary>
            Only90 = 0x2,
            /// <summary>
            /// 检测 270 度（±45 度）方向，鼻孔朝西
            /// </summary>
            Only270 = 0x3,
            /// <summary>
            /// 检测 180 度（±45 度）方向，鼻孔朝上
            /// </summary>
            Only180 = 0x4,
            /// <summary>
            /// 检测 0， 90， 180， 270 四个方向,0 度更优先
            /// </summary>
            Ext0 = 0x5
        }
    }
}
