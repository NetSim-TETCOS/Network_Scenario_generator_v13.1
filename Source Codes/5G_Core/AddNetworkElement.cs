using System;
using System.Net;
using System.Text;
using System.Xml;

namespace _5G_Core
{
    class AddNetworkElement
    {
        private static XmlWriter nsWriter = new XmlWriter();

        //public string next_ip_5g(int id, int _4th)
        //{
        //    int _4th_octate = _4th, _3rd_octate = 0, _2nd_octate = 0, _1st_octate = 10;

        //    _3rd_octate += id % 200;
        //    _2nd_octate += (id / 200) % 200;
        //    _1st_octate += ((id / 200) / 200) % 200;
        //    string ip = Convert.ToString(_1st_octate) + "." + Convert.ToString(_2nd_octate) + "." +
        //                Convert.ToString(_3rd_octate) + "." + Convert.ToString(_4th_octate);
        //    return ip;
        //}

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public string next_ip_5g()
        {
            lock (syncLock)
            { // synchronize
                return $"10.0.{random.Next(0, 255)}.{random.Next(0, 255)}";
            }
        }
        public string next_ip(int id, int _4th)
        {
            int _4th_octate = _4th, _3rd_octate = 1, _2nd_octate = 1, _1st_octate = 11; 
            _3rd_octate += (id / 200) % 200;
            _2nd_octate += id % 200;
            _1st_octate += ((id / 200) / 200) % 200;
            string ip = Convert.ToString(_1st_octate) + "." + Convert.ToString(_2nd_octate) + "." +
                        Convert.ToString(_3rd_octate) + "." + Convert.ToString(_4th_octate);
            return ip;
        }
        public string ip_same_network(string add)
        {
           // string ad = "12.4.3.5";
            string[] address_split = add.Split('.');
       
            int[] address_octate = new int[address_split.Length];

            for (int i = 0; i < address_split.Length; i++)
                address_octate[i] = Convert.ToInt32(address_split[i]);
                        
            int _4th_octate = address_octate[address_split.Length - 1] + 1,
                _3rd_octate = address_octate[2], 
                _2nd_octate = address_octate[1],
                _1st_octate = address_octate[0];
            
            string ip = Convert.ToString(_1st_octate) + "." + Convert.ToString(_2nd_octate) + "." +
                        Convert.ToString(_3rd_octate) + "." + Convert.ToString(_4th_octate);
            return ip;          

        }
        public string next_ime1_number(string ime1_number)
        {
            int i = 0, j, n, index = 0, id = -1;
            char temp;
            n = ime1_number.Length;
            //Console.WriteLine("value of n: {0}", n);
            StringBuilder str = new StringBuilder(ime1_number);
            for (i = 0; i < n - 1; i++)
            {
                if (str[i] <= str[i + 1])
                {
                    id = i;
                }
            }

            if (id == -1)
                return str.ToString();
            //Console.WriteLine("value of i: {0}", i);
            for (j = id + 1; j < n; j++)
            {
                if (str[j] > str[id])
                    index = j;
            }
            temp = str[id];
            str[id] = str[index];
            str[index] = temp;
            i = id + 1; j = n - 1;
            while (i <= j)
            {
                temp = str[i];
                str[i] = str[j];
                str[j] = temp;
                i++; j--;
            }
            return str.ToString();

        }    
        public string next_mobile_number(string mobile_number)
        {
            int i = 0, j, n, index = 0, id = -1;
            char temp;
            n = mobile_number.Length;
            //Console.WriteLine("value of n: {0}", n);
            StringBuilder str = new StringBuilder(mobile_number);
            for (i = 0; i < n - 1; i++)
            {
                if (str[i] <= str[i + 1])
                {
                    id = i;
                }
            }

            if (id == -1)
                return str.ToString();
            //Console.WriteLine("value of i: {0}", i);
            for (j = id + 1; j < n; j++)
            {
                if (str[j] > str[id])
                    index = j;
            }
            temp = str[id];
            str[id] = str[index];
            str[index] = temp;
            i = id + 1; j = n - 1;
            while (i <= j)
            {
                temp = str[i];
                str[i] = str[j];
                str[j] = temp;
                i++; j--;
            }
            return str.ToString();

        }
      
