using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExtendCSharp.Interfaces;

namespace ExtendCSharp.Services
{
    public class VolumeService : IService
    {
        [ComImport]
        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDeviceEnumerator
        {
            void _VtblGap1_1();
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice ppDevice);
        }
        private static class MMDeviceEnumeratorFactory
        {
            public static IMMDeviceEnumerator CreateInstance()
            {
                return (IMMDeviceEnumerator)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"))); // a MMDeviceEnumerator
            }
        }
        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDevice
        {
            int Activate([MarshalAs(UnmanagedType.LPStruct)] Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        }
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioEndpointVolume
        {
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE RegisterControlChangeNotify(/* [in] */__in IAudioEndpointVolumeCallback *pNotify) = 0;
            int RegisterControlChangeNotify(IntPtr pNotify);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE UnregisterControlChangeNotify(/* [in] */ __in IAudioEndpointVolumeCallback *pNotify) = 0;
            int UnregisterControlChangeNotify(IntPtr pNotify);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetChannelCount(/* [out] */ __out UINT *pnChannelCount) = 0;
            int GetChannelCount(ref uint pnChannelCount);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetMasterVolumeLevel( /* [in] */ __in float fLevelDB,/* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int SetMasterVolumeLevel(float fLevelDB, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetMasterVolumeLevelScalar( /* [in] */ __in float fLevel,/* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMasterVolumeLevel(/* [out] */ __out float *pfLevelDB) = 0;
            int GetMasterVolumeLevel(ref float pfLevelDB);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMasterVolumeLevelScalar( /* [out] */ __out float *pfLevel) = 0;
            int GetMasterVolumeLevelScalar(ref float pfLevel);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetChannelVolumeLevel(/* [in] */__in UINT nChannel,float fLevelDB,/* [unique][in] */ LPCGUID pguidEventContext) = 0;   
            int SetChannelVolumeLevel(uint nChannel, float fLevelDB, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetChannelVolumeLevelScalar(/* [in] */ __in UINT nChannel,float fLevel,/* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int SetChannelVolumeLevelScalar(uint nChannel, float fLevel, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetChannelVolumeLevel(/* [in] */ __in UINT nChannel,/* [out] */__out float *pfLevelDB) = 0;
            int GetChannelVolumeLevel(uint nChannel, ref float pfLevelDB);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetChannelVolumeLevelScalar(/* [in] */__in UINT nChannel,/* [out] */__out float *pfLevel) = 0;
            int GetChannelVolumeLevelScalar(uint nChannel, ref float pfLevel);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetMute( /* [in] *__in BOOL bMute, /* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int SetMute([MarshalAs(UnmanagedType.Bool)] Boolean bMute, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMute( /* [out] */ __out BOOL *pbMute) = 0;
            int GetMute(ref bool pbMute);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVolumeStepInfo( /* [out] */ __out UINT *pnStep,/* [out] */__out UINT *pnStepCount) = 0;
            int GetVolumeStepInfo(ref uint pnStep, ref uint pnStepCount);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE VolumeStepUp( /* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int VolumeStepUp(Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE VolumeStepDown(/* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int VolumeStepDown(Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE QueryHardwareSupport(/* [out] */ __out DWORD *pdwHardwareSupportMask) = 0;
            int QueryHardwareSupport(ref uint pdwHardwareSupportMask);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVolumeRange( /* [out] */ __out float *pflVolumeMindB,/* [out] */ __out float *pflVolumeMaxdB,/* [out] */ __out float *pflVolumeIncrementdB) = 0;
            int GetVolumeRange(ref float pflVolumeMindB, ref float pflVolumeMaxdB, ref float pflVolumeIncrementdB);
        }



        private IAudioEndpointVolume GetCurrentDevice()
        {
            try
            {
                IMMDeviceEnumerator deviceEnumerator = MMDeviceEnumeratorFactory.CreateInstance();
                IMMDevice speakers;
                const int eRender = 0;
                const int eMultimedia = 1;
                deviceEnumerator.GetDefaultAudioEndpoint(eRender, eMultimedia, out speakers);

                object aepv_obj;
                speakers.Activate(typeof(IAudioEndpointVolume).GUID, 0, IntPtr.Zero, out aepv_obj);
                IAudioEndpointVolume aepv = (IAudioEndpointVolume)aepv_obj;
                return aepv;
            }
            catch (Exception ex) { }
            return null;

        }
        private int FixVolume(int Volume)
        {
            if (Volume < 0)
                Volume = 0;
            else if (Volume > 100)
                Volume = 100;
            return Volume;
        }
        private float FixVolume(float Volume)     //il volume in float lo considero già /100
        {
            if (Volume < 0)
                Volume = 0;
            else if (Volume > 1)
                Volume = 1;
            return Volume;
        }

        public void Set(int Volume)
        {
            int Level = FixVolume(Volume);



            try
            {
                /*IMMDeviceEnumerator deviceEnumerator = MMDeviceEnumeratorFactory.CreateInstance();
                IMMDevice speakers;
                const int eRender = 0;
                const int eMultimedia = 1;
                deviceEnumerator.GetDefaultAudioEndpoint(eRender, eMultimedia, out speakers);

                object aepv_obj;
                speakers.Activate(typeof(IAudioEndpointVolume).GUID, 0, IntPtr.Zero, out aepv_obj);
                IAudioEndpointVolume aepv = (IAudioEndpointVolume)aepv_obj;*/

                IAudioEndpointVolume aepv = GetCurrentDevice();
                Guid ZeroGuid = new Guid();
                int res = aepv.SetMasterVolumeLevelScalar(Level / 100f, ZeroGuid);

            }
            catch (Exception ex) { }
        }
        public void Up(int delta=1)
        {
            IAudioEndpointVolume device = GetCurrentDevice();
            Guid ZeroGuid = new Guid();

            uint StepCurrent = 0, StepCount = 0;
            device.GetVolumeStepInfo(ref StepCurrent, ref StepCount);
            uint StepV = 100/(StepCount - 1);
            if (StepCurrent== delta)
            {
                
                int a = device.VolumeStepUp(ZeroGuid);
            }
            else
            {
                float CurrentLevel = 0;
                int res = device.GetMasterVolumeLevelScalar(ref CurrentLevel);
                CurrentLevel += (delta / 100.0f);
                CurrentLevel = FixVolume(CurrentLevel);
                res = device.SetMasterVolumeLevelScalar(CurrentLevel, ZeroGuid);
            }
           
        }
        public void Down(int delta =1)
        {
            IAudioEndpointVolume device = GetCurrentDevice();
            Guid ZeroGuid = new Guid();

            uint StepCurrent = 0, StepCount = 0;
            device.GetVolumeStepInfo(ref StepCurrent, ref StepCount);
            uint StepV = 100 / (StepCount - 1);
            if (StepCurrent == delta)
            {

                int a = device.VolumeStepDown(ZeroGuid);
            }
            else
            {
                float CurrentLevel = 0;
                int res = device.GetMasterVolumeLevelScalar(ref CurrentLevel);
                CurrentLevel -= (delta / 100.0f);
                CurrentLevel = FixVolume(CurrentLevel);
                res = device.SetMasterVolumeLevelScalar(CurrentLevel, ZeroGuid);
            }


        }
        public bool GetMute()
        {
            IAudioEndpointVolume device = GetCurrentDevice();
            bool b = false;
            device.GetMute(ref b);
            return b;
        }
        public void SetMute(bool Mute)
        {
            IAudioEndpointVolume device = GetCurrentDevice();
            Guid ZeroGuid = new Guid();
            device.SetMute(Mute, ZeroGuid);
        }
        public void SwitchMute()
        {
            IAudioEndpointVolume device = GetCurrentDevice();
            bool b = false;
            device.GetMute(ref b);
            Guid ZeroGuid = new Guid();
            device.SetMute(!b, ZeroGuid);
        }


        public int GetCurrent()
        {
            try
            {
                IAudioEndpointVolume device = GetCurrentDevice();

                float o = 0;
                int res = device.GetMasterVolumeLevelScalar(ref o);
                return (int)(o*100.0);

            }
            catch (Exception ex) { }
            return -1;
        }

        
    }
}
