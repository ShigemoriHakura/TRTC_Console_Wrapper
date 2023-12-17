using System;
using ManageLiteAV;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Shiro_Trtc_Wrapper_x64
{
    class Program
    {
        public ITRTCCloud mTRTCCloud;

        static void Main(string[] args)
        {
            var mTRTCCloud = ITRTCCloud.getTRTCShareInstance();
            mTRTCCloud.setLogLevel(TRTCLogLevel.TRTCLogLevelNone);
            //Console.WriteLine("============ Starting Echo ============ ");

            var manager = mTRTCCloud.getDeviceManager();

            bool addEmpty = false;
            if (args.Length > 0)
            {
                if (args[0].ToString() == "E" || args[0].ToString() == "-E")
                {
                    addEmpty = true;
                }
            }

            var returnedJson = new TXJson
            {
                Cameras = GetAllDevicesFromCollection(manager.getDevicesList(TXMediaDeviceType.TXMediaDeviceTypeCamera), addEmpty),
                Mics = GetAllDevicesFromCollection(manager.getDevicesList(TXMediaDeviceType.TXMediaDeviceTypeMic), addEmpty),
                Speakers = GetAllDevicesFromCollection(manager.getDevicesList(TXMediaDeviceType.TXMediaDeviceTypeSpeaker), addEmpty)
            };
            Console.WriteLine(JsonConvert.SerializeObject(returnedJson));
        }

        public static List<TXDevice> GetAllDevicesFromCollection(ITRTCDeviceCollection devices, bool addEmpty)
        {
            var ret = new List<TXDevice>();
            for (uint i = 0; i < devices.getCount(); i++)
            {
                ret.Add(GetDevice(devices, i));
            }
            if (addEmpty)
            {
                ret.Add(GetNullDevice());
            }
            return ret;
        }

        public static TXDevice GetDevice(ITRTCDeviceCollection devices, uint i)
        {
            var device = new TXDevice
            {
                DeviceName = devices.getDeviceName(i),
                DevicePID = devices.getDevicePID(i)
            };
            return device;
        }


        public static TXDevice GetNullDevice()
        {
            var device = new TXDevice
            {
                DeviceName = "/",
                DevicePID = "NoneQwQ_Empty"
            };
            return device;
        }

        public struct TXDevice
        {
            public String DevicePID;
            public String DeviceName;
        }

        public struct TXJson
        {
            public List<TXDevice> Cameras;
            public List<TXDevice> Mics;
            public List<TXDevice> Speakers;
        }
    }
}
