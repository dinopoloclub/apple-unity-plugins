using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Apple.Core.Runtime
{
    /// <summary>
    /// C# wrapper around NSKeyedArchiver.
    /// </summary>
    public class NSKeyedArchiver : NSObject
    {
        public NSKeyedArchiver(bool requireSecureCoding = true) : base(Interop.NSKeyedArchiver_Init(requireSecureCoding))
        {
        }

        public static NSData ArchivedData(InteropReference rootObject, bool requireSecureCoding = true)
        {
            IntPtr encodedDataPtr = Interop.NSKeyedArchiver_ArchivedData(rootObject.Pointer, requireSecureCoding, NSException.ThrowOnExceptionCallback);
            return encodedDataPtr == IntPtr.Zero ? null : new NSData(encodedDataPtr);
        }

        public void FinishEncoding()
        {
            Interop.NSKeyedArchiver_FinishEncoding(Pointer);
        }

        public NSData EncodedData
        {
            get
            {
                IntPtr encodedDataPointer = Interop.NSKeyedArchiver_GetEncodedData(Pointer, NSException.ThrowOnExceptionCallback);
                return encodedDataPointer != null ? new NSData(encodedDataPointer) : null;
            }
        }

        private static class Interop
        {
            [DllImport(InteropUtility.DLLName)] public static extern IntPtr NSKeyedArchiver_Init(bool requireSecureCoding);
            [DllImport(InteropUtility.DLLName)] public static extern IntPtr NSKeyedArchiver_ArchivedData(IntPtr rootObjectPtr, bool requireSecureCoding, NSExceptionCallback onException);
            [DllImport(InteropUtility.DLLName)] public static extern void NSKeyedArchiver_FinishEncoding(IntPtr nsKeyedArchivePtr);
            [DllImport(InteropUtility.DLLName)] public static extern IntPtr NSKeyedArchiver_GetEncodedData(IntPtr nsKeyedArchivePtr, NSExceptionCallback onException);
        }
    }
}
