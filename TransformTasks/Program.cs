/******************************
 * 
 * @Author: Gabriel Rodriguez
 * Date: 3/10/2018
 * project: Transform Tasks
 * 
 * Transform Tasks is intendend to be a C# approach to handling JSON Objects. The JSON file is parsed to the .EXE through the command line.
 * the know variable are then handled accordingly by deserializing the objects and placing the values in dictionaries. 
 * 
 * 
 * 
 * **************************/

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;



namespace TransformTasks
{
    class Program
    {
        //declarations
        //seperate counting int used for dictionary storage, this will vary depending upon which nodes hold each value
        //the count will be used to loop through each dictionary to avoid going "Out Of Bounds"
        public static Dictionary<int, string> appType = new Dictionary<int, string>();
        public static Dictionary<int, string> version = new Dictionary<int, string>();
        public static Dictionary<int, string> roleType = new Dictionary<int, string>();
        public static Dictionary<int, string> location = new Dictionary<int, string>();
        public static Dictionary<int, SerializeTask> task = new Dictionary<int, SerializeTask>();
        public static List<string> stdOutApp = new List<string>();
        public static List<string> stdOutVersion = new List<string>();
        public static List<string> stdOutRole = new List<string>();
        public static List<string> stdOutLoc = new List<string>();
        public static string fullPath = "";
        public static int count = 1;
        public static int countApp = 0;
        public static int countVer = 0;
        public static int countRole = 0;
        public static int countLoc = 0;

        static void Main(string[] args)
        {
            //used while testing

            //fullPath = "C:\\Users\\gabriel\\Desktop\\Code Examples\\TransformTasks\\TransformTasks\\bin\\Debug\\test.json";

            /*
                   used during deployment 
            */

            //current directory is used assuming the call the the .exe will also house the path to the .JSON file
            string path = Directory.GetCurrentDirectory();
            //get the name of the file that is piped when the program is called
            string file = Console.ReadLine();
            if(path.Length > 3)
            {
                //concat the path and command line param
                string fullPath = path + "\\" + file;
                SetStreamIn(fullPath);
                SetStreamOut();
                Console.Read();
           }
            

        }
        /*
         * open JSON file and store stdin value in dictionaries in preparation of output
         * TODO
         * test building objects with serializeTasks storing objects in dictionaries 
         * so they are ready for stdout
        */
        public static void SetStreamIn(string p)
        {
            try
            {
                //open file using stream reader
                using (StreamReader r = File.OpenText(p))
                {
                    //read entire file content
                    string json = r.ReadToEnd();
                    string name = "";
                    string app = "";
                    string vers = "";
                    string role = "";
                    string loc = "";

                    //parse to object using string
                    JObject jobj = JObject.Parse(json);

                    //iterate through object to gather key values
                    foreach (JProperty property in jobj.Properties())
                    {
                        name = "node" + (count);

                        //check for null values and skip them if they aren't used
                        //values based upon sample data of known input
                        //node values increment real values would likely need the program to find the name of the initial key
                        if (jobj.SelectToken(name + ".application") != null)
                        {
                            app = jobj.SelectToken(name + ".application").ToString();
                           
                            appType.Add(countApp, app );
                            countApp++;
                        }
                        if(jobj.SelectToken(name + ".version") != null)
                        {
                            vers = (string)jobj.SelectToken(name + ".version");
                          
                            version.Add(countVer, vers );
                            countVer++;
                        }
                        if(jobj.SelectToken(name + ".role") != null)
                        {
                            role = (string)jobj.SelectToken(name + ".role");
                        
                            roleType.Add(countRole, role );
                            countRole++;
                        }
                        if (jobj.SelectToken(name + ".location") != null)
                        {
                            loc = (string)jobj.SelectToken(name + ".location");
                            
                            location.Add(countLoc, loc );
                            countLoc++;
                        }
                        count++;
                     }
                }
            }
            catch (IOException ex)
            {
                IOException except = new IOException("An I/O error occurred while opening the file.", ex);
                throw except;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //create new file to write to for STDOUT 
        //Call to method with Dictionary, List, Int parameters to write to file for each dictionary 
        public static void SetStreamOut()
        {
            try
            {
                WriteToFile(appType, stdOutApp, countApp);
                WriteToFile(version, stdOutVersion, countVer);
                WriteToFile(roleType, stdOutRole, countRole);
                WriteToFile(location, stdOutLoc, countLoc);
                Console.Read();
            }
            catch (IOException ex)
            {
                IOException except = new IOException("An I/O error occurred while opening the file.", ex);
                throw except;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
         * static method to write to file
         * TODO
         * Write to file using streamwriter 
         * 
         */
        public static void WriteToFile(Dictionary<int,string> name,List<string> stdOut, int number)
        {
            List<string> sNodes = new List<string>(); ;
            int counter = 0;
            int j = 0;
            string version = "version";
            string ver = "";
            string otherVersion = "";
            string newPath = fullPath + "\\myNewFile.JSON";
            try
            {
                
                //loop while the count is less than the int argument---> param from the dictionaries (set previously in SetStreamIn) 
                while (counter < number)
                {
                    //for loop will start count over each time a new dictionary element is accessed until all elements are processed
                    for (int k = 0; k < number; k++)
                    {
                        //local string set to the value of the dictionary key position j
                        ver = name[j];
                        
                        //set initial output to file
                        if (k == 0)
                        {
                            Console.WriteLine(ver);
                            
                        }
                        //compare strings
                        if (name[k].Equals(ver))
                        { 
                            Console.WriteLine("\n\tNode{0}", k);
                            counter++;
                            //sNodes.Add("Node"+ k);
                        }
                        else
                        {
                            if (otherVersion == "")
                            {
                                otherVersion = name[k];
                                stdOut.Add(otherVersion);
                                Console.WriteLine("{0}", otherVersion);
                                Console.WriteLine("\n\tNode{0}", k);
                            }
                            else
                            {
                                foreach (string key in stdOut)
                                {
                                    if (!key.Equals(otherVersion))
                                    {
                                        stdOut.Add(otherVersion);
                                        otherVersion = name[k];
                                        Console.WriteLine("{0}", otherVersion);
                                        Console.WriteLine("\n\tNode{0}", k);
                                    }
                                }
                                if (counter < number)
                                {
                                    j++;
                                }
                            }
                            counter++;
                        }
                    }
                }
                //using (StreamWriter sw = File.CreateText(newPath))
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

