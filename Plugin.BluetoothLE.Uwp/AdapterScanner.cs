﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;


namespace Plugin.BluetoothLE
{
    public class AdapterScanner : IAdapterScanner
    {
        public IObservable<IAdapter> FindAdapters() => Observable.Create<IAdapter>(async ob =>
        {
            var devices = await DeviceInformation.FindAllAsync(BluetoothAdapter.GetDeviceSelector());
            foreach (var dev in devices)
            {
                var native = await BluetoothAdapter.FromIdAsync(dev.Id);
                var radio = await native.GetRadioAsync();
                var adapter = new Adapter(native, radio);
                ob.OnNext(adapter);
            }
            ob.OnCompleted();
            return Disposable.Empty;
        });
    }
}