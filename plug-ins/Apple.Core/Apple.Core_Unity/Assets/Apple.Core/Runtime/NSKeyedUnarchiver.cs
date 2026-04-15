using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Apple.Core.Runtime
{
    /// <summary>
    /// C# wrapper around NSKeyedUnarchiver.
    /// </summary>
    public class NSKeyedUnarchiver : NSObject
    {
        private bool _hasFinishedDecoding;
        
        public NSKeyedUnarchiver(NSData fromData) : base(Interop.NSKeyedUnarchiver_Init(fromData.Pointer, NSException.ThrowOnExceptionCallback))
        {
        }

        public static T UnarchiveTopLevelObject<T>(NSData fromData) where T : NSObject
        {
            InteropBoxer.Boxer boxer = InteropBoxer.LookupBoxer<T>();
            if(boxer == null)
            {
                throw new NotSupportedException($"NSKeyedUnarchiver does not support {typeof(T).FullName}");
            }

            IntPtr unarchivedObjectPtr = Interop.NSKeyedUnarchiver_UnarchiveTopLevelObject(fromData.Pointer, NSException.ThrowOnExceptionCallback);
            if(unarchivedObjectPtr == IntPtr.Zero)
            {
                return null;
            }

            if(boxer.TryUnbox(PointerCast<NSObject>(unarchivedObjectPtr), out var objValue))
            {
                return objValue as T;
            }
            else
            {
                return null;
            }
        }

        protected override void OnDispose(bool isDisposing)
        {
            if(!_hasFinishedDecoding)
            {
                FinishDecoding();
            }

            base.OnDispose(isDisposing);
        }

        public void FinishDecoding()
        {
            Interop.NSKeyedUnarchiver_FinishDecoding(Pointer);
            _hasFinishedDecoding = true;
        }

        private static class Interop
        {
            [DllImport(InteropUtility.DLLName)] public static extern IntPtr NSKeyedUnarchiver_Init(IntPtr fromData, NSExceptionCallback onException);
            [DllImport(InteropUtility.DLLName)] public static extern IntPtr NSKeyedUnarchiver_UnarchiveTopLevelObject(IntPtr nsDataPtr, NSExceptionCallback onException);
            [DllImport(InteropUtility.DLLName)] public static extern void NSKeyedUnarchiver_FinishDecoding(IntPtr nsKeyedArchivePtr);
        }
    }
}
