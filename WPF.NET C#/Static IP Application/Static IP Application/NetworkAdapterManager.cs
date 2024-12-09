using System;
using System.Collections.Generic;
using System.Management;

namespace PromptNow.NetworkAdapter
{
    public class NetworkAdapterManager
    {
        private static NetworkAdapterManager instance = new NetworkAdapterManager();

        private NetworkAdapterManager() { }

        // ฟังก์ชันสำหรับเรียก Singleton Instance
        public static NetworkAdapterManager GetInstance()
        {
            return instance;
        }

        // ฟังก์ชันเพื่อโหลด Network Adapters
        public bool GetNetworkAdapters(out List<string> adapterNames, out string error)
        {
            error = null;
            bool result = false;

            adapterNames = new List<string>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string adapterName = queryObj["Name"]?.ToString();
                    if (!string.IsNullOrEmpty(adapterName))
                    {
                        adapterNames.Add(adapterName);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }

        // ฟังก์ชันเพื่อโหลดข้อมูลของ Adapter ที่เลือก
        public bool GetAdapterProfile(string adapterName, out string ipAddress, out string subnetMask, out string gateway, out string preferredDNS, out string alternateDNS, out string error)
        {
            error = null;
            ipAddress = subnetMask = gateway = preferredDNS = alternateDNS = "";
            bool result = false;

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description = '{adapterName}' AND IPEnabled = true");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    // ดึงข้อมูล IP Address, Subnet Mask, Gateway และ DNS
                    string[] ipAddresses = (string[])queryObj["IPAddress"];
                    string[] subnetMasks = (string[])queryObj["IPSubnet"];
                    string[] gateways = (string[])queryObj["DefaultIPGateway"];
                    string[] dnsServers = (string[])queryObj["DNSServerSearchOrder"];

                    ipAddress = (ipAddresses != null && ipAddresses.Length > 0) ? ipAddresses[0] : "";
                    subnetMask = (subnetMasks != null && subnetMasks.Length > 0) ? subnetMasks[0] : "";
                    gateway = (gateways != null && gateways.Length > 0) ? gateways[0] : "";
                    preferredDNS = (dnsServers != null && dnsServers.Length > 0) ? dnsServers[0] : "";
                    alternateDNS = (dnsServers != null && dnsServers.Length > 1) ? dnsServers[1] : "";

                    result = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }

        // ฟังก์ชันสำหรับค้นหา Adapter
        public bool GetAdapterByName(string adapterName, out ManagementObjectSearcher adapter, out string error)
        {
            error = null;
            bool result = false;
            adapter = null;

            try
            {
                adapter = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Description = '{adapterName}' AND IPEnabled = true");

                result = adapter != null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }

        // ฟังก์ชันการตั้งค่า Static IP
        public bool SetIPAddress(string adapterName, string ipAddress, string subnetMask, out string error)
        {
            error = null;
            bool result = false;

            try
            {
                if (GetAdapterByName(adapterName, out var adapter, out error))
                {
                    foreach (ManagementObject queryObj in adapter.Get())
                    {
                        ManagementBaseObject newIP = queryObj.GetMethodParameters("EnableStatic");
                        newIP["IPAddress"] = new string[] { ipAddress };
                        newIP["SubnetMask"] = new string[] { subnetMask };
                        queryObj.InvokeMethod("EnableStatic", newIP, null);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }

        // ฟังก์ชันการตั้งค่า Gateway
        public bool SetGateway(string adapterName, string gateway, out string error)
        {
            error = null;
            bool result = false;

            try
            {
                if (GetAdapterByName(adapterName, out var adapter, out error))
                {
                    foreach (ManagementObject queryObj in adapter.Get())
                    {
                        ManagementBaseObject newGateway = queryObj.GetMethodParameters("SetGateways");
                        newGateway["DefaultIPGateway"] = new string[] { gateway };
                        queryObj.InvokeMethod("SetGateways", newGateway, null);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }

        // ฟังก์ชันการตั้งค่า DNS
        public bool SetDNS(string adapterName, string preferredDNS, string alternateDNS, out string error)
        {
            error = null;
            bool result = false;

            try
            {
                if (GetAdapterByName(adapterName, out var adapter, out error))
                {
                    foreach (ManagementObject queryObj in adapter.Get())
                    {
                        ManagementBaseObject newDNS = queryObj.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = new string[] { preferredDNS, alternateDNS };
                        queryObj.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }

        // ฟังก์ชันเปิดใช้งาน DHCP
        public bool SetDHCP(string adapterName, out string error)
        {
            error = null;
            bool result = false;

            try
            {
                if (GetAdapterByName(adapterName, out var adapter, out error))
                {
                    foreach (ManagementObject queryObj in adapter.Get())
                    {
                        queryObj.InvokeMethod("EnableDHCP", null);

                        ManagementBaseObject newDNS = queryObj.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = null; // ใช้ DNS อัตโนมัติ
                        queryObj.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return result;
        }
    }
}
