//
//  NSKeyedUnarchiver.swift
//  AppleCoreNative
//

import Foundation

@_cdecl("NSKeyedUnarchiver_Init")
public func NSKeyedUnarchiver_Init
(
    nsDataPtr: UnsafeMutableRawPointer,
    onError: @escaping NSErrorCallback
) -> UnsafeMutableRawPointer?
{
    let nsData = Unmanaged<NSData>.fromOpaque(nsDataPtr).takeUnretainedValue();
    do
    {
        let unarchiver = try NSKeyedUnarchiver.init(forReadingFrom: nsData as Data);
        return Unmanaged<NSKeyedUnarchiver>.passRetained(unarchiver).toOpaque();
    }
    catch
    {
        onError(Unmanaged<NSError>.passRetained(error as NSError).toOpaque());
        return nil;
    }
}

@_cdecl("NSKeyedUnarchiver_UnarchiveTopLevelObject")
public func NSKeyedUnarchiver_UnarchiveTopLevelObject
(
    nsDataPtr: UnsafeMutableRawPointer,
    onError: @escaping NSErrorCallback
) -> UnsafeMutableRawPointer?
{
    let nsData = Unmanaged<NSData>.fromOpaque(nsDataPtr).takeUnretainedValue();
    do
    {
        // TODO Replace this with unarchivedObject(ofClass:from:), which will require passing class names through.
        let unarchivedObject = try NSKeyedUnarchiver.unarchiveTopLevelObjectWithData(nsData as Data);
        if(unarchivedObject == nil)
        {
            return nil;
        }

        let obj = unarchivedObject as? NSObject;
        if(obj == nil)
        {
            return nil;
        }

        return Unmanaged<NSObject>.passRetained(obj!).toOpaque();
    }
    catch
    {
        onError(Unmanaged<NSError>.passRetained(error as NSError).toOpaque());
        return nil;
    }
}

@_cdecl("NSKeyedUnarchiver_FinishDecoding")
public func NSKeyedUnarchiver_FinishDecoding
(
    pointer: UnsafeMutableRawPointer
)
{
    let archiver = Unmanaged<NSKeyedUnarchiver>.fromOpaque(pointer).takeUnretainedValue();
    archiver.finishDecoding();
}
