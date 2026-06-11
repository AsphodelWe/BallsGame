using System.Runtime.InteropServices;
using UnityEngine;

public static class DeviceDetector
{
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool IsRealMobileDevice();
    #endif

    private static bool? _cachedIsMobile;
    
    public static bool IsMobile()
    {
        if (_cachedIsMobile.HasValue) 
            return _cachedIsMobile.Value;
        
        #if UNITY_EDITOR
            _cachedIsMobile = UnityEditor.EditorPrefs.GetBool("DeviceDetector.MobileMode", false);
        #elif UNITY_WEBGL
            try
            {
                _cachedIsMobile = IsRealMobileDevice();
                Debug.Log($"[DeviceDetector] WebGL detection: {_cachedIsMobile}");
            }
            catch
            {
                Debug.LogWarning("[DeviceDetector] JS failed, fallback to DPI");
                _cachedIsMobile = Screen.dpi > 180;
            }
        #else
            _cachedIsMobile = Application.isMobilePlatform;
        #endif
        
        return _cachedIsMobile.Value;
    }
    
    public static void ResetCache()
    {
        _cachedIsMobile = null;
    }
    
    public static void SetMobileMode(bool isMobile)
    {
        _cachedIsMobile = isMobile;
        Debug.Log($"[DeviceDetector] Manual mode set to: {isMobile}");
    }
}