        public string next_mac(string mac)
        {
            int i = 0, j, n, index = 0, id = -1;
            char temp;
            n = mac.Length;
            //Console.WriteLine("value of n: {0}", n);
            StringBuilder str = new StringBuilder(mac);
            for (i = 0; i < n - 1; i++)
            {
                if (str[i] <= str[i + 1])
                {
                    id = i;
                }
            }

            if (id == -1)
                return str.ToString();
            //Console.WriteLine("value of i: {0}", i);
            for (j = id + 1; j < n; j++)
            {
                if (str[j] > str[id])
                    index = j;
            }
            temp = str[id];
            str[id] = str[index];
            str[index] = temp;
            i = id + 1; j = n - 1;
            while (i <= j)
            {
                temp = str[i];
                str[i] = str[j];
                str[j] = temp;
                i++; j--;
            }
            return str.ToString();
        }
     
        public string subnet_mask()
        {
            return "255.255.0.0";
        }
     
        public void add_UPF(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location, DEVICE_CONTAINER[] device_container)
        {
           
            XmlNode device = nsWriter.add_device_element_from_file_with_format( Doc, parent, config_helper_location + "\\5G_NR\\UPF\\Device.txt",
                  Convert.ToString(device_attribute.DEVICE_ID),
                  Convert.ToString(device_attribute.DEVICE_NAME),
                  Convert.ToString(device_attribute.DEVICE_TYPE),
                  Convert.ToString(device_attribute.INTERFACE_COUNT),                  
                  Convert.ToString(pos_3d_attribute.X_OR_LON),
                  Convert.ToString(pos_3d_attribute.Y_OR_LAT),
                  Convert.ToString(pos_3d_attribute.Z)) ; 

            for (int i = 0; i < device_attribute.INTERFACE_COUNT; i++)
            {
                if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N4"))
                {
                   
                    nsWriter.add_element_from_file_with_format( Doc, device, config_helper_location + "\\5G_NR\\UPF\\5G_N4\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }

                else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N3"))
                {
                    nsWriter.add_element_from_file_with_format( Doc, device, config_helper_location + "\\5G_NR\\UPF\\5G_N3\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }

                else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N6"))
                {
                    nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\UPF\\5G_N6\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }
            }

            nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\UPF\\Application_layer.txt",
                         Convert.ToString(device_container[0].application.pfcp_upf_smf), 
                         Convert.ToString(device_container[0].application.gtpu_upf_gnb));
          //  nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\UPF\\Application_layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\UPF\\Tranport_layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\UPF\\Network_layer.txt");
        }
        public void add_SMF(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location, DEVICE_CONTAINER[] device_container)
        {          
            XmlNode device = nsWriter.add_device_element_from_file_with_format(Doc, parent, config_helper_location + "\\5G_NR\\SMF\\Device.txt",
                  Convert.ToString(device_attribute.DEVICE_ID),
                  Convert.ToString(device_attribute.DEVICE_NAME),
                  Convert.ToString(device_attribute.DEVICE_TYPE),
                  Convert.ToString(device_attribute.INTERFACE_COUNT),
                  Convert.ToString(pos_3d_attribute.X_OR_LON),
                  Convert.ToString(pos_3d_attribute.Y_OR_LAT),
                  Convert.ToString(pos_3d_attribute.Z));

            for (int i = 0; i < device_attribute.INTERFACE_COUNT; i++)
            {
                if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N11"))
                {
                    nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\SMF\\5G_N11\\Interface.txt",                         
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }
                else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N4"))
                {
                    nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\SMF\\5G_N4\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }
            }

            nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\SMF\\Application_layer.txt",
                         Convert.ToString(device_container[1].application.gtpc_smf_amf),
                         Convert.ToString(device_container[1].application.pfcp_smf_upf));
            //nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\SMF\\Application_layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\SMF\\Transport_Layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\SMF\\Network_layer.txt");
        }
        public void add_AMF(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location, DEVICE_CONTAINER[] device_container)
        {            
            XmlNode device = nsWriter.add_device_element_from_file_with_format(Doc, parent, config_helper_location + "\\5G_NR\\AMF\\Device.txt",
                  Convert.ToString(device_attribute.DEVICE_ID),
                  Convert.ToString(device_attribute.DEVICE_NAME),
                  Convert.ToString(device_attribute.DEVICE_TYPE),
                  Convert.ToString(device_attribute.INTERFACE_COUNT),
                  Convert.ToString(pos_3d_attribute.X_OR_LON),
                  Convert.ToString(pos_3d_attribute.Y_OR_LAT),
                  Convert.ToString(pos_3d_attribute.Z));

            for (int i = 0; i < device_attribute.INTERFACE_COUNT; i++)
            {
                if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N11"))
                {
                    nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\AMF\\5G_N11\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }
                else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N1_N2"))
                {
                    nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\AMF\\5G_N1_N2\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
                }
            }

            nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\AMF\\Application_layer.txt",
                       Convert.ToString(device_container[2].application.ngap_amf_gnb),
                       Convert.ToString(device_container[2].application.gtpc_amf_smf));
            //nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\AMF\\Application_layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\AMF\\Transport_Layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\AMF\\Network_layer.txt");
        }
        public void add_Switches(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location)
        {          
            XmlNode device = nsWriter.add_device_element_from_file_with_format( Doc, parent, config_helper_location + "\\5G_NR\\Switch\\Device.txt",
                  Convert.ToString(device_attribute.DEVICE_ID),
                  Convert.ToString(device_attribute.DEVICE_NAME),
                  Convert.ToString(device_attribute.DEVICE_TYPE),
                  Convert.ToString(device_attribute.INTERFACE_COUNT),
                  Convert.ToString(device_attribute.TYPE),
                  Convert.ToString(pos_3d_attribute.X_OR_LON),
                  Convert.ToString(pos_3d_attribute.Y_OR_LAT),
                  Convert.ToString(pos_3d_attribute.Z));

            for (int i = 0; i < device_attribute.INTERFACE_COUNT; i++)
            {
                if (interface_variables[i].INTERFACE_TYPE.Equals("ETHERNET"))
                {
                    nsWriter.add_element_from_file_with_format( Doc, device, config_helper_location + "\\5G_NR\\Switch\\Ethernet\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].INTERFACE_TYPE),
                          Convert.ToString(interface_variables[i].MAC_ADDRESS),
                          Convert.ToString(interface_variables[i].SWITCH_ID),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));

                }
            }            
        }
        public void add_GNB(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location, string ngap_gnb_amf, string ngap_gnb_smf, string gtpu_gnb_upf)
        {
            foreach (XmlNode device in Doc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@ DEVICE_ID = " + device_attribute.DEVICE_ID + "]")) 
            {
                XmlElement device_1 = device["POS_3D"];

                for (int i = device_attribute.INTERFACE_COUNT - 1; i > -1; i--)
                {
                    if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N3"))
                    {
                        XmlNode n3_interface = nsWriter.add_interface_element_from_file_with_format(Doc, device, device_1, config_helper_location + "\\5G_NR\\gNB\\5G_N3\\Interface.txt",
                             Convert.ToString(interface_variables[i].ID),
                             Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                             Convert.ToString(interface_variables[i].IP_ADDRESS),
                             Convert.ToString(interface_variables[i].SUBNET_MASK),
                             Convert.ToString(interface_variables[i].MAC_ADDRESS),
                             Convert.ToString(interface_variables[i].CONNECTED_TO));

                    }
                    else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_N1_N2"))
                    {
                        nsWriter.add_interface_element_from_file_with_format(Doc, device, device_1, config_helper_location + "\\5G_NR\\gNB\\5G_N1_N2\\Interface.txt",
                             Convert.ToString(interface_variables[i].ID),
                             Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                             Convert.ToString(interface_variables[i].IP_ADDRESS),
                             Convert.ToString(interface_variables[i].SUBNET_MASK),
                             Convert.ToString(interface_variables[i].MAC_ADDRESS),
                              Convert.ToString(interface_variables[i].CONNECTED_TO));
                    }
                    else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_XN"))
                    {
                        XmlNode xn_interface = nsWriter.add_interface_element_from_file_with_format(Doc, device, device_1, config_helper_location + "\\5G_NR\\gNB\\5G_XN\\Interface.txt",
                            Convert.ToString(interface_variables[i].ID),
                             Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                             Convert.ToString(interface_variables[i].IP_ADDRESS),
                             Convert.ToString(interface_variables[i].SUBNET_MASK),
                             Convert.ToString(interface_variables[i].MAC_ADDRESS),
                              Convert.ToString(interface_variables[i].CONNECTED_TO));
                    }
                    else if (interface_variables[i].INTERFACE_TYPE.Equals("5G_RAN"))
                    {

                        foreach (XmlNode gnb_ran_interface in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                        {
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "ASSOCIATE_AMF", "AMF_3");
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "ASSOCIATE_SMF", "SMF_2");
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "ASSOCIATE_UPF", "UPF_1");
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "ISNSAENABLE", "FALSE");
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "MASTER_NODE_TYPE", "Non_mmWave_Cell");
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "SECONDERY_NODE_TYPE", "mmWave_Cell");
                            nsWriter.add_attribute(Doc, gnb_ran_interface, "SELECTED_OPTION_TYPE", "");
                        }

