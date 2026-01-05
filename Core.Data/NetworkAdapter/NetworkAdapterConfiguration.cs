using System.Collections.Generic;
using System.Management;

namespace com.fabioscagliola.Core.Data.NetworkAdapter
{
    public class NetworkAdapterConfiguration
    {
        public string Caption { get; set; }
        public List<string> IPAddressList { get; set; }
        public List<string> IPSubnetList { get; set; }
        public List<string> DefaultIPGatewayList { get; set; }
        public List<string> DNSServerSearchOrderList { get; set; }

        public NetworkAdapterConfiguration()
        {
            IPAddressList = new List<string>();
            IPSubnetList = new List<string>();
            DefaultIPGatewayList = new List<string>();
            DNSServerSearchOrderList = new List<string>();
        }

        public string IPAddress
        {
            get
            {
                string value = null;

                if (IPAddressList.Count != 0)
                {
                    value = IPAddressList[0];
                }

                return value;
            }
            set
            {
                IPAddressList = new List<string>();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    IPAddressList.Add(value);
                }
            }
        }

        public string IPSubnet
        {
            get
            {
                string value = null;

                if (IPSubnetList.Count != 0)
                {
                    value = IPSubnetList[0];
                }

                return value;
            }
            set
            {
                IPSubnetList = new List<string>();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    IPSubnetList.Add(value);
                }
            }
        }

        public string DefaultIPGateway
        {
            get
            {
                string value = null;

                if (DefaultIPGatewayList.Count != 0)
                {
                    value = DefaultIPGatewayList[0];
                }

                return value;
            }
            set
            {
                DefaultIPGatewayList = new List<string>();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    DefaultIPGatewayList.Add(value);
                }
            }
        }

        public string DNSServerSearchOrder
        {
            get
            {
                string value = null;

                if (DNSServerSearchOrderList.Count != 0)
                {
                    value = DNSServerSearchOrderList[0];
                }

                return value;
            }
            set
            {
                DNSServerSearchOrderList = new List<string>();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    DNSServerSearchOrderList.Add(value);
                }
            }
        }

        public static List<NetworkAdapterConfiguration> GetNetworkAdapterConfigurationList()
        {
            List<NetworkAdapterConfiguration> networkAdapterConfigurationList = new List<NetworkAdapterConfiguration>();

            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                if ((bool)managementObject["IPEnabled"])
                {
                    NetworkAdapterConfiguration networkAdapterConfiguration = new NetworkAdapterConfiguration();

                    if (managementObject["Caption"] != null)
                        networkAdapterConfiguration.Caption = (string)managementObject["Caption"];
                    if (managementObject["IPAddress"] != null)
                        networkAdapterConfiguration.IPAddressList.AddRange((string[])managementObject["IPAddress"]);
                    if (managementObject["IPSubnet"] != null)
                        networkAdapterConfiguration.IPSubnetList.AddRange((string[])managementObject["IPSubnet"]);
                    if (managementObject["DefaultIPGateway"] != null)
                        networkAdapterConfiguration.DefaultIPGatewayList.AddRange((string[])managementObject["DefaultIPGateway"]);
                    if (managementObject["DNSServerSearchOrder"] != null)
                        networkAdapterConfiguration.DNSServerSearchOrderList.AddRange((string[])managementObject["DNSServerSearchOrder"]);

                    networkAdapterConfigurationList.Add(networkAdapterConfiguration);
                }
            }

            return networkAdapterConfigurationList;
        }

        public void Update()
        {
            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                if ((bool)managementObject["IPEnabled"] && (string)managementObject["Caption"] == Caption)
                {
                    ManagementBaseObject enableStatic = managementObject.GetMethodParameters("EnableStatic");
                    ManagementBaseObject setGateways = managementObject.GetMethodParameters("SetGateways");
                    ManagementBaseObject setDNSServerSearchOrder = managementObject.GetMethodParameters("SetDNSServerSearchOrder");

                    enableStatic["IPAddress"] = new string[] { IPAddress };
                    enableStatic["SubnetMask"] = new string[] { IPSubnet };

                    setGateways["DefaultIPGateway"] = new string[] { DefaultIPGateway };
                    setGateways["GatewayCostMetric"] = new int[] { 1 };

                    setDNSServerSearchOrder["DNSServerSearchOrder"] = new string[] { DNSServerSearchOrder };

                    managementObject.InvokeMethod("EnableStatic", enableStatic, null);
                    managementObject.InvokeMethod("SetGateways", setGateways, null);
                    managementObject.InvokeMethod("SetDNSServerSearchOrder", setDNSServerSearchOrder, null);

                    break;
                }
            }
        }

    }
}

