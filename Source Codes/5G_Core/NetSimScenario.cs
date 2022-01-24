using System;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace _5G_Core
{
    /*  structure to store the attributes of APPLICATION node  */
    public struct APPLICATION
    {
        public int DESTINATION_ID;
        public int ID;
        public string NAME;
        public int SOURCE_ID;
        public string gtpu_upf_gnb;
        public string pfcp_upf_smf;
        public string gtpc_smf_amf;
        public string pfcp_smf_upf;
        public string ngap_amf_gnb;
        public string gtpc_amf_smf;
        public string ngap_gnb_amf;
        public string ngap_gnb_smf;
        public string gtpu_gnb_upf;
    }

    /*  structure to store the Postion of a DEVICE*/
    public struct POS_3D
    {
        public double X_OR_LON;
        public double Y_OR_LAT;
        public double Z;
    }

    /*structure for containing DEVICE INFO of a LINK*/
    public struct LINK_DEVICE
    {
        public int DEVICE_ID;
        public int INTERFACE_ID;
        public string NAME;
    }

    /*  structure for containing attributes of a link  along with DEVICE INFO of the link */
    public struct LINK
    {
        public int DEVICE_COUNT;
        public int LINK_ID;
        public string LINK_NAME;
        public string link_type;
        public LINK_DEVICE[] link_device;
    }

    /* structure which stores information regarding position from centre of the grid */
    public struct LEVEL
    {
        public double r;
        public double theta;
        public int level;
        public double low_angle;
        public double increment;

    }

    /*  structure to store the attributes of a DEVICE */
    public struct DEVICE
    {
        public string DEFAULT_DEVICE_NAME;
        public int DEVICE_ID;
        public string DEVICE_IMAGE;
        public string DEVICE_NAME;
        public string DEVICE_TYPE;
        public int INTERFACE_COUNT;
        public string TYPE;
        public string WIRESHARK_OPTION;
      
        
    }

    /*  structure to store the attributes of an Interface of a DEVICE  */
    public struct INTERFACE
    {
        public int ID;
        public string DEFAULT_GATEWAY;
        public string IP_ADDRESS;
        public string SUBNET_MASK;
        public string MAC_ADDRESS;
        public string CONNECTED_TO;
        public string INTERFACE_TYPE;
        public string IMEI_NUMBER;
        public string MOBILE_NUMBER;
        public string SWITCH_ID;
      
    }

    public struct INTERFACE_N6
    {
        public int ROUTER_ID;
        public int UPF_INTERFACE_ID;
        public string DEFAULT_GATEWAY;
        public string IP_ADDRESS;
        public string SUBNET_MASK;      
    }

    /*  structure to store attributes,position,level and interfaces of a DEVICE */
    public struct DEVICE_CONTAINER
    {
        public DEVICE device;
        public POS_3D pos_3d;
        public LEVEL level;
        public INTERFACE[] _interface;
        //modify
        public APPLICATION application;
    }

    


    class Program
    {
        public static XmlWriter nsWriter;
        public static AddNetworkElement addNet;
        static void Main(string[] args)
        {
            int max_smf = 1;
            int max_amf = 1;
            int max_upf = 1;
            int max_switches = 3;
            int max_router = 0;
            int max_node = 0;
            int max_gnb = 0;
            int max_ue = 0;
            int max_epc = 0;
            int epc_id = 0;
            int max_ap = 0;
            int max_other_switches = 0;
            int max_wireless_node = 0;
            int max_application = 0;
            int epc_ethernet_count = 0;
            int copy_epc_ethernet_count = 1; // all ethernet interface for default router
            int epc_wan_count = 0;
            string[] ip_upf_n3 = new string[1];
            string[] ip_upf_n4 = new string[1];
            string[] ip_upf_n6 = new string[1];
            string[] ip_smf_n11 = new string[1];
            string[] ip_smf_n4 = new string[1];
            string[] ip_amf_n11 = new string[1];
            string[] ip_amf_n1_n2 = new string[1];           
            string[] ip_gnb_n3 = new string[1];           
            string[] ip_gnb_n1_n2 = new string[1];           
            string input_file_location;
            string output_file_location;
            string config_helper_location;
            string version_number = null;

            //if (args.Length == 3)
            //{
            //    input_file_location = args[0];
            //    output_file_location = args[1];
            //    config_helper_location = args[2];
            //}
            //else
            //{
            //    Console.WriteLine("incorrect!! required 3 arguments\n");
            //    return;
            //}

            XmlDocument xmlDoc = new XmlDocument();
           // xmlDoc.Load(input_file_location + "\\Configuration.netsim");

            //xmlDoc.Load("C:/Users/mridula/Desktop/automation_development/Configuration.netsim");
            xmlDoc.Load("C:/Users/mridula/Desktop/5_devices/Configuration.netsim");
            //xmlDoc.Load("C:/Users/mridula/Desktop/without router/Configuration.netsim");
            //xmlDoc.Load("C:/Users/mridula/Desktop/New folder/Configuration.netsim");
           // xmlDoc.Load("C:/Users/mridula/Desktop/Case_10_S64/Configuration.netsim");

            XmlNode root = xmlDoc.SelectSingleNode("descendant::NETWORK_CONFIGURATION/DEVICE_CONFIGURATION");
            XmlNode nwConfig = xmlDoc.SelectSingleNode("descendant::NETWORK_CONFIGURATION/CONNECTION");

            var apps = xmlDoc.SelectSingleNode("descendant::NETWORK_CONFIGURATION/APPLICATION_CONFIGURATION");
            var ver_no = xmlDoc.SelectSingleNode("descendant::EXPERIMENT_INFORMATION/NUMBER");

            XmlNode experiment_information = xmlDoc.SelectSingleNode("descendant::EXPERIMENT_INFORMATION");
            XmlNode sa_node = xmlDoc.CreateElement("SAORNSA");
            XmlNode SA0rNSA = experiment_information.AppendChild(sa_node);
            XmlNode opt_node = xmlDoc.CreateElement("OPTION");
            XmlNode option = experiment_information.AppendChild(opt_node);

            SA0rNSA.InnerText = "SA";
            option.InnerText = "";

            if (apps.Attributes != null && apps.Attributes["COUNT"].Value != null)
                max_application = Convert.ToInt32(apps.Attributes["COUNT"].Value);

            if (ver_no.InnerText != null)
            {
                version_number = Convert.ToString("13.1.26");
                ver_no.InnerText = version_number;
            }

            /********************************************************************************************/
            //mac address of devices and other variables
            string mac = "123456789ABC"; int max_n6_interface = 0;
            int i, j, ipv4_5g_count = 0, ipv4_count = 0, link_count = 0, ipv4_c = 1;
            double epc_pos_x = 0;
            double epc_pos_y = 0;
            double epc_pos_z = 0;
            /********************************************************************************************/

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE"))
            {
                if (node.Attributes["DEFAULT_DEVICE_NAME"] != null && node.Attributes["INTERFACE_COUNT"] != null)
                {
                    var device_name = node.Attributes["DEFAULT_DEVICE_NAME"].Value;
                    var inter_count = node.Attributes["INTERFACE_COUNT"].Value;

                    if (device_name != null && inter_count != null)
                    {
                        switch (device_name)
                        {
                            case "Wired_Node":
                                max_node = max_node + 1;
                                break;

                            case "Router":
                                max_router = max_router + 1;
                                break;

                            case "gNB":
                                max_gnb = max_gnb + 1;
                                break;

                            case "UE":
                                max_ue = max_ue + 1;
                                break;

                            case "Access_Point":
                                max_ap = max_ap + 1;
                                break;

                            case "Wireless_Node":
                                max_wireless_node = max_wireless_node + 1;
                                break;

                            case "L2_Switch":
                                max_other_switches = max_other_switches + 1;
                                break;

                            case "EPC":
                                XmlNode epc_pos = node.SelectSingleNode("descendant::  POS_3D");
                                epc_pos_x = Convert.ToDouble(epc_pos.Attributes["X_OR_LON"].Value);
                                epc_pos_y = Convert.ToDouble(epc_pos.Attributes["Y_OR_LAT"].Value);
                                epc_pos_z = Convert.ToDouble(epc_pos.Attributes["Z"].Value);
                                max_epc = max_epc + 1;
                                epc_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            // can be switch, wired node or AP 
            foreach (XmlNode epc_ethernet_node in xmlDoc.SelectNodes("descendant:: NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='EPC']/INTERFACE[@INTERFACE_TYPE='ETHERNET']"))
            {
                if (epc_ethernet_node != null)
                {
                    //ip_epc_ethernet[epc_ethernet_count] = epc_ethernet_node.Attributes["IP_ADDRESS"].Value;
                    //ip_router_ethernet[epc_ethernet_count] = ip_epc_ethernet[epc_ethernet_count];
                    epc_ethernet_count++;
                }
            }

            // router interface
            foreach (XmlNode epc_wan_node in xmlDoc.SelectNodes("descendant:: NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='EPC']/INTERFACE[@INTERFACE_TYPE='WAN']"))
            {
                if (epc_wan_node != null)
                {
                    //ip_epc_wan[epc_wan_count] = epc_wan_node.Attributes["IP_ADDRESS"].Value;
                    //ip_router_wan[epc_wan_count] = ip_epc_wan[epc_wan_count];
                    epc_wan_count++;
                }
            }

            if (epc_ethernet_count == 0)
                max_n6_interface = epc_wan_count;
            else
                max_n6_interface = epc_wan_count + 1; // all ethernet interfaces will be connected using one router

            int max_smf_interface = 2;
            double smf_x0 = epc_pos_x, smf_y0 = epc_pos_y, smf_z0 = epc_pos_z;

            int max_upf_interface = max_n6_interface + 2;
            double upf_x0 = epc_pos_x + epc_pos_y, upf_y0 = epc_pos_y + epc_pos_y, upf_z0 = epc_pos_z;

            int max_amf_interface = 2;
            double amf_x0 = epc_pos_x - epc_pos_y, amf_y0 = epc_pos_y + epc_pos_y, amf_z0 = epc_pos_z;

            int max_switch_upf_interface = 1 + max_gnb;
            double switch_upf_x0 = epc_pos_x + epc_pos_y, switch_upf_y0 = epc_pos_y + 2 * epc_pos_y, switch_upf_z0 = epc_pos_z;

            int max_switch_amf_interface = 1 + max_gnb;
            double switch_amf_x0 = epc_pos_x - epc_pos_y, switch_amf_y0 = epc_pos_y + 2 * epc_pos_y, switch_amf_z0 = epc_pos_z;

            int max_switch_gnb_interface = max_gnb;
            double switch_gnb_x0 = epc_pos_x, switch_gnb_y0 = epc_pos_y + 2 * epc_pos_y, switch_gnb_z0 = epc_pos_z;

            double router_x0 = epc_pos_x - (1.5 * epc_pos_y), router_y0 = epc_pos_y + epc_pos_y, router_z0 = epc_pos_z;

            /********************************************************************************************/
            //intermediate parameters used in generating scenario
            int total_device = 0;
            int new_router = 0;

            if (epc_ethernet_count != 0)
            {
                new_router = 1;
                total_device = max_router + max_node + max_gnb + max_ue + max_smf + max_amf + max_upf + max_switches + max_wireless_node + max_ap + max_other_switches + new_router;
            }
            else
                total_device = max_router + max_node + max_gnb + max_ue + max_smf + max_amf + max_upf + max_switches + max_wireless_node + max_ap + max_other_switches;

            int max_link = max_router + max_node + max_gnb + max_ue + max_smf + max_amf + max_upf + max_switches + new_router + max_other_switches + 100;
            Random rnd = new Random();
            nsWriter = new XmlWriter();
            addNet = new AddNetworkElement();
            // XmlNode root = nsWriter.open_document();
            /********************************************************************************************/

            //application for containing detailes of applications
            APPLICATION[] application = new APPLICATION[max_application];

            //device_container to store varible for each device
            DEVICE_CONTAINER[] device_container = new DEVICE_CONTAINER[total_device];

            //link for storing details of all the links
            LINK[] link = new LINK[max_link];

            /**********************************************************************************************/
            //this block sets the attributes,position of first device of UPF, SMF, AMF and all switches
            device_container[0].pos_3d.X_OR_LON = upf_x0;
            device_container[0].pos_3d.Y_OR_LAT = upf_y0;
            device_container[0].pos_3d.Z = upf_z0;
            device_container[max_upf].pos_3d.X_OR_LON = smf_x0;
            device_container[max_upf].pos_3d.Y_OR_LAT = smf_y0;
            device_container[max_upf].pos_3d.Z = smf_z0;
            device_container[max_upf + max_smf].pos_3d.X_OR_LON = amf_x0;
            device_container[max_upf + max_smf].pos_3d.Y_OR_LAT = amf_y0;
            device_container[max_upf + max_smf].pos_3d.Z = amf_z0;
            device_container[max_upf + max_smf + max_amf].pos_3d.X_OR_LON = switch_upf_x0;
            device_container[max_upf + max_smf + max_amf].pos_3d.Y_OR_LAT = switch_upf_y0;
            device_container[max_upf + max_smf + max_amf].pos_3d.Z = switch_upf_z0;
            device_container[max_upf + max_smf + max_amf + 1].pos_3d.X_OR_LON = switch_amf_x0;
            device_container[max_upf + max_smf + max_amf + 1].pos_3d.Y_OR_LAT = switch_amf_y0;
            device_container[max_upf + max_smf + max_amf + 1].pos_3d.Z = switch_amf_z0;
            device_container[max_upf + max_smf + max_amf + 2].pos_3d.X_OR_LON = switch_gnb_x0;
            device_container[max_upf + max_smf + max_amf + 2].pos_3d.Y_OR_LAT = switch_gnb_y0;
            device_container[max_upf + max_smf + max_amf + 2].pos_3d.Z = switch_gnb_z0;
            device_container[max_upf + max_smf + max_amf + max_switches + max_node].pos_3d.X_OR_LON = router_x0;
            device_container[max_upf + max_smf + max_amf + max_switches + max_node].pos_3d.Y_OR_LAT = router_y0;
            device_container[max_upf + max_smf + max_amf + max_switches + max_node].pos_3d.Z = router_z0;

            /**********************************************************************************************/
            //Adding 5G Core devices

            int router_epc_count = 0;
            int n6_no = 3;

            device_container[0].device.INTERFACE_COUNT = max_upf_interface;
            device_container[0]._interface = new INTERFACE[max_upf_interface];

            //Adding link information to Connection element
            foreach (XmlNode linknode in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION/CONNECTION/LINK"))
            {
                XmlNode node = linknode.SelectSingleNode("descendant::DEVICE[@DEVICE_ID = " + epc_id + "]");
                if (node != null)
                {
                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["INTERFACE_ID"] != null)
                    {
                        int connected_device_id = 0;
                        int connected_interface_id = 0;
                        var device_id = node.Attributes["DEVICE_ID"].Value;
                        int device_interface_id = 0;

                        device_interface_id = Convert.ToInt32(node.Attributes["INTERFACE_ID"].Value);

                        XmlNode previousNode = node.PreviousSibling;
                        XmlNode nextNode = node.NextSibling;

                        if (device_id != null)
                        {
                            // to find sibling nodes of device element in link
                            if (nextNode.Attributes["DEVICE_ID"] != null && nextNode.Attributes["INTERFACE_ID"] != null)
                            {
                                connected_device_id = Convert.ToInt32(nextNode.Attributes["DEVICE_ID"].Value);
                                connected_interface_id = Convert.ToInt32(nextNode.Attributes["INTERFACE_ID"].Value);
                            }

                            else if (previousNode.Attributes["DEVICE_ID"] != null && previousNode.Attributes["INTERFACE_ID"] != null)
                            {
                                connected_device_id = Convert.ToInt32(previousNode.Attributes["DEVICE_ID"].Value);
                                connected_interface_id = Convert.ToInt32(previousNode.Attributes["INTERFACE_ID"].Value);
                            }

                            XmlNode device_node = xmlDoc.SelectSingleNode("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEVICE_ID = " + connected_device_id + "]");
                            XmlNode epc_node = xmlDoc.SelectSingleNode("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEVICE_ID = " + epc_id + "]/INTERFACE[@ID = " + device_interface_id + "]");


                            if (device_node.Attributes["DEFAULT_DEVICE_NAME"] != null && device_node.Attributes["INTERFACE_COUNT"] != null && device_node.Attributes["DEVICE_ID"] != null)
                            {
                                var device_name = device_node.Attributes["DEFAULT_DEVICE_NAME"].Value;
                                var inter_count = device_node.Attributes["INTERFACE_COUNT"].Value;

                                if (device_name != null && inter_count != null)
                                {
                                    switch (device_name)
                                    {
                                        case "Wired_Node":
                                            XmlNode wired_ethernet_node = device_node.SelectSingleNode("descendant:: INTERFACE[@ID = " + connected_interface_id + "]/ LAYER[@TYPE = 'NETWORK_LAYER']/NETWORK_PROTOCOL/ PROTOCOL_PROPERTY");

                                            if (copy_epc_ethernet_count < epc_ethernet_count)
                                            {
                                                if (wired_ethernet_node.Attributes["IP_ADDRESS"] != null)
                                                {
                                                    //wired_ethernet_node.Attributes["IP_ADDRESS"].Value = addNet.ip_same_network(ip_epc_ethernet[copy_epc_ethernet_count]);
                                                    node.Attributes["INTERFACE_ID"].Value = Convert.ToString(copy_epc_ethernet_count + 1);
                                                    epc_node.Attributes["ID"].Value = Convert.ToString(copy_epc_ethernet_count + 1);
                                                    copy_epc_ethernet_count++;
                                                }
                                            }

                                            break;

                                        case "Router":
                                            int router_id = 0;
                                            int router_interface_id = 0;
                                            router_id = connected_device_id;
                                            router_interface_id = connected_interface_id;
                                            XmlNode router_wan_node = device_node.SelectSingleNode("descendant::INTERFACE[@ID = " + router_interface_id + "]");
                                            if (router_wan_node.Attributes["INTERFACE_TYPE"] != null)
                                            {
                                                if (router_wan_node.Attributes["INTERFACE_TYPE"].Value == "WAN")
                                                {
                                                    if (router_id > epc_id)
                                                        router_id = router_id - 1;

                                                    if (inter_count != null)
                                                    {
                                                        i = max_upf + max_smf + max_amf + max_switches + max_node + router_epc_count;
                                                        if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router))
                                                        {
                                                            int jj = 0;

                                                            if (n6_no <= max_upf_interface)
                                                            {
                                                                device_container[i].device.DEVICE_ID = router_id + 6;
                                                                device_container[i].device.INTERFACE_COUNT = 1;
                                                                device_container[i].device.DEVICE_TYPE = "ROUTER";

                                                                device_container[i]._interface = new INTERFACE[1];

                                                                device_container[i]._interface[jj].ID = router_interface_id;
                                                                device_container[i]._interface[jj].INTERFACE_TYPE = "SERIAL";
                                                                device_container[i]._interface[jj].CONNECTED_TO = "";
                                                                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                                                                XmlNode router_epc_wan_ip = router_wan_node.SelectSingleNode("descendant::LAYER[@TYPE='NETWORK_LAYER']/NETWORK_PROTOCOL/PROTOCOL_PROPERTY");
                                                                if (router_epc_wan_ip.Attributes["IP_ADDRESS"] != null)
                                                                {
                                                                    device_container[i]._interface[jj].IP_ADDRESS = router_epc_wan_ip.Attributes["IP_ADDRESS"].Value;
                                                                    device_container[0]._interface[n6_no - 1].IP_ADDRESS = addNet.ip_same_network(device_container[i]._interface[jj].IP_ADDRESS);
                                                                }

                                                                mac = addNet.next_mac(mac);
                                                                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                                                                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();

                                                                jj++;

                                                                link[link_count].link_device = new LINK_DEVICE[2];
                                                                link[link_count].DEVICE_COUNT = 2;
                                                                link[link_count].link_type = "UPF_Router";
                                                                link[link_count].LINK_ID = link_count + 1;
                                                                link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
                                                                link[link_count].link_device[0].DEVICE_ID = 1;
                                                                link[link_count].link_device[0].INTERFACE_ID = n6_no;
                                                                link[link_count].link_device[0].NAME = "UPF_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
                                                                link[link_count].link_device[1].DEVICE_ID = router_id + 6;
                                                                link[link_count].link_device[1].INTERFACE_ID = router_interface_id;
                                                                link[link_count].link_device[1].NAME = "Router_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
                                                                link_count++;
                                                                n6_no++;
                                                            }

                                                            router_wan_node.ParentNode.RemoveChild(router_wan_node);
                                                        }

                                                        router_epc_count++;
                                                    }

                                                }
                                            }
                                            break;

                                        case "gNB":
                                            break;

                                        case "Access_Point":
                                            XmlNode ap_ethernet_node = device_node.SelectSingleNode("descendant:: INTERFACE[@ID = " + connected_interface_id + "]/LAYER[@TYPE = 'NETWORK_LAYER']/NETWORK_PROTOCOL/ PROTOCOL_PROPERTY");


                                            if (copy_epc_ethernet_count < epc_ethernet_count)
                                            {

                                                //ap_ethernet_node.Attributes["IP_ADDRESS"].Value = addNet.ip_same_network(ip_epc_ethernet[copy_epc_ethernet_count]);
                                                node.Attributes["INTERFACE_ID"].Value = Convert.ToString(copy_epc_ethernet_count + 1);
                                                epc_node.Attributes["ID"].Value = Convert.ToString(copy_epc_ethernet_count + 1);
                                                copy_epc_ethernet_count++;

                                            }

                                            break;

                                        case "L2_Switch":
                                            XmlNode switch_ethernet_node = device_node.SelectSingleNode("descendant:: INTERFACE[@ID= " + connected_interface_id + "]/LAYER[@TYPE = 'NETWORK_LAYER']/NETWORK_PROTOCOL/ PROTOCOL_PROPERTY");

                                            if (copy_epc_ethernet_count < epc_ethernet_count)
                                            {
                                                //switch_ethernet_node.Attributes["IP_ADDRESS"].Value = addNet.ip_same_network(ip_epc_ethernet[copy_epc_ethernet_count]);
                                                node.Attributes["INTERFACE_ID"].Value = Convert.ToString(copy_epc_ethernet_count + 1);
                                                epc_node.Attributes["ID"].Value = Convert.ToString(copy_epc_ethernet_count + 1);
                                                copy_epc_ethernet_count++;
                                            }

                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            //UPF
            for (i = 0; i < max_upf; i++)
            {
                int jj = 0;
                int kk = max_upf + max_smf + max_amf + max_switches + max_node;

                device_container[i].device.DEVICE_ID = i + 1;
                device_container[i].device.DEVICE_NAME = "UPF_" + Convert.ToString(i + 1);
                device_container[i].device.DEVICE_TYPE = "UPF";
                device_container[i].device.TYPE = "EPC";

                device_container[i]._interface[jj].ID = jj + 1;
                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N4";
                device_container[i]._interface[jj].CONNECTED_TO = "";
                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                ip_upf_n4[0] = addNet.next_ip_5g();
                device_container[i]._interface[jj].IP_ADDRESS = ip_upf_n4[0];
                //app layer ip address
                device_container[1].application.pfcp_smf_upf = ip_upf_n4[0];

                mac = addNet.next_mac(mac);
                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                jj++;


                device_container[i]._interface[jj].ID = jj + 1;
                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N3";
                device_container[i]._interface[jj].CONNECTED_TO = "";
                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                ip_upf_n3[0] = addNet.next_ip_5g();
                device_container[i]._interface[jj].IP_ADDRESS = ip_upf_n3[0];
               
                mac = addNet.next_mac(mac);
                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                jj++;

                if (epc_ethernet_count != 0 || epc_wan_count != 0)
                {
                    for (; jj < max_n6_interface + 2; jj++)
                    {
                        device_container[i]._interface[jj].ID = jj + 1;
                        device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N6";
                        device_container[i]._interface[jj].CONNECTED_TO = "";
                        mac = addNet.next_mac(mac);
                        device_container[i]._interface[jj].MAC_ADDRESS = mac;
                        device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();

                        if (epc_ethernet_count != 0)
                        {
                            device_container[i]._interface[jj].DEFAULT_GATEWAY = "11.0.0.1";
                            device_container[i]._interface[jj].IP_ADDRESS = addNet.ip_same_network("11.0.0.1");
                            Console.WriteLine("Router required to connect devices to UPF. Using default Router Settings... ");
                        }
                    }
                }
            }

            //SMF
            for (; i < (max_upf + max_smf); i++)
            {
                int jj = 0;
                device_container[i].device.DEVICE_ID = i + 1;
                device_container[i].device.DEVICE_NAME = "SMF_" + Convert.ToString(i + 1);
                device_container[i].device.DEVICE_TYPE = "SMF";
                device_container[i].device.INTERFACE_COUNT = max_smf_interface;
                device_container[i].device.TYPE = "EPC";

                device_container[i]._interface = new INTERFACE[max_smf_interface];

                device_container[i]._interface[jj].ID = jj + 1;
                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N11";
                device_container[i]._interface[jj].CONNECTED_TO = "";
                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                ip_smf_n11[0] = addNet.next_ip_5g();
                device_container[i]._interface[jj].IP_ADDRESS = ip_smf_n11[0];
                //app layer ip address            
                device_container[2].application.gtpc_amf_smf = ip_smf_n11[0];

                mac = addNet.next_mac(mac);
                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                jj++;

                device_container[i]._interface[jj].ID = jj + 1;
                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N4";
                device_container[i]._interface[jj].CONNECTED_TO = "";
                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                ip_smf_n4[0] = addNet.next_ip_5g();
                device_container[i]._interface[jj].IP_ADDRESS = ip_smf_n4[0];
                //app layer ip address
                device_container[0].application.pfcp_upf_smf = ip_smf_n4[0];

                mac = addNet.next_mac(mac);
                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();

            }

            //AMF
            for (; i < (max_upf + max_smf + max_amf); i++)
            {
                int jj = 0;
                device_container[i].device.DEVICE_ID = i + 1;
                device_container[i].device.DEVICE_NAME = "AMF_" + Convert.ToString(i + 1);
                device_container[i].device.DEVICE_TYPE = "AMF";
                device_container[i].device.INTERFACE_COUNT = max_amf_interface;
                device_container[i].device.TYPE = "EPC";

                device_container[i]._interface = new INTERFACE[max_amf_interface];

                device_container[i]._interface[jj].ID = jj + 1;
                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N11";
                device_container[i]._interface[jj].CONNECTED_TO = "";
                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                ip_amf_n11[0] = addNet.next_ip_5g();
                device_container[i]._interface[jj].IP_ADDRESS = ip_amf_n11[0];
                //app layer ip address
                device_container[1].application.gtpc_smf_amf = ip_amf_n11[0];

                mac = addNet.next_mac(mac);
                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                jj++;


                device_container[i]._interface[jj].ID = jj + 1;
                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N1_N2";
                device_container[i]._interface[jj].CONNECTED_TO = "";
                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                ip_amf_n1_n2[0] = addNet.next_ip_5g();
                device_container[i]._interface[jj].IP_ADDRESS = ip_amf_n1_n2[0];

                mac = addNet.next_mac(mac);
                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();

            }

            //Switches
            for (; i < (max_upf + max_smf + max_amf + max_switches); i++)
            {

                device_container[i].device.DEVICE_ID = i + 1;
                device_container[i].device.DEVICE_NAME = "L2_Switch_" + Convert.ToString(i + 1);
                if (i == 3)
                {
                    int jj = 0;
                    device_container[i].device.DEVICE_TYPE = "L2_SWITCH_UPF";
                    device_container[i].device.INTERFACE_COUNT = max_switch_upf_interface;
                    device_container[i]._interface = new INTERFACE[max_switch_upf_interface];

                    for (; jj < max_switch_upf_interface; jj++)
                    {
                        device_container[i]._interface[jj].ID = jj + 1;
                        device_container[i]._interface[jj].INTERFACE_TYPE = "ETHERNET";
                        device_container[i]._interface[jj].CONNECTED_TO = "";
                        mac = addNet.next_mac(mac);
                        device_container[i]._interface[jj].MAC_ADDRESS = mac;
                        device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                        device_container[i]._interface[jj].SWITCH_ID = mac;

                    }
                }
                else if (i == 5)
                {
                    int jj = 0;
                    device_container[i].device.DEVICE_TYPE = "L2_SWITCH_gNB";
                    device_container[i].device.INTERFACE_COUNT = max_switch_gnb_interface;
                    device_container[i]._interface = new INTERFACE[max_switch_gnb_interface];

                    for (; jj < max_switch_gnb_interface; jj++)
                    {
                        device_container[i]._interface[jj].ID = jj + 1;
                        device_container[i]._interface[jj].INTERFACE_TYPE = "ETHERNET";
                        device_container[i]._interface[jj].CONNECTED_TO = "";
                        mac = addNet.next_mac(mac);
                        device_container[i]._interface[jj].MAC_ADDRESS = mac;
                        device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                        device_container[i]._interface[jj].SWITCH_ID = mac;
                    }
                }
                else
                {//can connect extra switches only to AMF
                    int jj = 0;
                    device_container[i].device.DEVICE_TYPE = "L2_SWITCH_AMF";
                    device_container[i].device.INTERFACE_COUNT = max_switch_amf_interface;
                    device_container[i]._interface = new INTERFACE[max_switch_amf_interface];

                    for (; jj < max_switch_amf_interface; jj++)
                    {
                        device_container[i]._interface[jj].ID = jj + 1;
                        device_container[i]._interface[jj].INTERFACE_TYPE = "ETHERNET";
                        device_container[i]._interface[jj].CONNECTED_TO = "";
                        mac = addNet.next_mac(mac);
                        device_container[i]._interface[jj].MAC_ADDRESS = mac;
                        device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                        device_container[i]._interface[jj].SWITCH_ID = mac;
                    }
                }

                device_container[i].device.WIRESHARK_OPTION = "Disable";
                device_container[i].device.TYPE = "SWITCH";
            }

            link[link_count].link_device = new LINK_DEVICE[2];
            link[link_count].DEVICE_COUNT = 2;
            link[link_count].link_type = "SMF_AMF";
            link[link_count].LINK_ID = link_count + 1;
            link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
            link[link_count].link_device[0].DEVICE_ID = 2;
            link[link_count].link_device[0].INTERFACE_ID = 1;
            link[link_count].link_device[0].NAME = "SMF_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
            link[link_count].link_device[1].DEVICE_ID = 3;
            link[link_count].link_device[1].INTERFACE_ID = 1;
            link[link_count].link_device[1].NAME = "AMF_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
            link_count++;

            link[link_count].link_device = new LINK_DEVICE[2];
            link[link_count].DEVICE_COUNT = 2;
            link[link_count].link_type = "UPF_SMF";
            link[link_count].LINK_ID = link_count + 1;
            link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
            link[link_count].link_device[0].DEVICE_ID = 1;
            link[link_count].link_device[0].INTERFACE_ID = 1;
            link[link_count].link_device[0].NAME = "UPF_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
            link[link_count].link_device[1].DEVICE_ID = 2;
            link[link_count].link_device[1].INTERFACE_ID = 2;
            link[link_count].link_device[1].NAME = "SMF_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
            link_count++;

            link[link_count].link_device = new LINK_DEVICE[2];
            link[link_count].DEVICE_COUNT = 2;
            link[link_count].link_type = "UPF_Switch";
            link[link_count].LINK_ID = link_count + 1;
            link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
            link[link_count].link_device[0].DEVICE_ID = 1;
            link[link_count].link_device[0].INTERFACE_ID = 2;
            link[link_count].link_device[0].NAME = "UPF_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
            link[link_count].link_device[1].DEVICE_ID = 4;
            link[link_count].link_device[1].INTERFACE_ID = 1;
            link[link_count].link_device[1].NAME = "L2_Switch_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
            link_count++;

            link[link_count].link_device = new LINK_DEVICE[2];
            link[link_count].DEVICE_COUNT = 2;
            link[link_count].link_type = "AMF_Swicth";
            link[link_count].LINK_ID = link_count + 1;
            link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
            link[link_count].link_device[0].DEVICE_ID = 3;
            link[link_count].link_device[0].INTERFACE_ID = 2;
            link[link_count].link_device[0].NAME = "AMF_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
            link[link_count].link_device[1].DEVICE_ID = 5;
            link[link_count].link_device[1].INTERFACE_ID = 1;
            link[link_count].link_device[1].NAME = "L2_Switch_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
            link_count++;

            /**********************************************************************************************/
            int count_node = 0;
            int non_epc_router_count = 0;
            int count_ue = 0;
            int count_gNB = 0;
            int count_wireless_node = 0;
            int count_ap = 0;
            int count_switch = 0;
            int switch_1_interface_count = 1;
            int switch_2_interface_count = 1;
            int switch_3_interface_count = 0;

            // adding changes to router, wired node, gNB, UE and EPC 
            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='Wired_Node']"))
            {
                if (node.Attributes != null)
                {
                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);

                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            var inter_count = node.Attributes["INTERFACE_COUNT"].Value;

                            if (inter_count != null)
                            {
                                i = max_upf + max_smf + max_amf + max_switches;
                                i += count_node;

                                if (i < (max_upf + max_smf + max_amf + max_switches + max_node))
                                {
                                    int jj = 0;

                                    node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id + 6);

                                    device_container[i].device.DEVICE_ID = device_id + 6;
                                    device_container[i].device.DEVICE_TYPE = "WIREDNODE";

                                    device_container[i]._interface = new INTERFACE[0];

                                    foreach (XmlNode node_ethernet_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='ETHERNET']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                    {
                                        mac = addNet.next_mac(mac);
                                        node_ethernet_mac.Attributes["MAC_ADDRESS"].Value = mac;
                                    }

                                    foreach (XmlNode wired_node_ethernet in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='ETHERNET']/LAYER[@TYPE = 'PHYSICAL_LAYER']/PROTOCOL/ PROTOCOL_PROPERTY"))
                                    {
                                        if (wired_node_ethernet.Attributes != null)
                                            wired_node_ethernet.Attributes["CONNECTED_TO"].Value = "";
                                    }
                                }
                                count_node = count_node + 1;
                            }
                        }
                    }
                }
            }

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='Router']"))
            {
                if (node.Attributes != null)
                {
                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);

                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id + 6);
                        node.Attributes["DEVICE_NAME"].Value = "Router_" + Convert.ToString(device_id + 6);


                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            var inter_count = node.Attributes["INTERFACE_COUNT"].Value;

                            if (inter_count != null)
                            {
                                i = max_upf + max_smf + max_amf + max_switches + max_node + router_epc_count + non_epc_router_count;
                                if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router))
                                {

                                    device_container[i].device.DEVICE_ID = device_id + 6;
                                    device_container[i].device.INTERFACE_COUNT = 0;
                                    device_container[i].device.DEVICE_TYPE = "ROUTER";

                                    device_container[i]._interface = new INTERFACE[device_container[i].device.INTERFACE_COUNT];


                                    non_epc_router_count++;
                                }
                            }
                        }
                    }

                    foreach (XmlNode router_ethernet in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='ETHERNET']/LAYER[@TYPE = 'PHYSICAL_LAYER']/PROTOCOL/ PROTOCOL_PROPERTY"))
                    {
                        if (router_ethernet.Attributes != null)
                            router_ethernet.Attributes["CONNECTED_TO"].Value = "";
                    }

                    foreach (XmlNode router_ethernet_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='ETHERNET']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                    {
                        mac = addNet.next_mac(mac);
                        router_ethernet_mac.Attributes["MAC_ADDRESS"].Value = mac;
                    }

                    foreach (XmlNode router_wan_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='WAN']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                    {
                        mac = addNet.next_mac(mac);
                        router_wan_mac.Attributes["MAC_ADDRESS"].Value = mac;
                    }

                    foreach (XmlNode router_area_id in node.SelectNodes("descendant::LAYER[@TYPE = 'APPLICATION_LAYER']/ROUTING_PROTOCOL/PROTOCOL_PROPERTY/INTERFACE[@AREAID='0.0.0.0']"))
                    {
                        if (router_area_id.Attributes != null)
                            router_area_id.ParentNode.RemoveChild(router_area_id);
                    }
                }
            }

            if (epc_ethernet_count == 0)
            {
                foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='EPC']"))
                {
                    node.ParentNode.RemoveChild(node);

                }
            }
            else if (epc_ethernet_count != 0)
            {
                max_router = max_router + 1;
                foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='EPC']"))
                {
                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        int device_id = 0;
                        device_id = total_device;
                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            int inter_count = Convert.ToInt32(node.Attributes["INTERFACE_COUNT"].Value);

                            i = max_upf + max_smf + max_amf + max_switches + max_node + router_epc_count + non_epc_router_count;
                            if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router))
                            {
                                int jj = 0;
                                node.Attributes["DEFAULT_DEVICE_NAME"].Value = "Router";
                                node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id);
                                node.Attributes["DEVICE_IMAGE"].Value = "InternalRouter.png";
                                node.Attributes["DEVICE_NAME"].Value = "Router_" + device_id;
                                node.Attributes["DEVICE_TYPE"].Value = "ROUTER";
                                node.Attributes["INTERFACE_COUNT"].Value = Convert.ToString(epc_ethernet_count + 1);
                                node.Attributes["TYPE"].Value = "ROUTER";
                                nsWriter.add_attribute(xmlDoc, node, "WIRESHARK_OPTION", "Disable");

                                device_container[i].device.DEVICE_ID = device_id;
                                device_container[i].device.INTERFACE_COUNT = 1;
                                device_container[i].device.DEVICE_TYPE = "ROUTER";
                                device_container[i]._interface = new INTERFACE[1];

                                device_container[i]._interface[jj].ID = inter_count + 1;
                                device_container[i]._interface[jj].INTERFACE_TYPE = "SERIAL";
                                device_container[i]._interface[jj].CONNECTED_TO = "";
                                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";
                                device_container[i]._interface[jj].IP_ADDRESS = "11.0.0.1";

                                mac = addNet.next_mac(mac);
                                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                                jj++;

                                foreach (XmlNode epc_wan in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='WAN']"))
                                {
                                    if (epc_wan.Attributes != null)
                                    {
                                        epc_wan.ParentNode.RemoveChild(epc_wan);
                                    }
                                }

                                foreach (XmlNode epc_lte_nr in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']"))
                                {
                                    if (epc_lte_nr.Attributes != null)
                                    {
                                        epc_lte_nr.ParentNode.RemoveChild(epc_lte_nr);
                                    }
                                }

                                foreach (XmlNode epc_ethernet_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='ETHERNET']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                {
                                    mac = addNet.next_mac(mac);
                                    epc_ethernet_mac.Attributes["MAC_ADDRESS"].Value = mac;
                                }
                            }

                            link[link_count].link_device = new LINK_DEVICE[2];
                            link[link_count].DEVICE_COUNT = 2;
                            link[link_count].link_type = "UPF_Router";
                            link[link_count].LINK_ID = link_count + 1;
                            link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
                            link[link_count].link_device[0].DEVICE_ID = 1;
                            link[link_count].link_device[0].INTERFACE_ID = n6_no;
                            link[link_count].link_device[0].NAME = "UPF_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
                            link[link_count].link_device[1].DEVICE_ID = total_device;
                            link[link_count].link_device[1].INTERFACE_ID = inter_count + 1;
                            link[link_count].link_device[1].NAME = "Router_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
                            link_count++;
                            n6_no++;
                        }
                    }
                }
            }

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='gNB']"))
            {
                if (node.Attributes["DEFAULT_DEVICE_NAME"] != null)
                {
                    var device_name = node.Attributes["DEFAULT_DEVICE_NAME"].Value;

                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);

                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            int inter_count = Convert.ToInt32(node.Attributes["INTERFACE_COUNT"].Value);

                            i = max_upf + max_smf + max_amf + max_switches + max_node + max_router;
                            i += count_gNB;

                            if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb))
                            {
                                int jj = 0;
                                int gnbid = 0;

                                gnbid = device_id + 6;
                                node.Attributes["DEVICE_ID"].Value = Convert.ToString(gnbid);
                                node.Attributes["DEVICE_NAME"].Value = "gNB_" + Convert.ToString(gnbid);

                                int gnb_interface = inter_count - 1 + 3; //minus epc + 3 switches
                                node.Attributes["INTERFACE_COUNT"].Value = Convert.ToString(gnb_interface);

                                device_container[i].device.DEVICE_ID = device_id + 6;

                                device_container[i].device.DEVICE_TYPE = "LTE_gNB";
                                device_container[i].device.INTERFACE_COUNT = gnb_interface;
                                device_container[i]._interface = new INTERFACE[gnb_interface];

                                device_container[i]._interface[jj].ID = jj + 1;
                                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N3";
                                device_container[i]._interface[jj].CONNECTED_TO = "";
                                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";

                                ip_gnb_n3[0] = addNet.next_ip_5g();                              
                                device_container[i]._interface[jj].IP_ADDRESS = ip_gnb_n3[0];

                                //app layer ip address
                                device_container[0].application.gtpu_upf_gnb = ip_gnb_n3[0];
                                

                                mac = addNet.next_mac(mac);
                                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                                jj++;

                                device_container[i]._interface[jj].ID = jj + 1;
                                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_N1_N2";
                                device_container[i]._interface[jj].DEFAULT_GATEWAY = ip_amf_n1_n2[0];//should add amf interface ip

                                ip_gnb_n1_n2[0] = addNet.next_ip_5g();
                                device_container[i]._interface[jj].IP_ADDRESS = ip_gnb_n1_n2[0];

                                //app layer ip address
                                device_container[2].application.ngap_amf_gnb= ip_gnb_n1_n2[0];


                                mac = addNet.next_mac(mac);
                                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                                jj++;

                                device_container[i]._interface[jj].ID = jj + 1;
                                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_XN";
                                device_container[i]._interface[jj].DEFAULT_GATEWAY = "";//should add amf interface ip
                                device_container[i]._interface[jj].IP_ADDRESS = addNet.next_ip_5g();

                                mac = addNet.next_mac(mac);
                                device_container[i]._interface[jj].MAC_ADDRESS = mac;
                                device_container[i]._interface[jj].SUBNET_MASK = addNet.subnet_mask();
                                jj++;

                                device_container[i]._interface[jj].INTERFACE_TYPE = "5G_RAN";

                                device_container[i].application.ngap_gnb_amf = ip_amf_n1_n2[0];
                                device_container[i].application.ngap_gnb_smf = ip_smf_n11[0];
                                device_container[i].application.gtpu_gnb_upf = ip_upf_n3[0];      
                                

                                foreach (XmlNode gnb_ran_interface in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                {
                                    mac = addNet.next_mac(mac);
                                    gnb_ran_interface.Attributes["MAC_ADDRESS"].Value = mac;
                                }

                                if (switch_1_interface_count < max_switch_upf_interface) {
                                    link[link_count].link_device = new LINK_DEVICE[2];
                                    link[link_count].DEVICE_COUNT = 2;
                                    link[link_count].link_type = "gNB_Switch1";
                                    link[link_count].LINK_ID = link_count + 1;
                                    link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
                                    link[link_count].link_device[0].DEVICE_ID = 4;
                                    link[link_count].link_device[0].INTERFACE_ID = switch_1_interface_count + 1;
                                    link[link_count].link_device[0].NAME = "L2_Switch_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
                                    link[link_count].link_device[1].DEVICE_ID = gnbid;
                                    link[link_count].link_device[1].INTERFACE_ID = 1;
                                    link[link_count].link_device[1].NAME = "gNB_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
                                    link_count++;
                                    switch_1_interface_count++;
                                }
                                if (switch_2_interface_count < max_switch_amf_interface)
                                {
                                    link[link_count].link_device = new LINK_DEVICE[2];
                                    link[link_count].DEVICE_COUNT = 2;
                                    link[link_count].link_type = "gNB_Switch2";
                                    link[link_count].LINK_ID = link_count + 1;
                                    link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
                                    link[link_count].link_device[0].DEVICE_ID = 5;
                                    link[link_count].link_device[0].INTERFACE_ID = switch_2_interface_count + 1;
                                    link[link_count].link_device[0].NAME = "L2_Switch_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
                                    link[link_count].link_device[1].DEVICE_ID = gnbid;
                                    link[link_count].link_device[1].INTERFACE_ID = 2;
                                    link[link_count].link_device[1].NAME = "gNB_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
                                    link_count++;
                                    switch_2_interface_count++;
                                }

                                if (switch_3_interface_count < max_switch_gnb_interface) {
                                    link[link_count].link_device = new LINK_DEVICE[2];
                                    link[link_count].DEVICE_COUNT = 2;
                                    link[link_count].link_type = "gNB_Switch3";
                                    link[link_count].LINK_ID = link_count + 1;
                                    link[link_count].LINK_NAME = Convert.ToString(link_count + 1);
                                    link[link_count].link_device[0].DEVICE_ID = 6;
                                    link[link_count].link_device[0].INTERFACE_ID = switch_3_interface_count + 1;
                                    link[link_count].link_device[0].NAME = "L2_Switch_" + Convert.ToString(link[link_count].link_device[0].DEVICE_ID);
                                    link[link_count].link_device[1].DEVICE_ID = gnbid;
                                    link[link_count].link_device[1].INTERFACE_ID = 3;
                                    link[link_count].link_device[1].NAME = "gNB_" + Convert.ToString(link[link_count].link_device[1].DEVICE_ID);
                                    link_count++;
                                    switch_3_interface_count++;
                                }

                            }
                            count_gNB = count_gNB + 1;

                        }
                    }
                }
            }

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='UE']"))
            {
                if (node.Attributes["DEFAULT_DEVICE_NAME"] != null)
                {
                    var device_name = node.Attributes["DEFAULT_DEVICE_NAME"].Value;

                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);

                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            var inter_count = Convert.ToInt32(node.Attributes["INTERFACE_COUNT"].Value);

                            i = max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb;
                            i += count_ue;

                            if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue))
                            {
                                node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id + 6);

                                device_container[i].device.DEVICE_ID = device_id + 6;
                                device_container[i].device.DEVICE_TYPE = "LTE_NR_UE";
                                device_container[i]._interface = new INTERFACE[inter_count];

                                foreach (XmlNode ue_interface in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='NETWORK_LAYER']/NETWORK_PROTOCOL/PROTOCOL_PROPERTY"))
                                {
                                    ue_interface.Attributes["DEFAULT_GATEWAY"].Value = ip_upf_n3[0];
                                    ue_interface.Attributes["IP_ADDRESS"].Value = addNet.next_ip_5g();
                                    ue_interface.Attributes["SUBNET_MASK"].Value = addNet.subnet_mask();
                                }
                            }
                            count_ue = count_ue + 1;
                        }

                        //foreach (XmlNode mobility_node in node.SelectNodes("descendant:: POS_3D/MOBILITY"))
                        //{
                            
                        //    if (mobility_node.Attributes["MODEL"].Value == "FILE_BASED_MOBILITY")
                        //    {
                        //        string sourceFile = input_file_location + "\\mobility.txt";
                        //        string destinationFile = output_file_location + "\\mobility.txt";
                        //        try
                        //        {
                        //            File.Copy(sourceFile, destinationFile, true);
                        //        }
                        //        catch (IOException iox)
                        //        {
                        //            Console.WriteLine(iox.Message);
                        //        }
                        //        string[] Lines = File.ReadAllLines(destinationFile);
                        //        int count = 0;
                        //        string[] ue_id = new string[max_ue];
                        //        int ue_number = 0;
                        //        string[] ue_num = new string[max_ue];
                                

                        //        for (int ij = 0; ij < Lines.Length; ++ij)
                        //        {
                        //            string line = Lines[ij];
                        //            if (line.Contains("#Initial"))
                        //            {
                        //                // string line;
                        //                string[] line_split = line.Split(' ');
                        //                int num = Convert.ToInt32(line_split[line_split.Length - 1]) - 1;

                        //                ue_id[count] = Convert.ToString(num);

                        //                ue_number = Convert.ToInt32(ue_id[count]) - 1 + 6;
                        //                ue_num[count] = Convert.ToString(ue_number);

                        //                string ue_name = "node_(" + ue_id[count] + ")";
                        //                string changed_ue_name = "node_(" + ue_num[count] + ")";
                        //                string text = File.ReadAllText(destinationFile);
                        //                File.WriteAllText(destinationFile, text.Replace(ue_name, changed_ue_name));
                        //                count++;

                        //            }                                
                        //        }
                        //    }
                        //}
                    }
                }
            }
    
            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='Wireless_Node']"))
            {
                if (node.Attributes["DEFAULT_DEVICE_NAME"] != null)
                {
                    var device_name = node.Attributes["DEFAULT_DEVICE_NAME"].Value;
                  
                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);
                        
                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            int inter_count = Convert.ToInt32(node.Attributes["INTERFACE_COUNT"].Value);
                          
                            i = max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue;
                            i += count_wireless_node;

                            if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue + max_wireless_node))
                            {
                                node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id + 6);
                                node.Attributes["INTERFACE_COUNT"].Value = Convert.ToString(inter_count);

                                device_container[i].device.DEVICE_ID = device_id;
                                device_container[i].device.DEVICE_TYPE = "WIRELESSNODE";

                                device_container[i]._interface = new INTERFACE[inter_count];

                                foreach (XmlNode wirelessnode_wireless_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='WIRELESS']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                {
                                    mac = addNet.next_mac(mac);
                                    wirelessnode_wireless_mac.Attributes["MAC_ADDRESS"].Value = mac;
                                }

                            }
                            count_wireless_node = count_wireless_node + 1;
                            
                        }
                    }
                }
            }

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='Access_Point']"))
            {
                if (node.Attributes != null)
                {
                    var device_name = node.Attributes["DEFAULT_DEVICE_NAME"].Value;

                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);
                       
                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            var inter_count = Convert.ToInt32(node.Attributes["INTERFACE_COUNT"].Value);
                           
                            i = max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue + max_wireless_node;
                            i += count_ap;

                            if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue + max_wireless_node + max_ap))
                            {
                                node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id + 6);

                                device_container[i].device.DEVICE_ID = device_id + 6;
                                device_container[i].device.DEVICE_TYPE = "ACCESSPOINT";

                                device_container[i]._interface = new INTERFACE[inter_count];

                                foreach (XmlNode ap_wireless_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='WIRELESS']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                {
                                    mac = addNet.next_mac(mac);
                                    ap_wireless_mac.Attributes["MAC_ADDRESS"].Value = mac;
                                }
                            }
                            count_ap = count_ap + 1;                            
                        }
                    }
                }
            }

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@DEFAULT_DEVICE_NAME ='L2_Switch']"))
            {
                if (node.Attributes["DEFAULT_DEVICE_NAME"] != null)
                {
                    var device_name = node.Attributes["DEFAULT_DEVICE_NAME"].Value;

                    if (node.Attributes["DEVICE_ID"] != null && node.Attributes["DEVICE_ID"].Value != null)
                    {
                        var device_id = Convert.ToInt32(node.Attributes["DEVICE_ID"].Value);

                        if (device_id > epc_id)
                            device_id = device_id - 1;

                        if (node.Attributes["INTERFACE_COUNT"] != null && node.Attributes["INTERFACE_COUNT"].Value != null)
                        {
                            var inter_count = Convert.ToInt32(node.Attributes["INTERFACE_COUNT"].Value);

                            i = max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue + max_wireless_node + max_ap;
                            i += count_switch;

                            if (i < (max_upf + max_smf + max_amf + max_switches + max_node + max_router + max_gnb + max_ue + max_wireless_node + max_ap + max_other_switches))
                            {
                                node.Attributes["DEVICE_ID"].Value = Convert.ToString(device_id + 6);
                                node.Attributes["DEVICE_NAME"].Value = "L2_Switch_" + Convert.ToString(device_id + 6);

                                device_container[i].device.DEVICE_ID = device_id + 6;
                                device_container[i].device.DEVICE_TYPE = "Switch";

                                device_container[i]._interface = new INTERFACE[inter_count];

                                foreach (XmlNode switch_ethernet_mac in node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='ETHERNET']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                {
                                    mac = addNet.next_mac(mac);
                                    switch_ethernet_mac.Attributes["MAC_ADDRESS"].Value = mac;
                                    switch_ethernet_mac.Attributes["SWITCH_ID"].Value = mac;
                                }
                            }
                            count_switch = count_switch + 1;
                            
                        }
                    }
                }
            }
           
           //Adding processing delay            
            foreach (XmlNode interface_node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE/LAYER[@TYPE='NETWORK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
            {
                nsWriter.add_attribute(xmlDoc, interface_node, "PROCESSING_DELAY_US", "0.0");                              
            }

            //Adding link information to Connection element
            foreach (XmlNode linknode in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION/CONNECTION/LINK"))
            {
                string DL_MIMO_COUNT = null;
                string UL_MIMO_COUNT = null;
 
                foreach (XmlNode node in linknode.SelectNodes("descendant::DEVICE"))
                {
                    if (node.Attributes["DEVICE_ID"] != null)
                    {
                        int device_number = 0;
                        int new_device_number = 0;
                        var device_id = node.Attributes["DEVICE_ID"].Value;  
                        
                        if (device_id != null)
                        {
                            // to add new device id to element device in link
                            device_number = Convert.ToInt32(device_id);

                            if (device_number > epc_id)
                                new_device_number = device_number - 1 + 6;
                            else
                                new_device_number = device_number + 6;

                            node.Attributes["DEVICE_ID"].Value = Convert.ToString(new_device_number);
                        }
                    }
                }

                foreach (XmlNode node in linknode.SelectNodes("descendant::DEVICE"))
                {
                    if (node.Attributes["NAME"] != null && node.Attributes["DEVICE_ID"] != null)
                    {
                        string[] device_split = null;
                        string[] router_split = null;
                        string connected_device = null;
                        string[] connected_device_split = null;
                        string device = null;

                        var device_name = node.Attributes["NAME"].Value;
                        var device_id = node.Attributes["DEVICE_ID"].Value;
                        XmlNode previousNode = node.PreviousSibling;
                        XmlNode nextNode = node.NextSibling;

                        if (device_name != null && device_id != null)
                        {
                            // to add new device name to element device in link
                            device_split = device_name.Split('_');
                            router_split = device_name.Split('_');
                            device_split[device_split.Length - 1] = Convert.ToString(device_id);

                            for (int dd = 0; dd < device_split.Length - 1; dd++)
                                device = device + device_split[dd] + "_";

                            device += device_split[device_split.Length - 1];

                            // to find sibling nodes of device element in link
                            if (nextNode.Attributes["NAME"] != null && nextNode.Attributes["NAME"].Value != null)
                                connected_device = nextNode.Attributes["NAME"].Value;

                            else if (previousNode.Attributes["NAME"] != null && previousNode.Attributes["NAME"].Value != null)
                                connected_device = previousNode.Attributes["NAME"].Value;

                            connected_device_split = connected_device.Split('_');

                            // For gnb - ue link
                            if (device_split[0] == "gNB")
                            {                            
                                node.Attributes["NAME"].Value = device;
                                if (connected_device_split[0] != "EPC")
                                {
                                    node.Attributes["INTERFACE_ID"].Value = "4";
                                    XmlNode gnb_node = xmlDoc.SelectSingleNode("descendant:: NETWORK_CONFIGURATION/DEVICE_CONFIGURATION/DEVICE[@DEVICE_ID = " + device_id + "]");
                                    
                                    if (gnb_node.Attributes["DEVICE_ID"] != null && gnb_node.Attributes["DEVICE_ID"].Value != null)
                                    {
                                        foreach (XmlNode gnb_ran_physical_layer in gnb_node.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='PHYSICAL_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                        {
                                            DL_MIMO_COUNT = gnb_ran_physical_layer.Attributes["DOWNLINK_MIMO_LAYER_COUNT"].Value;
                                            UL_MIMO_COUNT = gnb_ran_physical_layer.Attributes["UPLINK_MIMO_LAYER_COUNT"].Value;

                                            if (gnb_ran_physical_layer.SelectSingleNode("descendant::ANTENNA") == null)
                                            {
                                                XmlNode gnb_antenna = nsWriter.add_element(xmlDoc, gnb_ran_physical_layer, "ANTENNA");
                                                nsWriter.add_attribute(xmlDoc, gnb_antenna, "RX_ANTENNA_COUNT", UL_MIMO_COUNT);
                                                nsWriter.add_attribute(xmlDoc, gnb_antenna, "TX_ANTENNA_COUNT", DL_MIMO_COUNT);
                                            }
                                        }
                                    }

                                    XmlNode link_gnb = node.ParentNode;
                                    foreach (XmlNode ue_node in link_gnb.ChildNodes)
                                    {
                                        if (ue_node.Attributes["NAME"] != null && ue_node.Attributes["NAME"].Value != null)
                                        {
                                            var ue_name = ue_node.Attributes["NAME"].Value;
                                            string[] ue_split = null;
                                            ue_split = ue_name.Split('_');

                                            if (ue_split[0] != "gNB")
                                            {
                                                var ue_id = ue_node.Attributes["DEVICE_ID"].Value;
                                                int ue_number = 0;
                                                ue_number = Convert.ToInt32(ue_id);

                                                if (ue_node.Attributes["DEVICE_ID"] != null && ue_node.Attributes["DEVICE_ID"].Value != null)
                                                {
                                                    XmlNode ue = xmlDoc.SelectSingleNode("descendant::NETWORK_CONFIGURATION/DEVICE_CONFIGURATION/DEVICE[@DEVICE_ID =  " + ue_number + "]");
                                                    foreach (XmlNode ue_ran_physical_layer in ue.SelectNodes("descendant:: INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='PHYSICAL_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                                                    {
                                                        nsWriter.add_attribute(xmlDoc, ue_ran_physical_layer, "CONNECTED_TO", "");

                                                        XmlNode ue_antenna = nsWriter.add_element(xmlDoc, ue_ran_physical_layer, "ANTENNA");
                                                        nsWriter.add_attribute(xmlDoc, ue_antenna, "RX_ANTENNA_COUNT", DL_MIMO_COUNT);
                                                        nsWriter.add_attribute(xmlDoc, ue_antenna, "TX_ANTENNA_COUNT", UL_MIMO_COUNT);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                               
                            }

                            if (device_split[0] == "EPC")
                            {
                                if (connected_device_split[0] != "gNB" && connected_device_split[0] != "Router" && epc_ethernet_count != 0)
                                {
                                    string device_new_router = "Router" + "_" + Convert.ToString(total_device);

                                    node.Attributes["NAME"].Value = device_new_router;
                                    node.Attributes["DEVICE_ID"].Value = Convert.ToString(total_device);
                                }
                                else if (connected_device_split[0] == "gNB" || connected_device_split[0] == "Router")
                                {
                                    XmlNode parent_epc = linknode.ParentNode;
                                    parent_epc.RemoveChild(linknode);
                                }
                            }

                            if (device_split[0] == "Router" || device_split.Length - 1 >=  1 && device_split[1] == "Switch")
                            {
                                node.Attributes["NAME"].Value = device;
                            }
                                
                        }
                    }
                }
            }

            int link_new_count = link_count;
            int linkcount = link_count;

            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION/CONNECTION/LINK"))
            {                             
                int link_id = link_new_count + 1;
                int link_name = link_new_count + 1;

                node.Attributes["LINK_ID"].Value = Convert.ToString(link_id);
                node.Attributes["LINK_NAME"].Value = Convert.ToString(link_name);

                link_new_count++;
                linkcount++;
            }

            /***************************************************************************************/
            /***************************************************************************************/
            foreach (XmlNode node in xmlDoc.SelectNodes("descendant::NETWORK_CONFIGURATION/APPLICATION_CONFIGURATION/APPLICATION"))
            {
                int dest_id = 0, source_id = 0, peer_id = 0, seeder_id = 0, client_id = 0, server_id = 0;
                               
                if (node.Attributes["DESTINATION_ID"] != null && node.Attributes["SOURCE_ID"] != null) 
                {
                    dest_id = Convert.ToInt32(node.Attributes["DESTINATION_ID"].Value);
                    source_id = Convert.ToInt32(node.Attributes["SOURCE_ID"].Value);
                    if (dest_id > epc_id)
                        dest_id = dest_id - 1 + 6;
                    else
                        dest_id = dest_id + 6;

                    if (source_id > epc_id)
                        source_id = source_id - 1 + 6;
                    else
                        source_id = source_id + 6;

                    node.Attributes["DESTINATION_ID"].Value = Convert.ToString(dest_id);
                    node.Attributes["SOURCE_ID"].Value = Convert.ToString(source_id);

                }

                if (node.Attributes["PEER_ID"] != null && node.Attributes["SEEDER_ID"] != null)
                {
                    peer_id = Convert.ToInt32(node.Attributes["PEER_ID"].Value);
                    seeder_id = Convert.ToInt32(node.Attributes["SEEDER_ID"].Value);
                    if (peer_id > epc_id)
                        peer_id = peer_id - 1 + 6;
                    else
                        peer_id = peer_id + 6;

                    if (seeder_id > epc_id)
                        seeder_id = seeder_id - 1 + 6;
                    else
                        seeder_id = seeder_id + 6;

                    node.Attributes["PEER_ID"].Value = Convert.ToString(peer_id);
                    node.Attributes["SEEDER_ID"].Value = Convert.ToString(seeder_id);
                }

                if (node.Attributes["CLIENT_ID"] != null && node.Attributes["SERVER_ID"] != null)
                {
                    client_id = Convert.ToInt32(node.Attributes["CLIENT_ID"].Value);
                    server_id = Convert.ToInt32(node.Attributes["SERVER_ID"].Value);
                    if (client_id > epc_id)
                        client_id = client_id - 1 + 6;
                    else
                        client_id = client_id + 6;

                    if (server_id > epc_id)
                        server_id = server_id - 1 + 6;
                    else
                        server_id = server_id + 6;

                    node.Attributes["CLIENT_ID"].Value = Convert.ToString(client_id);
                    node.Attributes["SERVER_ID"].Value = Convert.ToString(server_id);
                }
                                 
            }

            var packet_trace = xmlDoc.SelectSingleNode("descendant:: STATISTICS_COLLECTION/PACKET_TRACE");
            var file_path = packet_trace.Attributes["FILE_PATH"].Value;

            if (file_path != null)
            {
                string[] file_path_split = file_path.Split('/');
                packet_trace.Attributes["FILE_PATH"].Value = file_path_split[0];
            }
            
            var event_trace = xmlDoc.SelectSingleNode("descendant::STATISTICS_COLLECTION/EVENT_TRACE");
            string event_file_path = event_trace.Attributes["FILE_PATH"].Value;

            if (event_file_path != ".")
            {
                event_file_path = event_trace.Attributes["FILE_PATH"].Value;
                string[] event_file_path_split = event_file_path.Split('/');
                event_trace.Attributes["FILE_PATH"].Value = event_file_path_split[0];
            }

            ////Adding network elements
            //addNet.add_network(xmlDoc, root, nwConfig, total_device, link_count, device_container, link, config_helper_location);
            //nsWriter.save_document(xmlDoc, output_file_location + "\\Configuration.netsim");

            addNet.add_network(xmlDoc, root, nwConfig, total_device, link_count, device_container, link, "C:/Users/mridula/Desktop/automation_development");
            nsWriter.save_document(xmlDoc, "C:/Users/mridula/Desktop/Configuration.netsim");

            //xmlDoc.Save(@"C:/Users/mridula/Desktop/Configuration_final.xml");
        }
    }
}