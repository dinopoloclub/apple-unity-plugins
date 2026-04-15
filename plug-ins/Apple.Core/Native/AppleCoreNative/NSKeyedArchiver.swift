//
//  NSKeyedArchiver.swift
//  AppleCoreNative
//

import Foundation

@_cdecl("NSKeyedArchiver_Init")
public func NSKeyedArchiver_Init
(
    requireSecureCoding: Bool
) -> UnsafeMutableRawPointer
{
    let archiver = NSKeyedArchiver.init(requiringSecureCoding: requireSecureCoding);
    return Unmanaged.passRetained(archiver).toOpaque();
}

@_cdecl("NSKeyedArchiver_GetEncodedData")
public func NSKeyedArchiver_GetEncodedData
(
    pointer: UnsafeMutableRawPointer
) -> UnsafeMutableRawPointer
{
    let archiver = Unmanaged<NSKeyedArchiver>.fromOpaque(pointer).takeUnretainedValue();
    return Unmanaged.passRetained(archiver.encodedData as NSData).toOpaque();
}

@_cdecl("NSKeyedArchiver_FinishEncoding")
public func NSKeyedArchiver_FinishEncoding
(
    pointer: UnsafeMutableRawPointer
)
{
    let archiver = Unmanaged<NSKeyedArchiver>.fromOpaque(pointer).takeUnretainedValue();
    archiver.finishEncoding();
}

@_cdecl("NSKeyedArchiver_ArchivedData")
public func NSKeyedArchiver_ArchivedData
(
    rootObjectPtr: UnsafeMutableRawPointer,
    requireSecureCoding : Bool,
    onError : @escaping NSErrorCallback
) -> UnsafeMutableRawPointer?
{
    let rootObject = Unmanaged<NSObject>.fromOpaque(rootObjectPtr).takeUnretainedValue();
    
    do
    {
        let archivedData = try NSKeyedArchiver.archivedData(withRootObject: rootObject, requiringSecureCoding: requireSecureCoding);
        return Unmanaged<NSData>.passRetained(archivedData as NSData).toOpaque();
    }
    catch
    {
        onError(Unmanaged<NSError>.passRetained(error as NSError).toOpaque());
        return nil;
    }
}