                        foreach (XmlNode gnb_ran_physical_layer in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='PHYSICAL_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                        {
                            nsWriter.remove_attribute(gnb_ran_physical_layer, "DOWNLINK_MIMO_LAYER_COUNT");
                            nsWriter.remove_attribute(gnb_ran_physical_layer, "UPLINK_MIMO_LAYER_COUNT");

                            XmlNode propagation_model = gnb_ran_physical_layer.SelectSingleNode("descendant::PROPAGATION_MODEL");
                            XmlNode channel_characteristics = gnb_ran_physical_layer.SelectSingleNode("descendant::CHANNEL_CHARACTERISTICS");

                            
                            if (channel_characteristics.Attributes["CHANNEL_CHARACTERISTICS"] != null && channel_characteristics.Attributes["CHANNEL_CHARACTERISTICS"].Value != null)
                            {
                                var channel_characteristic = channel_characteristics.Attributes["CHANNEL_CHARACTERISTICS"].Value;

                                if (channel_characteristics.Attributes["LOS_MODE"] != null && channel_characteristics.Attributes["LOS_MODE"].Value != null)
                                {
                                    var los_mode = channel_characteristics.Attributes["LOS_MODE"].Value;


                                    XmlNode channel_model = nsWriter.add_element(Doc, gnb_ran_physical_layer, "CHANNEL_MODEL");

                                    if (channel_characteristic == "NO_PATHLOSS")
                                    {
                                        nsWriter.add_attribute(Doc, channel_model, "PATHLOSS_MODEL", "NONE");                                      
                                    }
                                    else
                                    {
                                        nsWriter.add_attribute(Doc, channel_model, "PATHLOSS_MODEL", "3GPPTR38.901-7.4.1");
                                        if (propagation_model.Attributes["BUILDINGS_HEIGHT"] != null)
                                        {
                                            var building_height = propagation_model.Attributes["BUILDINGS_HEIGHT"].Value;
                                            nsWriter.add_attribute(Doc, channel_model, "BUILDINGS_HEIGHT", building_height);
                                        }
                                        
                                        if (propagation_model.Attributes["OUTDOOR_SCENARIO"] != null)
                                        {
                                            var outdoor_scenario = propagation_model.Attributes["OUTDOOR_SCENARIO"].Value;
                                            nsWriter.add_attribute(Doc, channel_model, "OUTDOOR_SCENARIO", outdoor_scenario);
                                        }
                                        if (propagation_model.Attributes["STREET_WIDTH"] != null)
                                        {
                                            var street_width = propagation_model.Attributes["STREET_WIDTH"].Value;
                                            nsWriter.add_attribute(Doc, channel_model, "STREET_WIDTH", street_width);
                                        }
                                                                                                                                                    
                                        nsWriter.add_attribute(Doc, channel_model, "INDOOR_SCENARIO", "INDOOR_OFFICE");

                                        if (los_mode == "TR38_901_STANDARD") 
                                            nsWriter.add_attribute(Doc, channel_model, "LOS_NLOS_SELECTION", "3GPPTR38.901-Table7.4.2-1");
                                        else if (los_mode == "USER_DEFINED")
                                        {
                                            if (channel_characteristics.Attributes["LOS_PROBABILITY"] != null && channel_characteristics.Attributes["LOS_PROBABILITY"].Value != null)
                                            {
                                                var los_probability = channel_characteristics.Attributes["LOS_PROBABILITY"].Value;
                                                nsWriter.add_attribute(Doc, channel_model, "LOS_NLOS_SELECTION", "USER_DEFINED");
                                                nsWriter.add_attribute(Doc, channel_model, "LOS_PROBABILITY", los_probability);
                                            }
                                        }

                                        if (channel_characteristic == "PATHLOSS_ONLY")
                                        {
                                            nsWriter.add_attribute(Doc, channel_model, "SHADOWFADING_MODEL", "NONE");
                                            nsWriter.add_attribute(Doc, channel_model, "FASTFADING_MODEL", "NO_FADING");
                                            nsWriter.add_attribute(Doc, channel_model, "O2I_BUILDING_PENETRATION_MODEL", "NONE");
                                        }

                                        else if (channel_characteristic == "PATHLOSS_WITH_SHADOWFADING")
                                        {
                                            nsWriter.add_attribute(Doc, channel_model, "SHADOWFADING_MODEL", "LOG_NORMAL");
                                            nsWriter.add_attribute(Doc, channel_model, "SHADOWFADING_STANDARD_DEVIATION", "3GPPTR38.901-Table7.4.1-1");
                                            nsWriter.add_attribute(Doc, channel_model, "FASTFADING_MODEL", "NO_FADING");
                                            nsWriter.add_attribute(Doc, channel_model, "O2I_BUILDING_PENETRATION_MODEL", "NONE");
                                        }

                                        else if (channel_characteristic == "PATHLOSS_WITH_SHADOWFADING_O2ILOSS")
                                        {
                                            nsWriter.add_attribute(Doc, channel_model, "SHADOWFADING_MODEL", "LOG_NORMAL");
                                            nsWriter.add_attribute(Doc, channel_model, "SHADOWFADING_STANDARD_DEVIATION", "3GPPTR38.901-Table7.4.1-1");
                                            nsWriter.add_attribute(Doc, channel_model, "FASTFADING_MODEL", "NO_FADING");
                                            if (propagation_model.Attributes["O2I_MODEL"] != null)
                                            {
                                                var O2I_model = propagation_model.Attributes["O2I_MODEL"].Value;
                                                nsWriter.add_attribute(Doc, channel_model, "O2I_BUILDING_PENETRATION_MODEL", O2I_model);
                                            }                                          
                                        }
                                    }
                                    propagation_model.ParentNode.RemoveChild(propagation_model);
                                    channel_characteristics.ParentNode.RemoveChild(channel_characteristics);
                                }
                            }
                        }
                    }
                }
                //S1 node removal
               foreach( XmlNode gnb_S1_node in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_S1']"))
                gnb_S1_node.ParentNode.RemoveChild(gnb_S1_node);

                //Removing associate epc information 
                foreach (XmlNode parent_lte_node in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER/PROTOCOL/PROTOCOL_PROPERTY"))
                {
                    if (parent_lte_node.Attributes != null)
                        parent_lte_node.Attributes.Remove(parent_lte_node.Attributes["ASSOCIATED_EPC"]);
                }

                //Removing application layer of gNB
                foreach(XmlNode gnb_5gnr_app_layer in device.SelectNodes("descendant::LAYER[@TYPE ='APPLICATION_LAYER']"))
                    gnb_5gnr_app_layer.ParentNode.RemoveChild(gnb_5gnr_app_layer);

                //Changing LTE_NR interface to 5G_RAN 
                foreach (XmlNode gnb_5gnr_node in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']"))
                {
                    if (gnb_5gnr_node.Attributes != null)
                    {
                        gnb_5gnr_node.Attributes["INTERFACE_TYPE"].Value = "5G_RAN";
                        gnb_5gnr_node.Attributes["ID"].Value = "4";

                    }
                }

                nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\gnB\\Application_layer.txt",
                         Convert.ToString(ngap_gnb_amf),
                         Convert.ToString(ngap_gnb_smf),
                         Convert.ToString(gtpu_gnb_upf));

                //nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\gNB\\Application_layer.txt");
                nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\gNB\\Transport_Layer.txt");
            }
        }
        public void add_UE(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location)
        {
            foreach (XmlNode device in Doc.SelectNodes("descendant::NETWORK_CONFIGURATION/DEVICE_CONFIGURATION/DEVICE[@ DEVICE_ID = " + device_attribute.DEVICE_ID + "]"))
            {
                foreach (XmlNode ue_ran_interface in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']/LAYER[@TYPE='DATALINK_LAYER']/PROTOCOL/PROTOCOL_PROPERTY"))
                {
                    nsWriter.add_attribute(Doc, ue_ran_interface, "ISNSAENABLE", "FALSE");
                    nsWriter.add_attribute(Doc, ue_ran_interface, "MASTER_NODE_TYPE", "Non_mmWave_Cell");
                    nsWriter.add_attribute(Doc, ue_ran_interface, "SECONDERY_NODE_TYPE", "mmWave_Cell");
                    nsWriter.add_attribute(Doc, ue_ran_interface, "SELECTED_OPTION_TYPE", "");
                }

                foreach (XmlNode ue_5gnr_node in device.SelectNodes("descendant::INTERFACE[@INTERFACE_TYPE='LTE_NR']"))
                {
                    if (ue_5gnr_node.Attributes != null)
                        ue_5gnr_node.Attributes["INTERFACE_TYPE"].Value = "5G_RAN";
                }

                nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\UE\\Network_Layer.txt");
            }
        }
        public void add_router(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location)
        {
            foreach (XmlNode device in Doc.SelectNodes("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION/DEVICE[@ DEVICE_ID = " + device_attribute.DEVICE_ID + "]"))
            {                
                XmlElement device_1 = device["POS_3D"];

                for (int i = device_attribute.INTERFACE_COUNT - 1; i > -1; i--)
                {
                    if (interface_variables[i].INTERFACE_TYPE.Equals("SERIAL"))
                    {
                        nsWriter.add_interface_element_from_file_with_format(Doc, device, device_1, config_helper_location + "\\5G_NR\\Router\\Serial\\Interface.txt",
                               Convert.ToString(interface_variables[i].ID),
                               Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                               Convert.ToString(interface_variables[i].IP_ADDRESS),
                               Convert.ToString(interface_variables[i].SUBNET_MASK),
                               Convert.ToString(interface_variables[i].MAC_ADDRESS),
                               Convert.ToString(interface_variables[i].CONNECTED_TO));
                    }
                }
            }
        }
        public void add_router_new(XmlDocument Doc, XmlNode parent, DEVICE device_attribute, POS_3D pos_3d_attribute, INTERFACE[] interface_variables, string config_helper_location)
        {
            XmlNode device = nsWriter.add_device_element_from_file_with_format(Doc, parent, config_helper_location + "\\5G_NR\\Router\\Device.txt",
                 Convert.ToString(device_attribute.DEVICE_ID),
                 Convert.ToString(device_attribute.DEVICE_NAME),
                 "ROUTER",
                 Convert.ToString(device_attribute.INTERFACE_COUNT),
                 Convert.ToString(pos_3d_attribute.X_OR_LON),
                 Convert.ToString(pos_3d_attribute.Y_OR_LAT),
                 Convert.ToString(pos_3d_attribute.Z));

            for (int i = 0; i < device_attribute.INTERFACE_COUNT; i++)
            {
                string folder;
                if (interface_variables[i].INTERFACE_TYPE.Equals("SERIAL"))
                    folder = "Serial";
                else
                    folder = "Ethernet";

                nsWriter.add_element_from_file_with_format(Doc, device, config_helper_location + "\\5G_NR\\Router\\" + folder + "\\Interface.txt",
                         Convert.ToString(interface_variables[i].ID),
                         Convert.ToString(interface_variables[i].DEFAULT_GATEWAY),
                         Convert.ToString(interface_variables[i].IP_ADDRESS),
                         Convert.ToString(interface_variables[i].SUBNET_MASK),
                         Convert.ToString(interface_variables[i].MAC_ADDRESS),
                         Convert.ToString(interface_variables[i].CONNECTED_TO));
            }
            XmlNode routing_protocol = nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\5G_NR\\Router\\Application_layer.txt");
            XmlNode protocol_property = nsWriter.add_element_from_file(Doc, routing_protocol, config_helper_location + "\\5G_NR\\Router\\Application_Layer_Routing_Protocol.txt");
            XmlNode property_interface = nsWriter.add_element_from_file(Doc, protocol_property, config_helper_location + "\\5G_NR\\Router\\Application_Layer_Routing_Protocol_Property.txt");
            
            for (int i = 0; i < device_attribute.INTERFACE_COUNT; i++)
            {
                if (interface_variables[i].INTERFACE_TYPE.Equals("SERIAL"))
                    nsWriter.add_element_from_file_with_format(Doc, property_interface, config_helper_location + "\\5G_NR\\Router\\Application_Layer_Routing_Protocol_Property_Interface_Wan.txt",
                    Convert.ToString(interface_variables[i].ID));
                else
                    nsWriter.add_element_from_file_with_format(Doc, property_interface, config_helper_location + "\\5G_NR\\Router\\Application_Layer_Routing_Protocol_Property_Interface_Ethernet.txt",
                         Convert.ToString(interface_variables[i].ID));
            }
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\ConfigHelper\\Router\\Transport_Layer.txt");
            nsWriter.add_element_from_file(Doc, device, config_helper_location + "\\ConfigHelper\\Router\\Network_Layer.txt");
        }
        public void add_deviceConfig(XmlDocument Doc, XmlNode parent, int device_count, DEVICE_CONTAINER[] device_container,string config_helper_location)
        {            
            XmlNode deviceCount = Doc.SelectSingleNode("descendant::NETWORK_CONFIGURATION /DEVICE_CONFIGURATION");
            deviceCount.Attributes["DEVICE_COUNT"].Value = Convert.ToString(device_count);

            for (int i = device_count -1 ; i > -1 ; i--)
            {
                if (device_container[i].device.DEVICE_TYPE.Equals("LTE_NR_UE"))
                    add_UE(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location);
                else if (device_container[i].device.DEVICE_TYPE.Equals("LTE_gNB"))
                    add_GNB(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location, device_container[i].application.ngap_gnb_amf, device_container[i].application.ngap_gnb_smf, device_container[i].application.gtpu_gnb_upf);  
                else if (device_container[i].device.DEVICE_TYPE.Equals("UPF"))
                    add_UPF(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location, device_container);
                else if (device_container[i].device.DEVICE_TYPE.Equals("SMF"))
                    add_SMF(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location, device_container);
                else if (device_container[i].device.DEVICE_TYPE.Equals("AMF"))
                    add_AMF(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location, device_container);
                else if (device_container[i].device.DEVICE_TYPE.Equals("L2_SWITCH_UPF") || device_container[i].device.DEVICE_TYPE.Equals("L2_SWITCH_AMF") || device_container[i].device.DEVICE_TYPE.Equals("L2_SWITCH_gNB"))
                    add_Switches(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location);
                else if (device_container[i].device.DEVICE_TYPE.Equals("ROUTER"))
                    add_router(Doc, parent, device_container[i].device, device_container[i].pos_3d, device_container[i]._interface, config_helper_location);             
            }
        }
        public void add_connection(XmlDocument Doc, XmlNode con, int link_count, LINK[] link, string config_helper_location)
        {
           // XmlNode con = nsWriter.add_element(parent, "CONNECTION");
            for (int i = link_count - 1; i > -1; i--)
            {
                XmlNode link_element; //string folder_name;
                if (link[i].link_type.Equals("SMF_AMF"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\SMF_AMF\\Link.txt",
                     Convert.ToString(link[i].DEVICE_COUNT),
                     Convert.ToString(link[i].LINK_ID),
                     Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\SMF_AMF\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");
                }
                else if (link[i].link_type.Equals("UPF_SMF"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\UPF_SMF\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\UPF_SMF\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");
                }
                else if (link[i].link_type.Equals("UPF_Switch"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\UPF_Switch\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\UPF_Switch\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");

                }
                else if (link[i].link_type.Equals("AMF_Swicth"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\AMF_Swicth\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\AMF_Swicth\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");

                }
                else if (link[i].link_type.Equals("gNB_Switch1"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\gNB_Switch1\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\gNB_Switch1\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");
                }
                else if (link[i].link_type.Equals("gNB_Switch2"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\gNB_Switch2\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\gNB_Switch2\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");
                }
                else if (link[i].link_type.Equals("gNB_Switch3"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\gNB_Switch3\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\gNB_Switch3\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");
                }
                else if (link[i].link_type.Equals("UPF_Router"))
                {
                    link_element = nsWriter.add_device_element_from_file_with_format(Doc, con, config_helper_location + "\\5G_NR\\Link\\UPF_Router\\Link.txt",
                    Convert.ToString(link[i].DEVICE_COUNT),
                    Convert.ToString(link[i].LINK_ID),
                    Convert.ToString(link[i].LINK_NAME));
                    for (int j = 0; j < link[i].DEVICE_COUNT; j++)
                    {
                        nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Device.txt",
                        Convert.ToString(link[i].link_device[j].DEVICE_ID),
                        Convert.ToString(link[i].link_device[j].INTERFACE_ID),
                        Convert.ToString(link[i].link_device[j].NAME));
                    }
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\UPF_Router\\Medium_Property.txt");
                    nsWriter.add_element_from_file_with_format(Doc, link_element, config_helper_location + "\\5G_NR\\Link\\Link_Failure.txt");
                }               
            }
        }
        public void add_network(XmlDocument Doc, XmlNode parent, XmlNode nwConfig, int device_count, int link_count,  DEVICE_CONTAINER[] device_container, LINK[] link, string config_helper_location)
        {           
            add_deviceConfig(Doc, parent, device_count, device_container, config_helper_location);
            add_connection(Doc, nwConfig, link_count, link, config_helper_location );
           // add_application(nwConfig, application_count);
        }
    }
}
